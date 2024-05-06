using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public Tooltip mainTooltip;
    // Start is called before the first frame update


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        mainTooltip.SetTitle("Welcome to the Tutorial!");
    }



    public void WiringSection()
    {
        //Highlight the tiles leading to the power source
        /*
            Vector2(1, 3)
            Vector2(2, 3)
            Vector2(3, 3)
            Vector2(3, 4)
            Vector2(3, 5)
            Vector2(3, 6)
            Vector2(3, 7)
            Vector2(3, 8)
        tile.GetComponent<Renderer>().material = highlighted;


        */
    }

    public void DeletionSection()
    {
        //Add a new building and show how to delete it
    }
}
