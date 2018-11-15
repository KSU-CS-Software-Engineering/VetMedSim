
namespace Assets.Scripts.UserInput
{
	/// <summary>
	/// Allows object to receive <see cref="Gesture"/> notification from <see cref="GestureHandler"/>
	/// </summary>
	public interface IGestureListener
	{
		/// <summary>
		/// Defines behavior on given <see cref="Gesture"/> being registered
		/// </summary>
		/// <param name="gesture"><see cref="Gesture"/> to react to</param>
		void OnGestureStart( Gesture gesture );

		/// <summary>
		/// Defines behavior on given <see cref="Gesture"/> being finished
		/// </summary>
		/// <param name="gesture"><see cref="Gesture"/> to react to its end</param>
		void OnGestureEnd( Gesture gesture );
	}
}
