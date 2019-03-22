using UnityEngine;
using Assets.Scripts.UserInput;

public class ShowCanvas : MonoBehaviour
{
    public GameObject CanvasObject;
    public GestureType _gestureType = new GestureType();
    // Start is called before the first frame update
    void Start()
    {
       CanvasObject.GetComponent<Canvas>().enabled = false;
    }

    private void OnMouseDown()
    {
        CanvasObject.GetComponent<Canvas>().enabled = true;
    }

    public void ActivateUI()
    {
        Debug.Log("Activate UI");
        CanvasObject.GetComponent<Canvas>().enabled = true;
        CanvasObject.SetActive(true);
    }

    public void DeactivateUI()
    {
        CanvasObject.GetComponent<Canvas>().enabled = false;
        CanvasObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Clicked");
            CanvasObject.GetComponent<Canvas>().enabled = true;
        }
        
    }
}
