using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interaction.Options
{
	/// <summary>
	/// Holds common functionality and interface description for interaction options
	/// </summary>
	public class InteractionOption : MonoBehaviour
	{
		#region Fields

		/// <summary>
		/// Reference to the dialog this interaction option is currently bound to
		/// </summary>
		private OptionDialog _dialog;

		#endregion

		#region Properties

		/// <summary>
		/// Name of the option
		/// </summary>
		public string Name;

		/// <summary>
		/// Function invoked when this option is selected
		/// </summary>
		public UnityEvent Callback;

		#endregion

		#region Functions

		/// <summary>
		/// Creates backlink to the given <see cref="OptionDialog"/>
		/// </summary>
		/// <param name="dialog">Dialog to attach this option to</param>
		public void AttachToDialog( OptionDialog dialog )
		{
			_dialog = dialog;
		}

		/// <summary>
		/// Removes backlink to any linked <see cref="OptionDialog"/>
		/// </summary>
		public void DetachFromDialog()
		{
			_dialog = null;
		}

		/// <summary>
		/// Callback function which gets called on this option being selected
		/// </summary>
		public void OnSelected()
		{
			Callback.Invoke();
		}

		/// <summary>
		/// Dismisses the parent dialog and clears its options
		/// </summary>
		protected void DismissDialog()
		{
			_dialog.Dismiss();
			_dialog.ClearOptions();
		}

		#endregion
	}
}
