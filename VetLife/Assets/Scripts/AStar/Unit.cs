using UnityEngine;
using System.Collections;

namespace Assets.Scripts.UserInput
{
    public class Unit : MonoBehaviour, IGestureListener
    {


        public Transform target;
        float speed = (float)4;
        Vector3[] path;
        int targetIndex;

        /// <summary>
        /// Object handling gesture recognition
        /// </summary>
        public GestureHandler GestureHandler;
        

        /// <summary>
		/// Animator component of player object
		/// </summary>
		internal Animator Animator { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            GestureHandler.RegisterListener(this);
        }

        private Camera cam;
        void Start()
        {
            cam = Camera.main;
            // PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        }

        void Update()
        {
            
        }
        internal Vector2 Position => gameObject.transform.position;

        public void OnGestureStart(Gesture gesture)
        {
            //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            switch (gesture.Type)
            {
                case GestureType.Tap:

                    Vector3 point = new Vector3();
                    Event currentEvent = Event.current;
                    Vector2 tapPos = new Vector2();

                    var origin = ((Tap)gesture).Origin;
                    // Get the mouse position from Event.
                    // Note that the y position from Event is inverted.
                    tapPos.x = origin.x;
                    tapPos.y = cam.pixelHeight - origin.y;

                    point = cam.ScreenToWorldPoint(new Vector3(tapPos.x, cam.nearClipPlane, tapPos.y));

                    //Vector3 targetLocation = new Vector3(origin.x, 0, origin.y);
                    PathRequestManager.RequestPath(transform.position, point, OnPathFound);
                    break;
            }
        }

        public void OnGestureEnd(Gesture gesture)
        {
            //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            // intentionally left blank
        }

        public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                path = newPath;
                targetIndex = 0;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        IEnumerator FollowPath()
        {
            Vector3 currentWaypoint = path[0];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;

            }
        }

        public void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }
    }
}