using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	// Use this for initialization
	public GameObject bullet;
	private Rigidbody2D rb2D;
	private Transform reticle;
	public float speed;


	void Awake()
	{
		reticle = transform.GetChild(0).GetComponent<Transform>();
		rb2D = GetComponent<Rigidbody2D>();
	} 

	
	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(bullet, reticle.position + (reticle.right/5), Quaternion.Euler(0, 0, reticle.rotation.eulerAngles.z));
        }
	}
	// Update is called once per frame
	void FixedUpdate() {
		Vector2 movement = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

        float mag = movement.magnitude;
        
        if (mag < 1f) {
            movement.Normalize();
            movement = new Vector2(movement.x * mag, movement.y * mag);
        } else {
            movement.Normalize();
        }
        
        Debug.Log(movement.magnitude);
        rb2D.MovePosition(rb2D.position+movement*speed*Time.deltaTime);
		// Mathf.Atan2 returns the tangent line to the two float values given, and then we multiple it to get it as an angle.
        reticle.eulerAngles = new Vector3(0, 0,Mathf.Atan2(Input.GetAxis("RstickVertical"), Input.GetAxis("RstickHorizontal")) * 180 / Mathf.PI);
    } 
		
}
