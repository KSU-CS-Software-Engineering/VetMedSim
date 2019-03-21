using Assets.Scripts.UserInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CabinetInteraction : MonoBehaviour, IGestureListener
    {

        public Canvas canvas;

        // Use this for initialization
        void Start()
        {
            canvas.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

            // Move this object to the position clicked by the mouse.
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
                if (hit.collider != null)
                {
                    Debug.Log("I'm hitting " + hit.collider.name);
                    if (hit.collider.name.Equals("cabinet2"))
                    {
                        canvas.enabled = !canvas.enabled;
                    }
                }
            }
        }

        public void OnGestureStart(Gesture gesture)
        {
            switch (gesture.Type)
            {
                case GestureType.Tap:
                    var origin = ((Tap)gesture).Origin;
                    var gestureLocation = Camera.main.ScreenToWorldPoint(origin);
                    RaycastHit2D hit = Physics2D.Raycast(gestureLocation, Vector2.zero);
                    if (hit.collider != null)
                    {
                        Debug.Log("I'm hitting " + hit.collider.name);
                        if (hit.collider.name.Equals("cabinet2"))
                        {
                            canvas.enabled = !canvas.enabled;
                        }
                    }
                    break;
            }
        }

        public void OnGestureEnd(Gesture gesture)
        {
            // intentionally left blank
        }
    }
}