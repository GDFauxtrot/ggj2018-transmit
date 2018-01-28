using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplosionDeleter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("delete", 1);
	}

    void delete()
    {
        Destroy(gameObject);
    }
}
