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
            //Make sure the bonus tile is for a windmill
            bool isTileBonus = false;
            Vector2 buildingPosition = new Vector2(x, y);
            // foreach (var tile in GridManager.Instance.scoreTiles)
            // {
            //     if (tile.position == buildingPosition)
            //     {
            //         if (tile.building != TileTypes.Windmills)
            //         {
            //             return;
            //         } 
            //         isTileBonus = true;
            //         break;  
            //     }
            // }

            for (int i = 0; i < GridManager.Instance.scoreTiles.Count; i++)
            {
                //checks if the score tile is a windmill
                if(GridManager.Instance.scoreTiles[i].position == buildingPosition)
                {
                    if (GridManager.Instance.scoreTiles[i].building != TileTypes.Windmills)
                    {
                        Debug.Log("Building not windmill");
                        Debug.Log(GridManager.Instance.scoreTiles[i].building);
                        return;
                    } 
                    Debug.Log("Building is windmill");
                    isTileBonus = true;
                    break;  
                }

                //Loops through all adjacent tiles
                for (int j = 0; j < GridManager.Instance.scoreTiles[i].adjacent.Count; j++)
                {
                    //checks if the adjacent tile is a windmill
                    if(GridManager.Instance.scoreTiles[i].adjacent[j].position == buildingPosition)
                    {
                        if (GridManager.Instance.scoreTiles[i].adjacent[j].building != TileTypes.Windmills)
                        {
                            return;
                        } 
                        isTileBonus = true;
                        break;  
                    }
                }

                //Loops through all diagonal tiles
                for (int k = 0; k < GridManager.Instance.scoreTiles[i].diagonals.Count; k++)
                {
                    //Checks if the diagonal tile is a windmill
                    if(GridManager.Instance.scoreTiles[i].diagonals[k].position == buildingPosition)
                    {
                        if (GridManager.Instance.scoreTiles[i].diagonals[k].building != TileTypes.Windmills)
                        {
                            return;
                        } 
                        isTileBonus = true;
                        break;  
                    }
                }

            }

            //Check if the tile is connected to the goal
            // bool isTileConnected = GridManager.Instance.tileBonus[GridManager.GetTileIndex(new Vector2(x,y))];
            //Set the connected parameter in the animator
            windmillAnimator.SetBool("Connected", isTileBonus);
        }
        catch
        {
            Debug.LogError("Error: Could not parse the parent name of the windmill");
        }
    }
    
}
