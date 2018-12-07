using Assets.Scripts.Interaction.Options;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Interaction
{
	/// <summary>
	/// Represents interaction options dialog
	/// </summary>
	public class OptionDialog : MonoBehaviour
	{
		#region Fields

		/// <summary>
		/// List of currently held <see cref="InteractionOption"/>s
		/// </summary>
		private readonly List<InteractionOption> _options = new List<InteractionOption>();

		/// <summary>
		/// Bounding point containing minimal coordinates
		/// </summary>
		private Vector2 _minBounds;

		/// <summary>
		/// Bounding point containing maximal coordinates
		/// </summary>
		private Vector2 _maxBounds;

		#endregion

		#region Properties

		/// <summary>
		/// Header text of this option dialog
		/// </summary>
		public Text HeaderText;

		/// <summary>
		/// Panel holding interaction options
		/// </summary>
		public Transform ContentPanel;

		/// <summary>
		/// Topmost UI object holding the interaction dialog
		/// </summary>
		public Transform InteractionDialog;

		/// <summary>
		/// Object pool holding instances of <see cref="OptionButton"/>
		/// </summary>
		public ObjectPool OptionButtonPool;

		#endregion

		#region Overrides

		private void Awake()
		{
			InitializeBounds();
		}

		#endregion

		#region Functions

		#region Public

		/// <summary>
		/// Shows this option dialog on the screen
		/// </summary>
		public void Show()
		{
			InteractionDialog.gameObject.SetActive( true );
		}

		/// <summary>
		/// Dismisses this option dialog from screen
		/// </summary>
		public void Dismiss()
		{
			InteractionDialog.gameObject.SetActive( false );
		}

		/// <summary>
		/// Checks, whether this option dialog is currently shown
		/// </summary>
		/// <returns>True in case dialog is visible to user, false otherwise</returns>
		public bool IsShown()
		{
			return InteractionDialog.gameObject.activeSelf;
		}

		/// <summary>
		/// Checks, whether given <see cref="Vector2"/> position is within bounds of this option dialog
		/// </summary>
		/// <param name="position"><see cref="Vector2"/> to be checked</param>
		/// <returns>True in case given position is within bounds of this dialog, false otherwise</returns>
		public bool IsWithin( Vector2 position )
		{
			return _minBounds.x <= position.x && position.x <= _maxBounds.x
				&& _minBounds.y <= position.y && position.y <= _maxBounds.y;
		}

		/// <summary>
		/// Sets header text of this option dialog to given text
		/// </summary>
		/// <param name="text">Text to be displayed in header of this option dialog</param>
		public void SetHeaderText( string text )
		{
			HeaderText.text = text;
		}
		
		/// <summary>
		/// Adds given <see cref="InteractionOption"/> to the bottom of list of this dialog
		/// </summary>
		/// <param name="option"><see cref="InteractionOption"/> to be added</param>
		public void AddOption( InteractionOption option )
		{
			_options.Add( option );
			AttachOptionToContent( option );
		}

		/// <summary>
		/// Removes all options from this option dialog
		/// </summary>
		public void ClearOptions()
		{
			_options.Clear();
			ClearContentOptions();
		}

		#endregion

		#region Private

		/// <summary>
		/// Finds the bounding points of the dialog UI component
		/// </summary>
		private void InitializeBounds()
		{
			Vector3[] corners = new Vector3[4];
			InteractionDialog.GetComponent<RectTransform>().GetWorldCorners( corners );

			_minBounds = Camera.main.WorldToScreenPoint( corners[0] );
			_maxBounds = Camera.main.WorldToScreenPoint( corners[2] );
		}

		/// <summary>
		/// Attaches given <see cref="InteractionOption"/> to content panel
		/// </summary>
		/// <param name="option"></param>
		private void AttachOptionToContent( InteractionOption option )
		{
			var newOptionObject = OptionButtonPool.RetrieveObject();
			newOptionObject.transform.SetParent( ContentPanel, false );

			newOptionObject.GetComponent<OptionButton>().PopulateBy( option );
			option.AttachToDialog( this );
		}

		/// <summary>
		/// Clears all the child game objects of content panel
		/// </summary>
		private void ClearContentOptions()
		{
			while( ContentPanel.childCount > 0 )
			{
				OptionButtonPool.ReturnObject( ContentPanel.GetChild( 0 ).gameObject );
			}
		}

		#endregion

		#endregion
	}
}
