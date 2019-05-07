using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour
{

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    Pathfinding pathfinding;

    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    /// <summary>
    /// Requests a new path
    /// </summary>
    /// <param name="pathStart">Location of the beginning of the path</param>
    /// <param name="pathEnd">Location of the ending of the path</param>
    /// <param name="callback">the callback</param>
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    /// <summary>
    /// Trys to process next Pathrequest from the queue
    /// </summary>
    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }
    /// <summary>
    /// returns if path is successful
    /// </summary>
    /// <param name="path">the given path</param>
    /// <param name="success">whether the request was successful</param>
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    /// <summary>
    /// struct for PathRequest
    /// </summary>
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        /// <summary>
        /// Constructor for PathRequest
        /// </summary>
        /// <param name="_start">begging of a path</param>
        /// <param name="_end">ending of a path</param>
        /// <param name="_callback">callback</param>
        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }

    }
}