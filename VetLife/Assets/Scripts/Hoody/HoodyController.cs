using System.Collections.Generic;
using Assets.Scripts.Commands;
using Assets.Scripts.Interaction.Prototype;
using UnityEngine;

namespace Assets.Scripts.Hoody
{
	/// <summary>
	/// Script defining behavior of main character Hoody
	/// </summary>
	public class HoodyController : MonoBehaviour, IInteractor
	{
		#region Fields

		/// <summary>
		/// Animator associated with Hoody
		/// </summary>
		private Animator mAnimator;

		/// <summary>
		/// List of interactive objects in proximity
		/// </summary>
		private List<IInteractive> mInteractives;

		/// <summary>
		/// Currently executed command
		/// </summary>
		private CommandType mCommand;

		/// <summary>
		/// Currently active main touch
		/// </summary>
		private Touch? mTouch;

		/// <summary>
		/// Current destination
		/// </summary>
		private Vector3? mDestination;

		/// <summary>
		/// Currently active interactive
		/// </summary>
		private IInteractive mInteractive;

		#endregion

		#region Overrides

		private void Awake()
		{
			mAnimator = GetComponent<Animator>();

			mCommand = CommandType.NONE;
			mInteractives = new List<IInteractive>();
		}

		private void Update()
		{
			GatherInput();
			Act();
		}

		private void OnCollisionStay2D( Collision2D collision )
		{
			StopMoving();
		}

		#endregion

		#region Functions

		#region Input

		/// <summary>
		/// Gathers user input
		/// </summary>
		private void GatherInput()
		{
			mTouch = GetActiveTouch();
			if( mCommand == CommandType.NONE )
			{
				mCommand = CommandRecognizer.Recognize( mInteractives, mTouch );
			}
		}

		/// <summary>
		/// Acts based on current command
		/// </summary>
		private void Act()
		{
			switch( mCommand )
			{
				case CommandType.INTERACTION:
					HandleInteraction();
					break;

				case CommandType.MOVEMENT:
					HandleMovement();
					break;

				default:
					/// intentionally left out blank - do nothing
					break;
			}
		}

		/// <summary>
		/// Retrieves the <see cref="Touch"/> object of current main touch event
		/// </summary>
		/// <returns>The <see cref="Touch"/> object of current main touch event or null, if there is neither
		/// active nor any new touch</returns>
		private Touch? GetActiveTouch()
		{
			if( !mTouch.HasValue )
			{
				return GetFirstAvailableTouch();
			}

			foreach( var touch in Input.touches )
			{
				if( touch.fingerId == mTouch.Value.fingerId )
				{
					return touch;
				}
			}

			return GetFirstAvailableTouch();
		}

		/// <summary>
		/// Retrieves first available <see cref="Touch"/>
		/// </summary>
		/// <returns>First available <see cref="Touch"/> or null, if there are no touches</returns>
		private Touch? GetFirstAvailableTouch()
		{
			return (Input.touches.Length > 0) ? Input.GetTouch( 0 ) as Touch? : null;
		}

		/// <summary>
		/// Releases currently hold <see cref="Touch"/>
		/// </summary>
		private void ReleaseTouch()
		{
			mTouch = null;
		}

		#endregion

		#region Interaction

		/// <summary>
		/// Handles Hoody's interaction with interactive objects
		/// </summary>
		private void HandleInteraction()
		{
			if( mTouch.HasValue )
			{
				switch( mTouch.Value.phase )
				{
					case TouchPhase.Began:
						StartInteraction();
						break;

					case TouchPhase.Canceled:
					case TouchPhase.Ended:
						AbortInteraction();
						break;

					default:
						// intentionally left blank
						break;
				}
			}
		}

		/// <summary>
		/// Starts Hoody's interaction with object
		/// </summary>
		private void StartInteraction()
		{
			var touchPoint = Camera.main.ScreenToWorldPoint( mTouch.Value.position );
			mInteractive = mInteractives.Find( interactive => interactive.IsTargeted( touchPoint ) );

			mInteractive.StartInteraction();

			mAnimator.SetTrigger( "Interacts" );
			mAnimator.ResetTrigger( "Moves" );
		}

		/// <summary>
		/// Aborts Hoody's interaction with object
		/// </summary>
		private void AbortInteraction()
		{
			mInteractive.AbortInteraction();

			mAnimator.ResetTrigger( "Interacts" );

			mCommand = CommandType.NONE;
		}

		#endregion

		#region Movement

		/// <summary>
		/// Handles movement of Hoody
		/// </summary>
		private void HandleMovement()
		{
			if( mTouch.HasValue && mTouch.Value.phase == TouchPhase.Ended )
			{
				mDestination = Camera.main.ScreenToWorldPoint( mTouch.Value.position );
				StartMoving();
			}

			if( mDestination == null )
			{
				return;
			}

			var relativePosition = mDestination.Value - gameObject.transform.position;

			Face( relativePosition );
			MoveTowards( relativePosition );
		}

		/// <summary>
		/// Turns Hoody so he will end up facing destination position
		/// </summary>
		/// <param name="relativePosition">Relative position of destination to Hoody's location</param>
		private void Face( Vector2 relativePosition )
		{
			var facingLeft = gameObject.transform.localScale.x > 0;
			if( (facingLeft && relativePosition.x > 0) || (!facingLeft && relativePosition.x < 0) )
			{
				gameObject.transform.localScale = new Vector3( -1 * gameObject.transform.localScale.x, 1f, 1f );
			}
		}

		/// <summary>
		/// Moves Hoody towards his destination
		/// </summary>
		/// <param name="relativePosition">Relative position of destination to Hoody's location</param>
		private void MoveTowards( Vector2 relativePosition )
		{
			var finalVelocity = relativePosition;
			if( relativePosition.magnitude > Speed * Time.deltaTime )
			{
				finalVelocity = relativePosition.normalized * Speed * Time.deltaTime;
			}

			if( finalVelocity.magnitude > 0 )
			{
				gameObject.transform.Translate( finalVelocity );
			}
			else
			{
				mCommand = CommandType.NONE;

				mDestination = null;
				mAnimator.ResetTrigger( "Moves" );
			}
		}

		/// <summary>
		/// Starts Hoody's movement
		/// </summary>
		private void StartMoving()
		{
			mAnimator.SetTrigger( "Moves" );
		}

		/// <summary>
		/// Terminates Hoody's movement
		/// </summary>
		private void StopMoving()
		{
			mCommand = CommandType.NONE;

			mDestination = null;
			mAnimator.ResetTrigger( "Moves" );
		}

		#endregion

		#endregion

		#region IInteractor

		public void AddInteractive( IInteractive interactive )
		{
			mInteractives.Add( interactive );
		}

		public void RemoveInteractive( IInteractive interactive )
		{
			mInteractives.Remove( interactive );
		}

		#endregion

		#region Properties

		/// <summary>
		/// Hoody's movement speed
		/// </summary>
		public float Speed;

		#endregion
	}
}
