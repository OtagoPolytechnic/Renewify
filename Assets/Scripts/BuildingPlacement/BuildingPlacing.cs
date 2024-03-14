/// <summary>
/// 
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacing : MonoBehaviour
{
    private void OnMouseDown()
    {
        //If the cursor is over one of the building selection panels
        selectBuilding();
        //If there is a building selected and the mouse is over a grid space
        placeBuilding();
    }

    private void selectBuilding()
    {
        //Get the building that was clicked on
        //Set the building to the selected building
    }

    /// <summary>
    /// Places a building where the user clicks    
    /// </summary>
    /// <param name="building"></param>
    private void placeBuilding(Buildings building) //TODO: Add enum variable input for building type
    {
        //Temp variables declared until merged with grid branch.
        //TODO: Change commented out variables to the real ones and delete temp variables.
        //int playerX = MouseManager.Instance.playerX;
        int playerX = 2;
        //int playerZ = MouseManager.Instance.playerZ;
        int playerZ = 2;
        //Get this from the grid script list for the player x and y
        //Use the ENUM for building types
        if (true) //TODO: Check if the tile is empty
        {
            //GridManager.CalculatePos(playerX, playerZ);
            //Set tile to the building
            //tile = building;
            //Place the building
        }
        {
            //Show a message saying that the space is full
        }
        //Calculates the center of the grid space
    }
}
