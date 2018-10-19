using UnityEngine;

namespace Assets.Scripts.Interaction.Prototype
{
	/// <summary>
	/// Represents loading circle on interactive object
	/// </summary>
	public class InteractionCircle
	{
		#region Constants 

		/// <summary>
		/// Width of loading circle line
		/// </summary>
		private static readonly float LINE_WIDTH = 0.075f;

		/// <summary>
		/// Radius of loading circle
		/// </summary>
		private static readonly float RADIUS = 0.15f;

		#endregion

		#region Fields

		/// <summary>
		/// Renderer of this object
		/// </summary>
		private readonly LineRenderer mRenderer;

		/// <summary>
		/// Cycles required to load interaction
		/// </summary>
		private readonly int mLoadingTime;

		/// <summary>
		/// Indicator determining, whether the object is interacted with
		/// </summary>
		private bool mActive;

		/// <summary>
		/// Cycles of loading elapsed
		/// </summary>
		private int mElapsedTime;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs new interaction circle
		/// </summary>
		/// <param name="renderer">Renderer of parent component used to render the circle</param>
		/// <param name="loadingTime">Time required for interaction to load</param>
		public InteractionCircle( LineRenderer renderer, int loadingTime )
		{
			mRenderer = renderer;
			mLoadingTime = loadingTime;

			mActive = false;
			mElapsedTime = 0;

			Finished = false;
		}

		#endregion

		#region Functions

		#region Inner

		/// <summary>
		/// Draws part of loading circle based on number of cycles elapsed
		/// </summary>
		private void DrawCirclePart()
		{
			mRenderer.widthMultiplier = LINE_WIDTH;
			mRenderer.positionCount = mElapsedTime;

			var theta = Mathf.PI / 2;
			var thetaDelta = (2f * Mathf.PI) / mLoadingTime;

			for( var i = 0; i < mElapsedTime; i++, theta -= thetaDelta )
			{
				mRenderer.SetPosition( i, new Vector3( RADIUS * Mathf.Cos( theta ), RADIUS * Mathf.Sin( theta ), 0f ) );
			}
		}

		/// <summary>
		/// Handles the life cycle progress of this indicator
		/// </summary>
		private void HandleLifecycleProgress()
		{
			if( mElapsedTime > mLoadingTime )
			{
				Finished = true;
				mActive = false;
			}
			else
			{
				mElapsedTime++;
			}
		}

		#endregion

		#region Visible

		/// <summary>
		/// Starts the interaction loading process
		/// </summary>
		public void StartInteraction()
		{
			mElapsedTime = 0;
			mActive = true;

			Finished = false;
		}

		/// <summary>
		/// Acknowledges the end of loading and turns it off
		/// </summary>
		public void FinishInteraction()
		{
			mElapsedTime = 0;
			mActive = false;

			Finished = false;

			mRenderer.positionCount = 0;
		}

		/// <summary>
		/// Updates the state of interaction circle
		/// </summary>
		public void Update()
		{
			if( !mActive )
			{
				return;
			}

			DrawCirclePart();
			HandleLifecycleProgress();
		}

		#endregion

		#endregion

		#region Properties

		/// <summary>
		/// Indicator, whether the interaction loading has finished
		/// </summary>
		public bool Finished { get; private set; }

		#endregion
	}
}
