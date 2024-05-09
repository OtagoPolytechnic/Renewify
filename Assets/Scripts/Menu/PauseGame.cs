using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controls pausing the game. This will likely be used for the pause menu
/// </summary>
public class PauseGame : MonoBehaviour
{
    //Time.timescale is a static variable, and will persist through scene transitions
    //Some code will still run while paused, the CheckMouseHover() function in MouseManager.cs is a crutial function that will still run when paused. 
    //So this function now first checks if the game is unpaused before running 
    //Source: https://www.youtube.com/watch?v=ROwsdftEGF0 

    public static PauseGame Instance;
    public bool isPaused = false;
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }   

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    public void UnPause() //Make sure to call this function when exiting the main scene from the pause menu
    {
        isPaused = false;
        Time.timeScale = 1;
    }
}
