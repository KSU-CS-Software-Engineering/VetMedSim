using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Object
{
    public class ToggleEnabled : MonoBehaviour
    {
        #region Functions
        /// <summary>
        /// Activates attached object
        /// </summary>
        public void EnableObject()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Deactivates attached object
        /// </summary>
        public void DisableObject()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}
