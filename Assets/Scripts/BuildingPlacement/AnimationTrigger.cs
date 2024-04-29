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
            
            int x_ = int.Parse(x.ToString());
            int y_ = int.Parse(y.ToString());
            newVector2 = new Vector2(x_,y_);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if tile is connected to a building
        bool isTileConnected = WirePlacement.Instance.isTileConnected(GridManager.GetTileIndex(newVector2));
        //Debug.Log(GridManager.GetTileIndex(newVector2));
        

        windmillAnimator.SetBool("Connected", isTileConnected);
    }

}
