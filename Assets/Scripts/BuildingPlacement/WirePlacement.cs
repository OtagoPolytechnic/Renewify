/// <summary>
/// This script is responsible for placing wires on the grid
/// TODO: Logic for changing to corner wires
/// TODO: Check for when a wire has been connected to the main station then change colour and lock it in place
/// TODO: If not connected when mouse click is released then remove the wire and the building and refund it to the inventory
/// TODO: If the building is deleted remove the wires connected to it
/// TODO: Wires can only be placed in 4way not diagonally
/// BUG: If the wire is dragged back over its own source building tile it will stop placement
///      Ideally this should allow you to keep placing but only if its the source building not any other
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePlacement : MonoBehaviour
{
    public GameObject StraightWire;
    public GameObject CornerWire;
    public Material NewMaterial;
    private List<int> tilesPlaced = new List<int>();
    private int lastTilePlaced = -1;

    // Update is called once per frame
    void Update()
    {
        //The first statement checks if the player has just placed a building and is now trying to place wires
        if (BuildingPlacing.WiresPlacing)
        {
            WirePlacingLogic();
        }
    }

    /// <summary>
    ///    The logic for placing wires
    ///    The player can place wires on empty tiles and remove wires by dragging over tiles that already have wires
    /// </summary>
    private void WirePlacingLogic()
    {
        //The player will only place wires for as long as they are holding down left click and dragging
        //It will also stop placing wires if the player is not hovering over a tile
        if (Input.GetMouseButton(0) && MouseManager.isHovering)
        {
            //If the player clicks on a tile that is empty, they can place a wire
            if (GridManager.IsTileEmpty(MouseManager.gridPosition))
            {
                PlaceWire(MouseManager.gridPosition, MouseManager.Instance.playerX, MouseManager.Instance.playerZ, StraightWire); //Wire will always be straight when first placed
                //Adding the placed tile to the list
                tilesPlaced.Add(MouseManager.gridPosition); 
                lastTilePlaced = MouseManager.gridPosition;
            }
            //If the player drags over a tile that already has a wire, they can remove everything placed since that tile was placed
            else if (lastTilePlaced != MouseManager.gridPosition)
            {
                while (tilesPlaced.Count > 0 && tilesPlaced[tilesPlaced.Count - 1] != MouseManager.gridPosition)
                {
                    RemoveWire(tilesPlaced[tilesPlaced.Count - 1]);
                    tilesPlaced.RemoveAt(tilesPlaced.Count - 1);
                    lastTilePlaced = MouseManager.gridPosition;
                    //If this removes all wires then stop placing wires
                    if (tilesPlaced.Count == 0)
                    {
                        Debug.Log("Wire Placement Stopped");
                        BuildingPlacing.WiresPlacing = false;
                        tilesPlaced.Clear();
                        lastTilePlaced = -1;
                    }
                }
            }
        }
        else
        {
            Debug.Log("Wire Placement Stopped");
            //If the player releases the left click, they are no longer placing wires
            BuildingPlacing.WiresPlacing = false;
            //Clear the list of placed tiles
            tilesPlaced.Clear();
            lastTilePlaced = -1;
        }
    }

    /// <summary>
    ///     Place a wire on the inputted tile
    /// </summary>
    /// <param name="position"> Position of the tile to place the wire on </param>
    /// <param name="wireX"> X position of the wire </param>
    /// <param name="wireZ"> Z position of the wire </param>
    /// <param name="wireType"> Type of wire to place </param>
    private void PlaceWire(int position, int wireX, int wireZ, GameObject wireType)
    {
        //If there is already a wire on the tile delete it first. This is assuming that the only reason a wire would be on a tile is if it was changing the wire type (Straight/Corner)
        if (GridManager.Instance.tileStates[position] == TileTypes.Wires)
        {
            RemoveWire(position);
        }
        //Instantiate the wire at the position of the tile
        GameObject temp = Instantiate(wireType, GridManager.CalculatePos(wireX, wireZ), Quaternion.identity);
        //set the parent of the wire to the tile
        temp.transform.SetParent(GridCreator.tiles[position].transform);
        GridManager.Instance.tileStates[MouseManager.gridPosition] = TileTypes.Wires;
    }

    /// <summary>
    ///   Remove a wire from the tile at the given position
    /// </summary>
    /// <param name="position"> Position of the tile to remove the wire from </param>
    private void RemoveWire(int position)
    {
        Destroy(GridCreator.tiles[position].transform.GetChild(0).gameObject);
        GridManager.Instance.tileStates[position] = TileTypes.None;
    }

    /// <summary>
    ///    Change the colour of the wire to the target material
    /// </summary>
    /// <param name="wire"> Game Object to be changed </param>
    /// <param name="targetMaterial"> Material to change it to </param>
    private void changeColour(GameObject wire, Material targetMaterial)
    {
        //change material of the all components in the wire to the target material
        foreach (Transform child in wire.transform)
        {
            if (child.gameObject.GetComponent<MeshRenderer>() != null)
            {
                child.gameObject.GetComponent<MeshRenderer>().material = targetMaterial;
                break;
            }
        }
    }
}
