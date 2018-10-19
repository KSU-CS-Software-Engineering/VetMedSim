using System.Collections;
using Assets.Scripts.Hoody;
using Assets.Scripts.Interaction.Prototype;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interaction.Implementation
{
	/// <summary>
	/// Handles interactive behavior of table object
	/// </summary>
	public class TableInteractionScript : InteractiveObject
	{
		#region Overrides

		protected override void Interact()
		{
			StartCoroutine( ShowMessage( "Interacting with table!", 2f ) );
		}

		protected override IInteractor GetInteractor()
		{
			return Hoody;
		}

		#endregion

		#region Functions

		/// <summary>
		/// Shows given message after specified amount of time
		/// </summary>
		/// <param name="message">Message to be shown</param>
		/// <param name="delay">Time for which the message should be shown</param>
		/// <returns>The return enumerator should be ignored</returns>
		private IEnumerator ShowMessage( string message, float delay )
		{
			InfoText.text = message;
			InfoText.enabled = true;

			yield return new WaitForSeconds( delay );

			InfoText.enabled = false;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Game's main character Hoody
		/// </summary>
		public HoodyController Hoody;

		/// <summary>
		/// Informative text for showing events
		/// </summary>
		public Text InfoText;

		#endregion
	}
}
