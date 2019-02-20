using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCanvas : MonoBehaviour
{
    public GameObject CanvasObject;

    // Start is called before the first frame update
    void Start()
    {
        CanvasObject.GetComponent<Canvas>().enabled = false;
    }

    public void ActivateUI()
    {
        CanvasObject.GetComponent<Canvas>().enabled = true;
        CanvasObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
