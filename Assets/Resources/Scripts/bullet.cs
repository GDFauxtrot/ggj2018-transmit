using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

	public float speed;
	// Update is called once per frame
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
       // transform.rotation=(transform.localRotation);
    }
	void Update () {
		Vector3 moving=transform.right*speed*Time.deltaTime;

		transform.position+= (moving);
		Invoke("bulletBeGone",2f);
	}

	void bulletBeGone()
	{
		Destroy(this.gameObject);
	}
}
