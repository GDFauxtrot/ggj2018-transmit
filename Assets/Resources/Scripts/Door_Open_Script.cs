using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Open_Script : MonoBehaviour
{

    private GameObject thing_Colliding;
    public GameObject open, close;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerStay2D(Collider2D c)
    {
        if ((c.CompareTag("Player") || c.CompareTag("Enemy")))
        {
            thing_Colliding = c.gameObject;
            open.SetActive(true);
            close.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if ((c.CompareTag("Player") || c.CompareTag("Enemy")))
        {
            open.SetActive(false);
            close.SetActive(true);
        }
        
    }
}
