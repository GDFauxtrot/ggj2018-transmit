using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour {

	// Use this for initialization
	public int size;
	public BulletPoolScriptable poolAPI;
	public GameObject bullet;
	void Start () {
		for (int i=0;i<size;i++){
			Instantiate(bullet,new Vector3(0,0,0),new Quaternion(0,0,0,0),transform).SetActive(false);
		}
        poolAPI.ResetScriptable();
		poolAPI.pool=gameObject;

    }
	

}
