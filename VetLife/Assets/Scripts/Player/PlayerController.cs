using Assets.Scripts.UserInput;
using UnityEngine;

namespace Assets.Scripts.Player
{
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
