using UnityEngine;

namespace Assets.Scripts
{
	/// <summary>
	/// Controls the camera movement
	/// </summary>
	public class CameraController : MonoBehaviour
	{
		#region Fields

		/// <summary>
		/// Offset on the Z axis
		/// </summary>
		private readonly float mZOffset = -10f;

		#endregion

		#region Overrides

		private void LateUpdate()
		{
			var hoodyPosition = Hoody.transform.position;
			transform.position = new Vector3( hoodyPosition.x, hoodyPosition.y, mZOffset );
		}

		#endregion

		#region Properties 

		/// <summary>
		/// The main game character
		/// </summary>
		public GameObject Hoody;

		#endregion
	}
}
