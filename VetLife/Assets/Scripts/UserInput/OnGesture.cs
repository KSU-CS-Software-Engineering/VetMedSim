using Assets.Scripts.UserInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInput
{
    /// <summary>
    /// Class that listens for gestures and invokes the attached GameObject's onclick() event when appropriate
    /// </summary>
    public class OnGesture : MonoBehaviour, IGestureListener
    {
        #region Properties
        /// <summary>
        /// Object handling gesture recognition
        /// </summary>
        public GestureHandler GestureHandler;

        #endregion

        #region Overrides
        private void Awake()
        {
            GestureHandler.RegisterListener( this );
        }

        #endregion

        #region IGestureListener
        public void OnGestureStart( Gesture gesture )
        {
            switch ( gesture.Type )
            {
                case GestureType.Tap:
                    Vector2 origin = ( (Tap)gesture ).Origin;
                    Vector3 gestureLocation = Camera.main.ScreenToWorldPoint( origin );
                    RaycastHit2D hit = Physics2D.Raycast( gestureLocation, Vector2.zero );
                    if ( hit.transform.gameObject == gameObject )
                    {
                        gameObject.GetComponent<Button>().onClick.Invoke();
                    }
                    break;
            }
        }

        public void OnGestureEnd( Gesture gesture )
        {
            //intentionally left blank
        }

        #endregion
    }

}
