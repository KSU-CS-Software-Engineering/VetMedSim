using UnityEngine;

namespace Assets.Scripts.UserInput
{
	#region Core

	/// <summary>
	/// Contains all possible types of user gestures
	/// </summary>
	public enum GestureType
	{
		Tap,
		Hold,
		Drag
	}

	/// <summary>
	/// Represents basic user gesture
	/// </summary>
	public abstract class Gesture
	{
		#region Fields

		/// <summary>
		/// Time stamp of the moment this gesture started
		/// </summary>
		private readonly float _startTime;

		/// <summary>
		/// Time stamp of the moment this gesture ended
		/// </summary>
		private float _endTime;

		#endregion

		#region Properties

		/// <summary>
		/// Type of this user gesture
		/// </summary>
		public GestureType Type { get; }

		/// <summary>
		/// Indicator, whether this gestures is finished (opposed to being in progress)
		/// </summary>
		public bool Finished { get; private set; }

		/// <summary>
		/// Time elapsed during making of this gesture
		/// </summary>
		public float TimeElapsed => Finished ? _endTime - _startTime : Time.time - _startTime;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs basic user gesture
		/// </summary>
		/// <param name="type">Type of this gesture</param>
		protected Gesture( GestureType type )
		{
			_startTime = Time.time;
			Finished = false;

			Type = type;
		}

		#endregion

		#region Functions

		/// <summary>
		/// Ends the timer of this gesture and marks it as finished
		/// </summary>
		internal void Finish()
		{
			_endTime = Time.time;
			Finished = true;
		}

		#endregion
	}

	#endregion

	#region Single-finger

	/// <summary>
	/// Represents user gestures that are made using one finger
	/// </summary>
	public abstract class SingleFingerGesture : Gesture
	{
		#region Properties

		/// <summary>
		/// The <see cref="Vector2"/> containing coordinates of the place where this gesture started
		/// </summary>
		public Vector2 Origin { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs base single-finger gesture
		/// </summary>
		/// <param name="origin">The <see cref="Vector2"/> containing coordinates of the place where this gesture started</param>
		/// <param name="type">Type of this gesture</param>
		protected SingleFingerGesture( Vector2 origin, GestureType type ) : base( type )
		{
			Origin = origin;
		}

		#endregion
	}

	/// <summary>
	/// Represents user (single or multiple) tap gesture
	/// </summary>
	public class Tap : SingleFingerGesture
	{
		#region Properties

		/// <summary>
		/// Number of consecutive taps user performed
		/// </summary>
		public int TapCount { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs tap gesture
		/// </summary>
		/// <param name="origin">The <see cref="Vector2"/> containing coordinates of the place where this gesture started</param>
		/// <param name="tapCount">Number of consecutive taps user performed</param>
		public Tap( Vector2 origin, int tapCount ) : base( origin, GestureType.Tap )
		{
			TapCount = tapCount;
		}

		#endregion
	}

	/// <summary>
	/// Represents user (prolonged stationary) hold gesture
	/// </summary>
	public class Hold : SingleFingerGesture
	{
		#region Constructors

		/// <summary>
		/// Constructs hold gesture
		/// </summary>
		/// <param name="origin">The <see cref="Vector2"/> containing coordinates of the place where this gesture started</param>
		public Hold( Vector2 origin ) : base( origin, GestureType.Hold )
		{
			// intentionally left blank
		}

		#endregion
	}

	/// <summary>
	/// Represents user (prolonged moved) drag gesture
	/// </summary>
	public class Drag : SingleFingerGesture
	{
		#region Properties

		/// <summary>
		/// Coordinates of last user touch
		/// </summary>
		public Vector2 End { get; set; }

		/// <summary>
		/// Overall direction of this gesture
		/// </summary>
		public Vector2 Direction => End - Origin;

		/// <summary>
		/// Direction of this gesture from last update (frame)
		/// </summary>
		public Vector2 DeltaDirection { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs drag gesture
		/// </summary>
		/// <param name="origin">The <see cref="Vector2"/> containing coordinates of the place where this gesture started</param>
		/// <param name="end">The last known position of this gesture</param>
		public Drag( Vector2 origin, Vector2 end ) : base( origin, GestureType.Drag )
		{
			End = end;
			DeltaDirection = Origin - End;
		}

		#endregion

		#region Functions

		/// <summary>
		/// Updates the last know position by moving to given coordinates
		/// </summary>
		/// <param name="end">New value of coordinates</param>
		public void MoveTo( Vector2 end )
		{
			End = end;
			DeltaDirection = Origin - End;
		}

		/// <summary>
		/// Updates the last know position by moving by given delta coordinates
		/// </summary>
		/// <param name="delta">Difference of coordinates that should be applied to last known position</param>
		public void MoveBy( Vector2 delta )
		{
			DeltaDirection = delta;
			End += delta;
		}

		#endregion
	}

	#endregion

	#region Double-finger

	/// <summary>
	/// Represents user gestures that are made using two fingers
	/// </summary>
	public abstract class DoubleFingerGesture : Gesture
	{
		#region Properties

		/// <summary>
		/// The <see cref="Vector2"/> containing coordinates of one of the places where this gesture started
		/// </summary>
		public Vector2 OriginMain { get; }

		/// <summary>
		/// The <see cref="Vector2"/> containing coordinates of the other place where this gesture started
		/// </summary>
		public Vector2 OriginSecondary { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs base double-finger gesture
		/// </summary>
		/// <param name="originMain">The <see cref="Vector2"/> containing coordinates of one of the places where this gesture started</param>
		/// <param name="originSecondary">The <see cref="Vector2"/> containing coordinates of the other place where this gesture started</param>
		/// <param name="type">Type of this user gesture</param>
		protected DoubleFingerGesture( Vector2 originMain, Vector2 originSecondary, GestureType type ) : base( type )
		{
			OriginMain = originMain;
			OriginSecondary = originSecondary;
		}

		#endregion
	}

	#endregion
}
