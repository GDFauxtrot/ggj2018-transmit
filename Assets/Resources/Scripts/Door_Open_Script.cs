using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Open_Script : MonoBehaviour
{

    public GameObject open, close;

    public AudioClip open_sounds, close_sounds;

    public AudioSource sounds;


    void OnTriggerStay2D(Collider2D c)
    {
        if ((c.CompareTag("Player") || c.CompareTag("Enemy")))
        {
            sounds.clip = open_sounds;
            sounds.Play();
            open.SetActive(true);
            close.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if ((c.CompareTag("Player") || c.CompareTag("Enemy")))
        {
            sounds.clip = close_sounds;
            sounds.Play();
            open.SetActive(false);
            close.SetActive(true);
        }
        
    }
}
