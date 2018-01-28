using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Shooting : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(ShootEverywhere());
	}

    private IEnumerator ShootEverywhere()
    {
        while(true)
        {
            //shoots
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }
	
}
