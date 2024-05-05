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
    //Some code will still run while paused, the CheckMouseHover() function in MouseManager.cs is a crutial function that will still run when paused. 
    //So this function now first checks if the game is unpaused before running 
    //Source: https://www.youtube.com/watch?v=ROwsdftEGF0 
    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void UnPause() //Make sure to call this function when exiting the main scene from the pause menu
    {
        Time.timeScale = 1;
    }
}