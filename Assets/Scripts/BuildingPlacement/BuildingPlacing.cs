using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //If the cursor is over one of the building selection panels
        selectBuilding();
        //If there is a building selected and the mouse is over a grid space
        placeBuilding();
    }

    selectBuilding()
    {
        //Get the building that was clicked on
        //Set the building to the selected building
    }

    private void placeBuilding()
    {
        //Get X, Y, and Size from the GridCreator script
        int playerX = 2;
        int playerY = 2;
        int gridSize = 10;
        float tileSize = 10.0f;
        //Calculation from Liam's script
        float xPos = (x * tileSize) - (gridSize / 2 * tileSize) + (tileSize / 2);
        float yPos = (y * tileSize) - (gridSize / 2 * tileSize) + (tileSize / 2);
        //Check if the space is empty. Figure out how to track this?
        bool empty = true;
        if (empty)
        {
            //Set space to full
            empty = false;
            //Place the building
        }
        {
            //Show a message saying that the space is full
        }
    }
}
