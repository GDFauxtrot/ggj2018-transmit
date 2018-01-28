using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Shooting : MonoBehaviour {

    public BulletPoolScriptable bulletPool;
    public GameObject left, right;


    // Use this for initialization
    void Start () {
        StartCoroutine(ShootEverywhere());
	}

    private IEnumerator ShootEverywhere()
    {
        while(true)
        {
            for(int i = 0; i < 20; ++i)
            {
                bool l = Random.Range(0, 2) == 0 ? true : false;

                Vector3 new_pos = l ? left.transform.position + new Vector3(Random.Range(-1, -5f), Random.Range(-1f, 1f), 0) : right.transform.position + new Vector3(Random.Range(1, 5f), Random.Range(-1f, 1f), 0);
                new_pos = new_pos - (l ? left.transform.position : right.transform.position);
                Quaternion angle = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(new_pos.y, new_pos.x) * Mathf.Rad2Deg));

                bulletPool.request(l ? left.transform.position : right.transform.position, angle, "boss");
                yield return new WaitForSeconds(0.1f);
            }
            //shoots
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }
	
}
