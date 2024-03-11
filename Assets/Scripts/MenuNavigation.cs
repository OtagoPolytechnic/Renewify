using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{

    public void OpenScene(string levelName) //eg: "Level 1"
    {
         SceneManager.LoadScene(levelName);
    }
    

    public void QuitGame() //This function will be called when the quit button is clicked and quit the game
    {
        MenuManager.instance.DisplayDialog("Quit Game?", "Are you sure you want to quit?");
        // if (confirmQuit == true)
        // {
        //     Application.Quit();
        // }
    }
}
