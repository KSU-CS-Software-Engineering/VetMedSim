using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	
	public float speed;
	public bool shouldFaceLeft;
	public float maxX;
	public float minX;
	
	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis("Horizontal");
		
		//move with the horizontal input
		if((horizontal > 0 && transform.position[0] < maxX) || (horizontal < 0 && transform.position[0] > minX))
		{
			transform.Translate(horizontal * Time.deltaTime * speed, 0f, 0f);
		}
		
		//face in the direction last moved
		if(horizontal > 0)
		{
			shouldFaceLeft = false;
		}
		else if(horizontal < 0)
		{
			shouldFaceLeft = true;
		}
		spriteRenderer.flipX = shouldFaceLeft;
	}
}
