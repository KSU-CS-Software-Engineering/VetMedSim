using Assets.Scripts.Interaction.Options;
using Assets.Scripts.Object;
using Assets.Scripts.UserInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interaction
{
    public class ObjectInteraction : MonoBehaviour, IGestureListener
    {
        #region Fields
        private OptionDialog _optionDialog;

        private List<InteractionOption> _options = new List<InteractionOption>();
        #endregion

        #region Properties
        public GameObject Dialog;
        public GestureHandler GestureHandler;
        public GameObject ContentPanel;
        #endregion

        #region Overrides
        void Awake()
        {
            Dialog.SetActive( false );
            GestureHandler.RegisterListener( this );
            _optionDialog = ContentPanel.GetComponent<OptionDialog>();

            InteractionOption[] optionsArray = GetComponentsInChildren<InteractionOption>();
            foreach( InteractionOption option in optionsArray )
            {
                _options.Add( option );
            }
        }

        #endregion

        #region IGestureListener

        public void OnGestureStart( Gesture gesture )
        {
            switch( gesture.Type )
            {
                case GestureType.Tap:
                    Vector2 origin = ( ( Tap ) gesture ).Origin;
                    Vector3 gestureLocation = Camera.main.ScreenToWorldPoint( origin );
                    RaycastHit2D hit = Physics2D.Raycast( gestureLocation, Vector2.zero );
                    if( hit.collider.gameObject == gameObject )
                    {
                        Dialog.SetActive( true );
                        _optionDialog.ClearOptions();
                        foreach( InteractionOption option in _options )
                        {
                            _optionDialog.AddOption( option );
                        }
                    }
                    break;
            }
        }

        public void OnGestureEnd( Gesture gesture )
        {
            // intentionally left blank
        }

        #endregion
    }
}
