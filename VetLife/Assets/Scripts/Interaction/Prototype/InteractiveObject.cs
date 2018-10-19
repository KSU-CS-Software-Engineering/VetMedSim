using UnityEngine;

namespace Assets.Scripts.Interaction.Prototype
{
	/// <summary>
	/// Holds common behavior for interactive objects
	/// </summary>
	[RequireComponent( typeof( LineRenderer ) )]
	[RequireComponent( typeof( CapsuleCollider2D ) )]
	public abstract class InteractiveObject : MonoBehaviour, IInteractive
	{
		#region Fields

		/// <summary>
		/// Attached interaction circle
		/// </summary>
		private InteractionCircle mCircle;

		/// <summary>
		/// Attached trigger collider
		/// </summary>
		private CapsuleCollider2D mTrigger;

		#endregion

		#region Overrides

		private void Awake()
		{
			mCircle = new InteractionCircle( GetComponent<LineRenderer>(), LoadingTime );
			mTrigger = GetComponent<CapsuleCollider2D>();
		}

		private void Update()
		{
			if( mCircle.Finished )
			{
				Interact();
				mCircle.FinishInteraction();
			}
			else
			{
				mCircle.Update();
			}
		}

		private void OnTriggerEnter2D( Collider2D collision )
		{
			if( collision.gameObject.tag == "Player" )
			{
				GetInteractor().AddInteractive( this );
			}
		}

		private void OnTriggerExit2D( Collider2D collision )
		{
			if( collision.gameObject.tag == "Player" )
			{
				GetInteractor().RemoveInteractive( this );
			}
		}

		#endregion

		#region Functions

		/// <summary>
		/// This function should start the actual interaction
		/// </summary>
		protected abstract void Interact();

		/// <summary>
		/// Retrieves associated <see cref="IInteractor"/>
		/// </summary>
		/// <returns>Associated <see cref="IInteractor"/></returns>
		protected abstract IInteractor GetInteractor();

		#endregion

		#region Properties

		/// <summary>
		/// Cycles required to load interaction
		/// </summary>
		public int LoadingTime;

		#endregion

		#region IInteractive

		public void StartInteraction()
		{
			mCircle.StartInteraction();
		}

		public void AbortInteraction()
		{
			mCircle.FinishInteraction();
		}

		public bool IsTargeted( Vector2 position )
		{
			return mTrigger.bounds.Contains( position );
		}

		#endregion
	}
}
