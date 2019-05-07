using System;
using UnityEngine;

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

		/// <summary>
		/// Function invoked when this option is selected
		/// </summary>
		private readonly Action _callback;

		#endregion

		#region Properties

		/// <summary>
		/// Name of the option
		/// </summary>
		public string Name { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs base interaction option
		/// </summary>
		/// <param name="name">Name of the option</param>
		/// <param name="callbackFunction">Function invoked when this option is selected</param>
		protected InteractionOption( string name, Action callbackFunction )
		{
			Name = name;
			_callback = callbackFunction;
		}

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
			_callback.Invoke();
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
