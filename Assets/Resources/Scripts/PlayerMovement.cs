using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	// Use this for initialization
	private Rigidbody2D rb2D;
	public Transform reticle;
	public float speed;


	void Awake()
	{
		reticle=this.gameObject.transform.GetChild(0).GetComponent<Transform>();
		rb2D=gameObject.GetComponent<Rigidbody2D>();
	} 

	
	// Update is called once per frame
	void FixedUpdate()
	{
		Vector2 movement= new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

        movement.Normalize();
		rb2D.MovePosition(rb2D.position+movement*speed);
		// Mathf.Atan2 returns the tangent line to the two float values given, and then we multiple it to get it as an angle.
        reticle.eulerAngles = new Vector3(0, 0,Mathf.Atan2(Input.GetAxis("RstickVertical"), Input.GetAxis("RstickHorizontal")) * 180 / Mathf.PI);


    } 
		
}
