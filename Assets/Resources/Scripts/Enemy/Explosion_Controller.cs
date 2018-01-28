using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_Controller : MonoBehaviour {

    public GameObject explosion;

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.0784f, 0.588f), Random.Range(.3921f, 1), Random.Range(0, .3921f), Random.Range(.3921f, .7843f));
        transform.Rotate(new Vector3(0, 0, Random.Range(0f, 180f)));
        Invoke("Destroy_Explosion", 1.5f);
	}
	
    void Destroy_Explosion()
    {
        Destroy(explosion);
    }
}
