using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
        public static MenuManager instance;

    public GameObject modalPrefab;

    void Start()
    {
        instance = this;
    }
    public void DisplayDialog(string Title, string subTitle)
    {
        GameObject g = GameObject.Instantiate(modalPrefab,);
    }

}
