using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Pack : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D c)
    {
        if(c.CompareTag("Player"))
        {
            c.GetComponent<Player>().TakeDamage(-10);
            c.GetComponent<Player>().Munch();
            Destroy(gameObject);
        }
    }
}
