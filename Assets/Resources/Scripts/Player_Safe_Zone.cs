using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Safe_Zone : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("SpawnPoint"))
            c.GetComponent<SpawnPointIsOn>().isactive = false;
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("SpawnPoint"))
            c.GetComponent<SpawnPointIsOn>().isactive = true;
    }
}
