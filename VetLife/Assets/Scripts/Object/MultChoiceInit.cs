using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Object
{
    /// <summary>
    /// Initialization and reinitialization of a multiple-choice type question
    /// </summary>
    public class MultChoiceInit : MonoBehaviour
    {
        #region Properties
        public GameObject answer;
        public GameObject options;
        public GameObject results;
        #endregion

        #region Overrides

        void OnEnable()
        {
            ToggleEnabled answerToggle = answer.GetComponent<ToggleEnabled>();
            ToggleChildrenEnabled resultsToggle = results.GetComponent<ToggleChildrenEnabled>();

            if( answerToggle != null )
            {
                answerToggle.DisableObject();
            }

            if( resultsToggle != null )
            {
                resultsToggle.DisableChildren();
            }
            
            foreach( Transform optionTransform in options.transform )
            {
                Button option = optionTransform.GetComponent<Button>();
                if( option != null )
                {
                    option.interactable = true;
                }
            }
        }

        #endregion
    }
}
