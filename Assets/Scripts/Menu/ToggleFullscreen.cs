using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFullscreen : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Toggle>().isOn = Screen.fullScreen;
        //Debug.Log(Screen.fullScreen);
    }

    public void Fullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt("Volume", 50);
        PlayerPrefs.Save();
    }
    
    public void LoadPrefs()
    {
        int volume = PlayerPrefs.GetInt("Volume", 0); 
    }
}
