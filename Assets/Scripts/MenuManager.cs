using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public GameObject modalPrefab;
    public Canvas canvas;

    void Start()
    {
        instance = this;
    }
    public void DisplayDialog(string title, string subTitle , Action onAccept)
    {
        GameObject g = GameObject.Instantiate(modalPrefab,canvas.transform);
        Modal m = g.GetComponent<Modal>();
        m.Initialize(title, subTitle); 
        m.OnAccept += onAccept; //When the accept button is clicked, the specified action is invoked
    }


}
