using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login_Setter : MonoBehaviour {

    public InputField streamer, chat;
    public Text name_of_streamer;
    public GameObject to_disable;

    void Start()
    {
        Time.timeScale = 0;
    }

    public void set()
    {
        if(streamer.text != "" && chat.text != "")
        {
            name_of_streamer.text = streamer.text;
            PlayerPrefs.SetString("Streamer", streamer.text);
            PlayerPrefs.SetString("Chat", chat.text);
            to_disable.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
