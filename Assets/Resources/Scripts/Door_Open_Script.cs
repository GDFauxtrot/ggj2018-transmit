using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Open_Script : MonoBehaviour
{

    public GameObject open, close;


    void OnTriggerStay2D(Collider2D c)
    {
        if ((c.CompareTag("Player") || c.CompareTag("Enemy")))
        {
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
