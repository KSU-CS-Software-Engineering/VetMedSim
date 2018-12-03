using Assets.Scripts.Interaction.Options;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interaction
{
	/// <summary>
	/// Represents the button in option dialog holding one of the options
	/// </summary>
	public class OptionButton : MonoBehaviour
	{
		#region Fields

		/// <summary>
		/// Currently attached interaction option
		/// </summary>
		private InteractionOption _option;

		#endregion

		#region Properties

		/// <summary>
		/// Button controlling component of this button
		/// </summary>
		public Button ButtonComponent;

		/// <summary>
		/// Text component attached to this button
		/// </summary>
		public Text TextComponent;

		#endregion

		#region Overrides

		void Start()
		{
			if( _option != null )
			{
				ButtonComponent.onClick.AddListener( HandleClick );
			}
		}

		#endregion

		#region Functions

		/// <summary>
		/// Handles the situation, when this button gets clicked
		/// </summary>
		public void HandleClick()
		{
			_option.OnSelected();
		}

		/// <summary>
		/// Populates this button by given interaction option
		/// </summary>
		/// <param name="option"><see cref="InteractionOption"/> which data should populate this button</param>
		public void PopulateBy( InteractionOption option )
		{
			_option = option;
			TextComponent.text = _option.Name;

			Debug.Log( "Populated!" );
		}

		#endregion
	}
}
