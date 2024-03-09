using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() //This function will be called when the play button is clicked and load the main scene
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void Options() //This function will be called when the options button is clicked and load the options scene
    {
        //SceneManager.LoadScene("Options");
    }

    public void LevelSelect() //This function will be called when the level select button is clicked and load the level select scene
    {
        //SceneManager.LoadScene("LevelSelect");
    }

    public void QuitGame() //This function will be called when the quit button is clicked and quit the game
    {
        bool confirmQuit = EditorUtility.DisplayDialog(
            "Quit Game",
            "Are you sure you want to quit?",
            "Yes",
            "No"
        );
        if (confirmQuit == true)
        {
            Application.Quit();
        }
    }
}
