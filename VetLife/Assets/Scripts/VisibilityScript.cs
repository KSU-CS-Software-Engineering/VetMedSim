using UnityEngine;

namespace Assets.Scripts
{
	/// <summary>
	/// Script handling visibility ordering of objects with regards to Hoody
	/// </summary>
	public class VisibilityScript : MonoBehaviour
	{
		#region Fields

		/// <summary>
		/// Associated renderer
		/// </summary>
		private Renderer mRenderer;

		#endregion

		#region Overrides

		private void Awake()
		{
			mRenderer = GetComponent<Renderer>();
		}

		private void Update()
		{
			if( Hoody.gameObject.transform.position.y > gameObject.transform.position.y )
			{
				mRenderer.sortingLayerName = "Foreground";
			}
			else
			{
				mRenderer.sortingLayerName = "Background";
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// The main character Hoody
		/// </summary>
		public GameObject Hoody;

		#endregion
	}
}
