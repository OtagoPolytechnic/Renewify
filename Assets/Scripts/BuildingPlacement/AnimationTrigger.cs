using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Animator windmillAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //Get the animator component
        windmillAnimator = gameObject.GetComponent<Animator>();
        //Getting the name of the parent gameobject
        string parentName = gameObject.transform.parent.gameObject.name;
        try
        {
            //Get the x and y of the parent tile from the parent's name
            int x = int.Parse(parentName[5].ToString());
            int y = int.Parse(parentName[7].ToString());
            //Check if the tile is connected to the goal
            bool isTileConnected = GridManager.Instance.tileBonus[GridManager.GetTileIndex(new Vector2(x,y))];
            //Set the connected parameter in the animator
            windmillAnimator.SetBool("Connected", isTileConnected);
        }
        catch
        {
            Debug.LogError("Error: Could not parse the parent name of the windmill");
        }
    }
}
