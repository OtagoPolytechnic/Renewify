using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Animator windmillAnimator;
    private Vector2 newVector2;

    // Start is called before the first frame update
    void Start()
    {
        windmillAnimator = gameObject.GetComponent<Animator>();

        //get the parent of gameObject
        Transform parent = gameObject.transform.parent;
        if(parent != null)
        {
            char x = parent.gameObject.name[5];
            char y = parent.gameObject.name[7];
            newVector2 = new Vector2(x,y);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Implement after vector2 refactoring
        //bool isTileConnected = WirePlacement.Instance.isTileConnected(newVector2);

        //Testing for current codebase
        bool isTileConnected = WirePlacement.Instance.isTileConnected(1);

        windmillAnimator.SetBool("Connected", isTileConnected);
    }

}
