using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interaction;

public class OnCabinetClick : MonoBehaviour
{ 
    public GameObject CanvasObject;
    public Canvas _Canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateUI()
    {
        CanvasObject.GetComponent<Canvas>().enabled = true;
        CanvasObject.SetActive(true);
    }

    public void DeactivateUI()
    {
        CanvasObject.GetComponent<Canvas>().enabled = false;
        CanvasObject.SetActive(false);
    }
}
