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
        #endregion

        #region Properties
        public GameObject Dialog;
        public GestureHandler GestureHandler;
        public GameObject ContentPanel;
        public List<InteractionOption> Options = new List<InteractionOption>();
        #endregion

        #region Overrides
        void Awake()
        {
            GestureHandler.RegisterListener( this );
            _optionDialog = ContentPanel.GetComponent<OptionDialog>();

            InteractionOption[] optionsArray = GetComponentsInChildren<InteractionOption>();
            foreach( InteractionOption option in optionsArray )
            {
                Options.Add( option );
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
                    if( hit.collider != null && hit.collider.gameObject == gameObject )
                    {
                        _optionDialog.Show();
                        _optionDialog.ClearOptions();
                        foreach( InteractionOption option in Options )
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
