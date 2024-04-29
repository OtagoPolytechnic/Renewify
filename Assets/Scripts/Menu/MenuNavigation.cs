using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    private GameObject mainMenuParent;
    private GameObject levelSelectParent;

    void Start()
    {
        mainMenuParent = GameObject.Find("MainMenu");
        levelSelectParent = GameObject.Find("LevelSelect");
        mainMenuParent.SetActive(true);
        levelSelectParent.SetActive(false);
    }

    public void OpenLevelSelect()
    {
        mainMenuParent.SetActive(false);
        levelSelectParent.SetActive(true);
    }
    public void CloseLevelSelect()
    {
        mainMenuParent.SetActive(true);
        levelSelectParent.SetActive(false);
    }

    public void QuitGame() //This function will be called when the quit button is clicked and quit the game
    {
        MenuManager.instance.DisplayDialog(
            "Quit Game?",
            "Are you sure you want to quit?",
            () => Application.Quit() //This is the action that will be invoked when the accept button is clicked
        );
    }
}
