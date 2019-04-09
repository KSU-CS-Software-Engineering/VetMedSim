using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLocation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < Input.touchCount; i++)
        {


            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {


                transform.position = new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, transform.position.z);

            }
        }
    }
}
