using Assets.Scripts.UserInput;
using UnityEngine;

namespace Assets.Scripts.Player
{
	#region Player State Machine

	/// <summary>
	/// Represents base abstract player state
	/// </summary>
	internal abstract class PlayerState
	{
		#region Properties

		/// <summary>
		/// Reference to the <see cref="PlayerController"/> owner object
		/// </summary>
		internal PlayerController Player { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs base player state
		/// </summary>
		/// <param name="player">Reference to the <see cref="PlayerController"/> owner object</param>
		protected PlayerState( PlayerController player )
		{
			Player = player;
		}

		#endregion

		#region Functions

		/// <summary>
		/// Handles the updating of currently active state
		/// </summary>
		internal abstract void OnUpdate();

		/// <summary>
		/// Defines behavior when player sprite collides with something
		/// </summary>
		/// <param name="collision">Object containing collision details</param>
		internal abstract void OnCollision( Collision2D collision );

		#endregion
	}

	/// <summary>
	/// Represents player state, when player is not doing anything
	/// </summary>
	internal class IdleState : PlayerState
	{
		#region Constructors

		/// <summary>
		/// Constructs base idle state
		/// </summary>
		/// <param name="player">Reference to the <see cref="PlayerController"/> owner object</param>
		internal IdleState( PlayerController player ) : base( player )
		{
			Player.Animator.ResetTrigger( Player.MOVE_ANIMATION_TRIGGER );
		}

		#endregion

		#region Overrides

		internal override void OnCollision( Collision2D collision )
		{
			// intentionally left blank
		}

		internal override void OnUpdate()
		{
			// intentionally left blank
		}

		#endregion
	}

	#endregion

	/// <summary>
	/// Defines the behavior of the player sprite
	/// </summary>
	[RequireComponent( typeof( Animator ) )]
	public class PlayerController : MonoBehaviour, IGestureListener
	{
		#region Constants

		/// <summary>
		/// Name of the animation trigger for movement
		/// </summary>
		internal string MOVE_ANIMATION_TRIGGER = "Moves";

		#endregion

		#region Properties

		/// <summary>
		/// Object handling gesture recognition
		/// </summary>
		public GestureHandler GestureHandler;

		/// <summary>
		/// Speed by which the player character moves
		/// </summary>
		public float Speed;

		/// <summary>
		/// Current state of the player
		/// </summary>
		internal PlayerState State { get; private set; }

		/// <summary>
		/// Animator component of player object
		/// </summary>
		internal Animator Animator { get; private set; }

		/// <summary>
		/// Current position of the player
		/// </summary>
		internal Vector2 Position => gameObject.transform.position;

		#endregion

		#region Overrides

		private void Awake()
		{
			GestureHandler.RegisterListener( this );

			Animator = GetComponent<Animator>();

			State = new IdleState( this );
		}

		private void Update()
		{
			State.OnUpdate();
		}

		private void OnCollisionStay2D( Collision2D collision )
		{
			State.OnCollision( collision );
		}

		#endregion

		#region Functions

		/// <summary>
		/// Changes player state to given one
		/// </summary>
		/// <param name="state">New state of player</param>
		internal void ChangeState( PlayerState state )
		{
			State = state;
			State.OnUpdate();
		}

		#endregion

		#region IGestureListener

		public void OnGestureStart( Gesture gesture )
		{
			switch( gesture.Type )
			{
				case GestureType.Tap:
					var origin = ((Tap) gesture).Origin;
					var destination = Camera.main.ScreenToWorldPoint( origin );

					// TODO: Start walking towards destination
					break;
			}
		}

		public void OnGestureEnd( Gesture gesture )
		{
			// intentionally left blank
		}

		#endregion
	}
}
