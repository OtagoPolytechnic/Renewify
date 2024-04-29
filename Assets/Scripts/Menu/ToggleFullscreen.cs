using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFullscreen : MonoBehaviour
{
    void Start()
    {
        LoadPrefs();
    }

    public void Fullscreen()
    {
        if(gameObject.GetComponent<Toggle>().isOn)
        {
            PlayerPrefs.SetInt("fullscreen", 1);
        }else
        {
            PlayerPrefs.SetInt("fullscreen", 0);
        }

        Screen.fullScreen = gameObject.GetComponent<Toggle>().isOn;
    }

    public void LoadPrefs()
    {
        bool fullscreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;

        Screen.fullScreen = fullscreen;
        gameObject.GetComponent<Toggle>().isOn = fullscreen;
    }
}
