using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceQuit : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape) && Input.GetKey(KeyCode.X))
        {
            Application.Quit();
        }
    }
}
