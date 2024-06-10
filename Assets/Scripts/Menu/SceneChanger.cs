using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //This function was originally in MenuNavigation.cs, but I moved it here so it can be used outside of the main menu.
    public void OpenScene(string levelName) //eg: "Level 1"
    {
        SceneManager.LoadScene(levelName);
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
