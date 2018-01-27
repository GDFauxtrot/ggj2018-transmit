using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

	public float speed;
	// Update is called once per frame
	void Update () {
		Vector3 moving=new Vector3(speed * Time.deltaTime,0,0);
		transform.position+= (moving);
	}
}
