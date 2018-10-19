using UnityEngine;

namespace Assets.Scripts.Interaction.Prototype
{
	/// <summary>
	/// Objects, which can be interacted with
	/// </summary>
	public interface IInteractive
	{
		/// <summary>
		/// Starts interaction attempt
		/// </summary>
		void StartInteraction();

		/// <summary>
		/// Aborts any attempt of interaction
		/// </summary>
		void AbortInteraction();

		/// <summary>
		/// Checks whether given position is within trigger bounds of this object
		/// </summary>
		/// <param name="position">Position to be checked</param>
		/// <returns>True in case position selects bounds of this object, false otherwise</returns>
		bool IsTargeted( Vector2 position );
	}
}
