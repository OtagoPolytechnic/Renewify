/// <summary>
/// This script is used to place buildings on the grid
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacing : MonoBehaviour
{
    public GameObject Windmill;
    public GameObject SolarPanelField;
    //Enum building variable
    //public static BuildingType selectedBuilding = NONE;
    private void OnMouseDown()
    {
        //if(selectedBuilding != NONE && mouse is over a grid space)
        //{
            //placeBuilding();
        //}
    }

    private void selectBuilding()
    {
        //Get the building that was clicked on
        //Set the building to the selected building
    }

    /// <summary>
    /// Places a building where the user clicks    
    /// </summary>
    /// <param name="building">Building currently selected</param>
    private void placeBuilding() //TODO: Add enum variable input for building type
    {
        //Temp variables declared until merged with grid branch.
        //TODO: Change commented out variables to the real ones and delete temp variables.
        //int playerX = MouseManager.Instance.playerX;
        //int playerZ = MouseManager.Instance.playerZ;
        //Get this from the grid script list for the player x and y
        //Use the ENUM for building types
        //if (GridManager.IsTileEmpty(MouseManager.gridPosition))
        {
            //Pass through the building I want to be placed
            //GridManager.tileStates[MouseManager.gridPosition] == 
            //Place the building
            //Get the prefab with the same name as the building variable
            //GameObject temp = Instantiate(building, GridManager.CalculatePos(playerX, playerZ), Quaternion.identity);
            //selectBuilding = NONE;
        }
        {
            //Show a message saying that the space is full
        }
    }
}