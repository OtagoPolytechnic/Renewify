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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WiringSection()
    {
        //Highlight the tiles leading to the power source
    }
}
