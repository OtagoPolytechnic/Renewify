using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controls pausing the game. This will likely be used for the pause menu
/// 
/// WIP: This script is currently unused.
/// </summary>
public class PauseGame : MonoBehaviour
{
    //Time.timescale is a static variable, and will persist through scene transitions
    //Source: https://www.youtube.com/watch?v=ROwsdftEGF0 
    public void Pause()
    {
        //TODO: mousemanager still functions while the game is paused, so you can place buildings while paused
        Time.timeScale = 0;
    }

    public void UnPause() //Make sure to call this function when exiting the main scene from the pause menu
    {
        Time.timeScale = 1;
    }
}
