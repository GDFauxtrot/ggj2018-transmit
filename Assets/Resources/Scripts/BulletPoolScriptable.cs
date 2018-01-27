using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu( menuName = "bullet handler")]
public class BulletPoolScriptable : ScriptableObject {

	int size=0; // please check before firing
	public GameObject pool;
	GameObject active;

	public void ResetScriptable()
	{
		if(pool!=null)
			size=pool.transform.childCount;
		else
			size=0;
	}

	public void request(Vector3 requesty, Quaternion angle)// used to request and send
	{
		var child = pool.transform.GetChild(0);
		child.transform.position=requesty;
		child.transform.rotation=angle;
		child.gameObject.SetActive(true);
		child.gameObject.GetComponent<bullet>().ReturnToPool();
	}
}
