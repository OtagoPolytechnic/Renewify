/// <summary>
/// This script is responsible for placing wires on the grid
/// TODO: If not connected when mouse click is released then remove the building and refund it to the inventory
/// TODO: If the building is deleted remove the wires connected to it
/// CONSIDER: Accidentally going diagonally doesn't feel great. Not sure how this would be changed without introducing other issues.
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePlacement : MonoBehaviour
{
    public GameObject StraightWire;
    public GameObject CornerWire;
    private GameObject lastWire;
    public Material CompletedConnection; //Just using one material for now. Can change to different ones based on the building
    private Material selectedMaterial; 
    private List<int> tilesPlaced = new List<int>();
    private int startingTile = -1;
    private int lastTile = -1;
    private int secondLastTile = -1;
    private int gridsize;
    private TileTypes goal = TileTypes.Goal;
    //I have an intermediary variable so that I only need to change it in one place if the name of the tile is changed in the goal branch

    // Start is called before the first frame update
    void Start()
    {
        gridsize = GridManager.Instance.gridSize;   
    }
    // Update is called once per frame
    void Update()
    {
        //This checks if the player has just placed a building and is now trying to place wires
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
                //If the player is trying to place a wire diagonally (or somehow across multiple tiles), remove the wires placed so far
                //This uses abs and the grid size to check if the difference in both directions is over 0
                if ((Math.Abs((MouseManager.gridPosition % gridsize) - (lastTile % gridsize)) > 0) &&
                (Math.Abs((MouseManager.gridPosition / gridsize) - (lastTile / gridsize)) > 0))
                {
                    foreach (int tile in tilesPlaced)
                    {
                        RemoveWire(tile);
                    }
                    resetTileList();
                    return;
                }
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
                    lastWire = CornerWire; //Set the last wire to a corner wire
                }
                else
                {
                    //If the player is placing a wire straight
                    //This is so that when you backtrack over a corner wire it will properly straight if it needs to
                    if (lastTile / gridsize == secondLastTile / gridsize)
                    {
                        rotation = 90;
                    }
                    else
                    {
                        rotation = 0;
                    }
                    lastWire = StraightWire;
                }
                //Place the wire on the last tile if it is not the starting tile
                if (lastTile != startingTile)
                {
                    PlaceWire(lastTile, lastTile / gridsize, lastTile % gridsize, rotation, lastWire);
                }
                //Checking the rotation of the current wire
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
            //If they drag over the goal tile, they lock in the wire
            else if (GridManager.Instance.tileStates[MouseManager.gridPosition] == goal)
            {
                //Can swap out the material based on the building when we add the materials
                switch (GridManager.Instance.tileStates[startingTile])
                {
                    case TileTypes.SolarPanels:
                        selectedMaterial = CompletedConnection;
                        break;
                    case TileTypes.Windmills:
                        selectedMaterial = CompletedConnection;
                        break;
                    //case TileTypes. whatever we use for water
                    //    selectedMaterial = CompletedConnection;
                    //    break;
                    default:
                        Debug.Log("The source of the wire was not a valid building type.");
                        selectedMaterial = CompletedConnection;
                        break;
                }
                //Change the colour of the wire to the target material
                foreach (int tile in tilesPlaced)
                {
                    changeColour(GridCreator.tiles[tile].transform.GetChild(0).gameObject, CompletedConnection);
                }
                //Stops placing and resets the list for the next wire
                resetTileList();
            }
            //If the player drags over a tile that already has a wire, they can remove everything placed since that tile was placed
            //If it is the source building it removes everything but lets the player keep placing wires
            //If it is the edge or another building it removes everything and stops the player from placing wires
            else if (lastTile != MouseManager.gridPosition)
            {
                //While there are tiles left and it hasn't gotten to the tile the player is currently hovering over
                while (tilesPlaced.Count > 0 && tilesPlaced[tilesPlaced.Count - 1]!= MouseManager.gridPosition)
                {
                    //Removed the wires from the tile and remove the tile from the list
                    RemoveWire(tilesPlaced[tilesPlaced.Count - 1]);
                    tilesPlaced.RemoveAt(tilesPlaced.Count - 1);
                    lastTile = tilesPlaced.Count > 0 ? tilesPlaced[tilesPlaced.Count - 1] : startingTile; //Resets what the last tile and second last tile are every time one is deleted
                    secondLastTile = tilesPlaced.Count > 1 ? tilesPlaced[tilesPlaced.Count - 2] : startingTile; 
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
            //If the player releases the left click before reaching the goal tile, remove the wires and refund the building
            foreach (int tile in tilesPlaced)
            {
                RemoveWire(tile);
            }
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
        GridManager.Instance.tileStates[position] = TileTypes.Wires;
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
        Debug.Log(wire.name);
        //change material of the all components in the wire to the target material
        foreach (Transform child in wire.transform)
        {
            Debug.Log(child.name);
            if (child.gameObject.GetComponent<MeshRenderer>() != null)
            {
                child.gameObject.GetComponent<MeshRenderer>().material = targetMaterial;
            }
        }
    }
}
