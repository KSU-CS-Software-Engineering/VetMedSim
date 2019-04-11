using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.UserInput;

namespace Assets.Scripts.Player
{
    public class ChangeLocation : MonoBehaviour//, IGestureListener
    {
        public GestureHandler GestureHandler;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
           //test(GestureHandler.)

        }

        public void test(Gesture gesture)
        {
            var origin = ((Tap)gesture).Origin;
            this.transform.Translate(origin.x, origin.y, origin.y);
        }
        public void OnGestureStart(Gesture gesture)
        {
            switch (gesture.Type)
            {
                case GestureType.Tap:
                    var origin = ((Tap)gesture).Origin;
                    this.transform.Translate(origin.x, origin.y, origin.y);
                    break;
            }
        }

    }
}

