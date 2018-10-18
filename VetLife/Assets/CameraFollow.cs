using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public Transform target;
    public float smoothTime = 0.3F;
	public float posY;
	public float minX;
	public float maxX;
	
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, posY, -10));

        // Smoothly move the camera towards that target position
        Vector3 desiredPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
		transform.position = new Vector3(Mathf.Clamp(desiredPosition.x, minX, maxX), desiredPosition.y, desiredPosition.z);
    }
}
