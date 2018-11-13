using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UserInput
{
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

		#endregion

		#endregion

		#region Overrides

		private void Awake()
		{
			NewTouches = new List<Touch>();

			ActiveTouches = new Dictionary<int, Touch>();
			ActiveTouchOrigins = new Dictionary<int, Vector2>();
		}

		#endregion
	}
}
