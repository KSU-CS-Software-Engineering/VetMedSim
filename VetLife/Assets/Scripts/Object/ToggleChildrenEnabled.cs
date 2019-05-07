using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Object
{
    /// <summary>
    /// Activates or deactivates all objects in children
    /// </summary>
    public class ToggleChildrenEnabled : MonoBehaviour
    {
        #region Functions
        /// <summary>
        /// Activates attached object's children (recursively if applicable)
        /// </summary>
        public void EnableChildren()
        {
            foreach( Transform child in transform )
            {
                GameObject childObject = child.gameObject;
                childObject.SetActive( true );
                ToggleChildrenEnabled childToggle = childObject.GetComponent<ToggleChildrenEnabled>();
                if( childToggle != null )
                {
                    childToggle.EnableChildren();
                }
            }
        }

        /// <summary>
        /// Deactivates attached object's children (recursively if applicable)
        /// </summary>
        public void DisableChildren()
        {
            foreach( Transform child in transform )
            {
                GameObject childObject = child.gameObject;
                childObject.SetActive( false );
                ToggleChildrenEnabled childToggle = childObject.GetComponent<ToggleChildrenEnabled>();
                if( childToggle != null )
                {
                    childToggle.DisableChildren();
                }
            }
        }

        #endregion
    }
}
