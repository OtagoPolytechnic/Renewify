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
    public static TileTypes selectedBuilding = TileTypes.None;
    //Note: This is just to get it working as I don't have anywhere to attach the event trigger to yet. Will be changed out of update
    void Update()
    {
        //If the user clicks the mouse
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }
    }

    public void OnMouseDown()
    {
        if(selectedBuilding != TileTypes.None && MouseManager.isHovering)
        {
            Debug.Log("Placing building");
            placeBuilding();
        }
    }

    /// <summary>
    /// Places a building where the user clicks    
    /// </summary>
    private void placeBuilding()
    {
        int playerX = MouseManager.Instance.playerX;
        int playerZ = MouseManager.Instance.playerZ;
        if (GridManager.IsTileEmpty(MouseManager.gridPosition) && InventoryManagement.instance.BuildingsLeft())
        {
            //Pass through the building I want to be placed
            GridManager.Instance.tileStates[MouseManager.gridPosition] = selectedBuilding;
            //Remove a building from the inventory
            InventoryManagement.instance.PlaceSelectedBuilding();
            //Place the building
            //Get the prefab with the same name as the building variable
            switch(selectedBuilding)
            {
                case TileTypes.Windmills:
                    spawnBuilding(Windmill, playerX, playerZ);
                    break;
                case TileTypes.SolarPanels:
                    spawnBuilding(SolarPanelField, playerX, playerZ);
                    break;
                default:
                    Debug.Log("No valid building type selected");
                    break;
            }
            selectedBuilding = TileTypes.None;
        }
        {
            //Show a message saying that the space is full
            Debug.Log("Space is full");
        }
    }

    private void spawnBuilding(GameObject building, int playerX, int playerZ)
    {
        GameObject temp = Instantiate(building, GridManager.CalculatePos(playerX, playerZ), Quaternion.identity);
        //Rotate temp by 180 degrees as the model is facing the wrong way no matter how I rotate the prefab?
        temp.transform.Rotate(0, 180, 0);
    }
}
