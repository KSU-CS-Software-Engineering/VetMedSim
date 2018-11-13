using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UserInput
{
	#region Gesture Gatherer State Machine

	/// <summary>
	/// Represents base abstract gesture input state
	/// </summary>
	internal abstract class InputState
	{
		#region Properties

		/// <summary>
		/// Reference to the <see cref="GestureHandler"/> owner object
		/// </summary>
		protected GestureHandler Handler;

		/// <summary>
		/// The time current state started
		/// </summary>
		protected readonly float StartTime;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs base input state
		/// </summary>
		/// <param name="handler">Reference to the <see cref="GestureHandler"/> owner object</param>
		protected InputState( GestureHandler handler )
		{
			Handler = handler;
			StartTime = Time.time;
		}

		#endregion

		#region Functions

		/// <summary>
		/// Responds to situation when there is new unregistered input
		/// </summary>
		internal virtual void OnNewInput()
		{
			if( Handler.ActiveGesture != null )
			{
				Handler.FinishGesture();
			}

			Handler.ChangeState( new GatheringState( Handler ) );
		}

		/// <summary>
		/// Handles the updating of currently active touches and gesture
		/// </summary>
		internal abstract void OnUpdate();

		/// <summary>
		/// Checks, whether time elapsed in this state has crossed given threshold
		/// </summary>
		/// <param name="threshold">Threshold to check</param>
		/// <returns>True in case state is up longer than given threshold, false otherwise</returns>
		internal bool HasTimePassed( float threshold )
		{
			return Time.time - StartTime >= threshold;
		}

		/// <summary>
		/// Checks, whether given <see cref="Touch"/> is stationary compared to last frame
		/// </summary>
		/// <param name="touch"><see cref="Touch"/> to be checked</param>
		/// <returns>True in case the delta of <see cref="Touch"/> position is lesser than given threshold, false otherwise</returns>
		internal bool IsStationary( Touch touch )
		{
			return touch.deltaPosition.magnitude <= Handler.StationaryDistanceThreshold;
		}

		#endregion
	}

	/// <summary>
	/// Represents input state, when no input is being processed
	/// </summary>
	internal class IdleState : InputState
	{
		#region Constructors

		/// <summary>
		/// Constructs base idle state
		/// </summary>
		/// <param name="handler">Reference to the <see cref="GestureHandler"/> owner object</param>
		internal IdleState( GestureHandler handler ) : base( handler )
		{
			// intentionally left blank
		}

		#endregion

		#region Overrides

		internal override void OnUpdate()
		{
			// intentionally left blank
		}

		#endregion
	}

	/// <summary>
	/// Represents input state, which input is being gathered in
	/// </summary>
	internal class GatheringState : InputState
	{
		#region Constructors

		/// <summary>
		/// Constructs base gathering state
		/// </summary>
		/// <param name="handler">Reference to the <see cref="GestureHandler"/> owner object</param>
		internal GatheringState( GestureHandler handler ) : base( handler )
		{
			// intentionally left blank
		}

		#endregion

		#region Overrides

		internal override void OnUpdate()
		{
			if( HasDetectedEarlyTap() )
			{
				Handler.ChangeState( new TapState( Handler ) );

				return;
			}

			if( !HasTimePassed( Handler.ReactionTimeThreshold ) )
			{
				return;
			}

			if( Handler.ActiveTouches.Count == 1 )
			{
				Handler.ChangeState( RecognizeSingleFingerGesture() );
			}

			if( Handler.ActiveTouches.Count >= 2 )
			{
				Handler.ChangeState( RecognizeDoubleFingerGesture() );
			}
		}

		#endregion

		#region Functions

		/// <summary>
		/// Checks, wether the user has finished the touch within gathering period
		/// </summary>
		/// <returns>True in case early tap is detected, false otherwise</returns>
		private bool HasDetectedEarlyTap()
		{
			return Handler.ActiveTouches.Count == 1
				&& Handler.ActiveTouchFlags[Handler.MainTouch.fingerId].HasEnded;
		}

		/// <summary>
		/// Recognizes currenlty drawn single-finger gesture and switches to appropriate state
		/// </summary>
		/// <returns>Appropriate <see cref="GathererState"/> to switch to</returns>
		private InputState RecognizeSingleFingerGesture()
		{
			var touch = Handler.MainTouch;
			var flags = Handler.ActiveTouchFlags[touch.fingerId];

			if( flags.HasEnded )
			{
				return new TapState( Handler );
			}
			else if( flags.HasMoved )
			{
				return new DragState( Handler );
			}
			else
			{
				return new HoldState( Handler );
			}
		}

		/// <summary>
		/// Recognizes currenlty drawn double-finger gesture and switches to appropriate state
		/// </summary>
		/// <returns>Appropriate <see cref="GathererState"/> to switch to</returns>
		private InputState RecognizeDoubleFingerGesture()
		{
			return new PinchState( Handler );
		}

		#endregion
	}

	/// <summary>
	/// Represents input state, which taps are being registered in
	/// </summary>
	internal class TapState : InputState
	{
		#region Fields

		/// <summary>
		/// Current number of registered taps
		/// </summary>
		private int _tapCount;

		/// <summary>
		/// Additional time STM will remain in this state before transitioning out
		/// </summary>
		private float _reactionTimeExtension;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs base tap state
		/// </summary>
		/// <param name="handler">Reference to the <see cref="GestureHandler"/> owner object</param>
		internal TapState( GestureHandler handler ) : base( handler )
		{
			_tapCount = 0;
			_reactionTimeExtension = Handler.TapDifferenceTimeThreshold;
		}

		#endregion

		#region Overrides

		internal override void OnUpdate()
		{
			if( !HasTimePassed( _reactionTimeExtension ) )
			{
				return;
			}

			var touch = Handler.MainTouch;
			if( touch.tapCount != _tapCount )
			{
				_tapCount = touch.tapCount;
				_reactionTimeExtension += Handler.TapDifferenceTimeThreshold;
			}
			else
			{
				Handler.RegisterGesture( new Tap( Handler.FindOrigin( Handler.MainTouch ), _tapCount ) );
				Handler.FinishGesture();

				Handler.UnregisterTouch( touch );
				Handler.ChangeState( new IdleState( Handler ) );
			}
		}

		#endregion
	}

	/// <summary>
	/// Represents input state, which drag motion is being resitered in
	/// </summary>
	internal class DragState : InputState
	{
		#region Fields

		/// <summary>
		/// Currenlty active drag gesture
		/// </summary>
		private readonly Drag _drag;

		/// <summary>
		/// Time the dragged motion has stayed stationary
		/// </summary>
		private float _stationaryTime;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs base drag state
		/// </summary>
		/// <param name="handler">Reference to the <see cref="GestureHandler"/> owner object</param>
		internal DragState( GestureHandler handler ) : base( handler )
		{
			_drag = new Drag( Handler.FindOrigin( Handler.MainTouch ), Handler.MainTouch.position );
			Handler.RegisterGesture( _drag );

			_stationaryTime = 0f;
		}

		#endregion

		#region Overrides

		internal override void OnUpdate()
		{
			var touch = Handler.MainTouch;
			var flags = Handler.ActiveTouchFlags[touch.fingerId];

			if( IsStationary( touch ) )
			{
				_stationaryTime += Time.deltaTime;
			}
			else
			{
				_stationaryTime = 0f;
			}

			if( flags.HasEnded )
			{
				Handler.FinishGesture();

				Handler.UnregisterTouch( touch );
				Handler.ChangeState( new IdleState( Handler ) );
			}
			else if( _stationaryTime >= Handler.StationaryTimeThreshold )
			{
				Handler.FinishGesture();

				Handler.ResetTouchFlags( touch );
				Handler.ChangeState( new HoldState( Handler ) );
			}
			else
			{
				_drag.MoveTo( touch.position );
			}
		}

		#endregion
	}

	/// <summary>
	/// Represents input state, which hold gesture is being registered in
	/// </summary>
	internal class HoldState : InputState
	{
		#region Constructors

		/// <summary>
		/// Contructs base hold state
		/// </summary>
		/// <param name="handler">Reference to the <see cref="GestureHandler"/> owner object</param>
		internal HoldState( GestureHandler handler ) : base( handler )
		{
			Handler.RegisterGesture( new Hold( Handler.FindOrigin( Handler.MainTouch ) ) );
		}

		#endregion

		#region Overrides

		internal override void OnUpdate()
		{
			var touch = Handler.MainTouch;
			var flags = Handler.ActiveTouchFlags[touch.fingerId];

			if( flags.HasEnded )
			{
				Handler.FinishGesture();

				Handler.UnregisterTouch( touch );
				Handler.ChangeState( new IdleState( Handler ) );
			}
			else if( flags.HasMoved )
			{
				Handler.FinishGesture();

				Handler.ResetTouchFlags( touch );
				Handler.ChangeState( new DragState( Handler ) );
			}
		}

		#endregion
	}

	/// <summary>
	/// Represents input state, which pinch gesture is being registered in
	/// </summary>
	internal class PinchState : InputState
	{
		#region Fields

		/// <summary>
		/// Currently active pinch gesture
		/// </summary>
		private readonly Pinch _pinch;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructs base pinch state
		/// </summary>
		/// <param name="handler">Reference to the <see cref="GestureHandler"/> owner object</param>
		internal PinchState( GestureHandler handler ) : base( handler )
		{
			_pinch = new Pinch( Handler.FindOrigin( Handler.MainTouch ), Handler.FindOrigin( Handler.SecondaryTouch ) );
			Handler.RegisterGesture( _pinch );
		}

		#endregion

		#region Overrides

		internal override void OnUpdate()
		{
			var mainTouch = Handler.MainTouch;
			var mainFlags = Handler.ActiveTouchFlags[mainTouch.fingerId];

			var secondaryTouch = Handler.SecondaryTouch;
			var secondaryFlags = Handler.ActiveTouchFlags[secondaryTouch.fingerId];

			if( mainFlags.HasEnded || secondaryFlags.HasEnded )
			{
				Handler.FinishGesture();
				if( mainFlags.HasEnded )
				{
					Handler.UnregisterTouch( mainTouch );
				}

				if( secondaryFlags.HasEnded )
				{
					Handler.UnregisterTouch( secondaryTouch );
				}

				if( Handler.ActiveTouches.Any() )
				{
					Handler.ChangeState( new GatheringState( Handler ) );
				}
				else
				{
					Handler.ChangeState( new IdleState( Handler ) );
				}
			}
			else
			{
				_pinch.MoveTo( mainTouch.position, secondaryTouch.position );
			}
		}

		#endregion
	}

	#endregion

	/// <summary>
	/// Contains flags for touch object
	/// </summary>
	internal class TouchFlags
	{
		#region Properties

		/// <summary>
		/// Indicator, whether associated touch has moved
		/// </summary>
		internal bool HasMoved { get; set; }

		/// <summary>
		/// Indicator, whether associated touch has ended
		/// </summary>
		internal bool HasEnded { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs base collection of touch flags
		/// </summary>
		internal TouchFlags()
		{
			HasMoved = false;
			HasEnded = false;
		}

		#endregion

		#region Functions

		/// <summary>
		/// Resets all the flags to default values
		/// </summary>
		public void Reset()
		{
			HasEnded = false;
			HasMoved = false;
		}

		#endregion
	}

	/// <summary>
	/// Component responsible for gesture recognition
	/// </summary>
	public class GestureHandler : MonoBehaviour
	{
		#region Properties

		#region Public

		/// <summary>
		/// Duration of period during which <see cref="Tap"/> can be registered
		/// </summary>
		public float ReactionTimeThreshold;

		/// <summary>
		/// Duration of period during which additional taps are expected before creating <see cref="Tap"/>
		/// </summary>
		public float TapDifferenceTimeThreshold;

		/// <summary>
		/// Duration of period for which can dragging motion stay stationary before <see cref="Hold"/> is created instead
		/// </summary>
		public float StationaryTimeThreshold;

		/// <summary>
		/// Distance which <see cref="Touch"/> can travel before it is considered a <see cref="Drag"/>
		/// </summary>
		public float StationaryDistanceThreshold;

		/// <summary>
		/// Currently active gesture
		/// </summary>
		public Gesture ActiveGesture { get; private set; }

		#endregion

		#region Internal

		/// <summary>
		/// Current state of handler
		/// </summary>
		internal InputState State { get; private set; }

		/// <summary>
		/// The first registered touch
		/// </summary>
		internal Touch MainTouch => ActiveTouches.Values.ElementAt( 0 );

		/// <summary>
		/// Optional second registered touch (should be accessed only in double-finger gestures)
		/// </summary>
		internal Touch SecondaryTouch => ActiveTouches.Values.ElementAt( 1 );

		/// <summary>
		/// List of touches, which have been added in this frame
		/// </summary>
		internal List<Touch> NewTouches { get; private set; }

		/// <summary>
		/// Dictionary of all active registered touches indexed by their finger IDs
		/// </summary>
		internal Dictionary<int, Touch> ActiveTouches { get; private set; }

		/// <summary>
		/// Dictionary of origins of all active registered touches indexed by finger IDs of those touches
		/// </summary>
		internal Dictionary<int, Vector2> ActiveTouchOrigins { get; private set; }

		/// <summary>
		/// Dictionary of flags associated with active registered touches indexed by finger IDs of those touches
		/// </summary>
		internal Dictionary<int, TouchFlags> ActiveTouchFlags { get; private set; }

		#endregion

		#endregion

		#region Overrides

		private void Awake()
		{
			NewTouches = new List<Touch>();

			ActiveTouches = new Dictionary<int, Touch>();
			ActiveTouchFlags = new Dictionary<int, TouchFlags>();
			ActiveTouchOrigins = new Dictionary<int, Vector2>();

			State = new IdleState( this );
		}

		private void Update()
		{
			UpdateTouches();

			if( NewTouches.Any() )
			{
				State.OnNewInput();
			}
			else
			{
				State.OnUpdate();
			}
		}

		#endregion

		#region Functions

		#region Internal

		/// <summary>
		/// Resets all the flags of given <see cref="Touch"/>
		/// </summary>
		/// <param name="touch"><see cref="Touch"/> to reset flags for</param>
		internal void ResetTouchFlags( Touch touch )
		{
			ActiveTouchFlags[touch.fingerId].Reset();
		}

		/// <summary>
		/// Finds origin of specified <see cref="Touch"/>
		/// </summary>
		/// <param name="touch"><see cref="Touch"/> to find origin for</param>
		/// <returns>Origin of given <see cref="Touch"/></returns>
		internal Vector2 FindOrigin( Touch touch )
		{
			return ActiveTouchOrigins[touch.fingerId];
		}

		/// <summary>
		/// Registers given <see cref="Gesture"/> as active gesture
		/// </summary>
		/// <param name="gesture"><see cref="Gesture"/> to be set as currently active</param>
		internal void RegisterGesture( Gesture gesture )
		{
			ActiveGesture = gesture;
		}

		/// <summary>
		/// Finishes (and publishes) currently active gesture
		/// </summary>
		internal void FinishGesture()
		{
			ActiveGesture.Finish();
			ActiveGesture = null;
		}

		/// <summary>
		/// Removes given <see cref="Touch"/> from active touches
		/// </summary>
		/// <param name="touch"><see cref="Touch"/> to be unregistered</param>
		internal void UnregisterTouch( Touch touch )
		{
			ActiveTouches.Remove( touch.fingerId );
			ActiveTouchFlags.Remove( touch.fingerId );
		}

		/// <summary>
		/// Changes current state of gatherer
		/// </summary>
		/// <param name="state"><see cref="InputState"/> to be set as new</param>
		internal void ChangeState( InputState state )
		{
			State = state;
			State.OnUpdate();
		}

		#endregion

		#region Private

		/// <summary>
		/// Updates tracked touches and stores the rest
		/// </summary>
		private void UpdateTouches()
		{
			NewTouches.Clear();
			foreach( var touch in Input.touches )
			{
				if( ActiveTouches.ContainsKey( touch.fingerId ) )
				{
					CheckTouchFlags( touch );
					ActiveTouches[touch.fingerId] = touch;
				}
				else
				{
					ActiveTouches[touch.fingerId] = touch;
					ActiveTouchFlags[touch.fingerId] = new TouchFlags();
					ActiveTouchOrigins[touch.fingerId] = touch.position;

					NewTouches.Add( touch );
				}
			}
		}

		/// <summary>
		/// Checks given <see cref="Touch"/> to update its flags
		/// </summary>
		/// <param name="touch"><see cref="Touch"/> to be checked</param>
		private void CheckTouchFlags( Touch touch )
		{
			var flags = ActiveTouchFlags[touch.fingerId];

			if( touch.deltaPosition.magnitude >= StationaryDistanceThreshold )
			{
				flags.HasMoved = true;
			}

			if( touch.phase == TouchPhase.Ended )
			{
				flags.HasEnded = true;
			}
		}

		#endregion

		#endregion
	}
}
