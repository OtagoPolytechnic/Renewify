/// <summary>
/// This script is responsible for placing wires on the grid
/// TODO: Check for when a wire has been connected to the main station then change colour and lock it in place
/// TODO: If not connected when mouse click is released then remove the wire and the building and refund it to the inventory
/// TODO: If the building is deleted remove the wires connected to it
/// BUG: If you backtrack over a corner wire it doesn't remove anything
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePlacement : MonoBehaviour
{
    public GameObject StraightWire;
    public GameObject CornerWire;
    public Material NewMaterial;
    private List<int> tilesPlaced = new List<int>();
    private int startingTile = -1;
    private int lastTile = -1;
    private int secondLastTile = -1;
    private int gridsize;

    // Start is called before the first frame update
    void Start()
    {
        gridsize = GridManager.Instance.gridSize;   
    }
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
            //If this is the first tile set starting tile to the current tile
            if (startingTile == -1)
            {
                startingTile = MouseManager.gridPosition;
            }
            lastTile = tilesPlaced.Count > 0 ? tilesPlaced[tilesPlaced.Count - 1] : startingTile; //Either gets the last tile or the starting tile
            secondLastTile = tilesPlaced.Count > 1 ? tilesPlaced[tilesPlaced.Count - 2] : startingTile; //Either gets the second last tile or the starting tile
            //If the player drags through a tile that is empty, they can place a wire
            if (GridManager.IsTileEmpty(MouseManager.gridPosition))
            {
                int rotation = 0;
                //If the player is trying to place a wire diagonally, they can't
                //This uses abs and the grid size to check if the difference in both directions is over 0
                if ((Math.Abs((MouseManager.gridPosition % gridsize) - (lastTile % gridsize)) > 0) &&
                (Math.Abs((MouseManager.gridPosition / gridsize) - (lastTile / gridsize)) > 0))
                {
                    resetTileList();
                    return;
                }
                //Getting the corners to calculate, delete, and place properly took me multiple hours. It's ugly but it works and I am actually scared to touch it in case it stops working.
                //This checks if the player has placed a wire on a different axis as the last two tiles
                if ((lastTile / gridsize != secondLastTile / gridsize || lastTile / gridsize != MouseManager.gridPosition / gridsize) &&
                    (lastTile % gridsize != secondLastTile % gridsize || lastTile % gridsize != MouseManager.gridPosition % gridsize))
                {
                    // Determine the direction of movement from the second last tile to the last tile and from the last tile to the current tile
                    bool isMovingRightOrUpSecondLastToLast = secondLastTile < lastTile;
                    bool isMovingRightOrUpLastToCurrent = lastTile < MouseManager.gridPosition;

                    // Determine the rotation of the corner wire. I just put different rotations in until all directions worked.
                    if (MouseManager.gridPosition / gridsize == lastTile / gridsize)
                    {
                        if (isMovingRightOrUpSecondLastToLast)
                        {
                            rotation = isMovingRightOrUpLastToCurrent ? 270 : 180;
                        }
                        else
                        {
                            rotation = isMovingRightOrUpLastToCurrent ? 0 : 90;
                        }
                    }
                    else
                    {
                        if (isMovingRightOrUpSecondLastToLast)
                        {
                            rotation = isMovingRightOrUpLastToCurrent ? 90 : 180;
                        }
                        else
                        {
                            rotation = isMovingRightOrUpLastToCurrent ? 0 : 270;
                        }
                    }
                    // Replace the last wire with a corner wire
                    PlaceWire(lastTile, lastTile / gridsize, lastTile % gridsize, rotation, CornerWire);
                }
                //Checking the rotation of the wire
                if (MouseManager.gridPosition / gridsize == lastTile / gridsize)
                {
                    rotation = 90;
                }
                else
                {
                    rotation = 0;
                }
                //Place the wire on the current tile
                PlaceWire(MouseManager.gridPosition, MouseManager.Instance.playerX, MouseManager.Instance.playerZ, rotation, StraightWire);
                //Adding the placed tile to the list
                tilesPlaced.Add(MouseManager.gridPosition); 
            }
            //If the player drags over a tile that already has a wire, they can remove everything placed since that tile was placed
            else if (lastTile != MouseManager.gridPosition)
            {
                while (tilesPlaced.Count > 0 && tilesPlaced[tilesPlaced.Count - 1]!= MouseManager.gridPosition)
                {
                    RemoveWire(tilesPlaced[tilesPlaced.Count - 1]);
                    tilesPlaced.RemoveAt(tilesPlaced.Count - 1);
                    lastTile = tilesPlaced.Count > 0 ? tilesPlaced[tilesPlaced.Count - 1] : startingTile; //Either gets the last tile or the starting tile
                    secondLastTile = tilesPlaced.Count > 1 ? tilesPlaced[tilesPlaced.Count - 2] : startingTile; //Either gets the second last tile or the starting tile
                    //If this removes all wires then stop placing wires unless this is the starting tile
                    if (tilesPlaced.Count == 0 && lastTile != startingTile)
                    {
                        resetTileList();
                    }
                }
            }
        }
        else
        {
            //If the player releases the left click, they are no longer placing wires
            resetTileList();
        }
    }

    /// <summary>
    ///    Reset the list of placed tiles and stop placing wires
    /// </summary>
    private void resetTileList()
    {
        BuildingPlacing.WiresPlacing = false;
        //Clear the list of placed tiles
        tilesPlaced.Clear();
        startingTile = -1;
        lastTile = -1;
        secondLastTile = -1;
    }

    /// <summary>
    ///     Place a wire on the inputted tile
    /// </summary>
    /// <param name="position"> Position of the tile to place the wire on </param>
    /// <param name="wireX"> X position of the wire </param>
    /// <param name="wireZ"> Z position of the wire </param>
    /// <param name="rotation"> Rotation of the wire </param>
    /// <param name="wireType"> Type of wire to place </param>
    private void PlaceWire(int position, int wireX, int wireZ, int rotation, GameObject wireType)
    {
        //Clear any wire that already exists on the tile
        RemoveWire(position);
        //Instantiate the wire at the position of the tile
        GameObject temp = Instantiate(wireType, GridManager.CalculatePos(wireX, wireZ), Quaternion.Euler(0, rotation, 0));
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
        //If the tile has a wire, remove it
        if (GridCreator.tiles[position].transform.childCount > 0)
        {
            Destroy(GridCreator.tiles[position].transform.GetChild(0).gameObject);
            GridManager.Instance.tileStates[position] = TileTypes.None;
        }
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
