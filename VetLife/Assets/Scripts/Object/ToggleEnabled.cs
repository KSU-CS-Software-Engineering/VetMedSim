using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Object
{
    public class ToggleEnabled : MonoBehaviour
    {
        #region Functions
        public void EnableObject()
        {
            gameObject.SetActive(true);
        }

        public void DisableObject()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}
