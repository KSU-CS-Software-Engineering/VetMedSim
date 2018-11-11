using UnityEngine;

namespace Assets.Scripts.UserInput
{
	#region Core

	/// <summary>
	/// Contains all possible types of user gestures
	/// </summary>
	public enum GestureType
	{
		Tap
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

	#endregion
}
