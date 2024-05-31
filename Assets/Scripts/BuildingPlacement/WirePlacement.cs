/// <summary>
/// This script is responsible for placing wires on the grid
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WirePlacement : MonoBehaviour
{
    public GameObject StraightWire;
    public GameObject CornerWire;
    public GameObject  PowerParticle;
    private GameObject lastWire;
    public Material CompletedConnection; //Just using one material for now. Can change to different ones based on the building
    private Material selectedMaterial;
    private List<Vector2> tilesPlaced = new List<Vector2>();
    private List<List<Vector2>> wiresPlaced = new List<List<Vector2>>(); //This is the list of saved wires so they can be removed if the building is removed
    [SerializeField] private List<Vector2> buildingTiles = new List<Vector2>(); //This is the list of building tiles that don't have a wire attached to them
    private List<Vector2> connectedBuildings = new List<Vector2>(); //Buildings that have been connected to the goal
    private Vector2 startingTile = new Vector2(-1, -1);
    private Vector2 lastTile = new Vector2(-1, -1);
    private Vector2 secondLastTile = new Vector2(-1, -1);
    private int gridsize;
    private TileTypes goal = TileTypes.Goal;
    //I have an intermediary variable so that I only need to change it in one place if the name of the tile is changed in the goal branch

    //Singleton pattern
    public static WirePlacement Instance;

    public List<Vector2> ConnectedBuildings { get => connectedBuildings; } //public getter for connected buildings

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gridsize = GridManager.Instance.gridSize;
    }
    // Update is called once per frame
    void Update()
    {
        if (InventoryManagement.instance.deleteMode.isOn) //If delete mode is on, don't place wires
        {
            return;
        }
        //This checks if the player has just placed a building and is now trying to place wires
        if (BuildingPlacing.WiresPlacing)
        {
            WirePlacingLogic();
        }
        else if (buildingTiles.Contains(MouseManager.gridPosition) && Input.GetMouseButtonDown(0))
        {
            BuildingPlacing.WiresPlacing = true;
            startingTile = MouseManager.gridPosition;
        }
    }

    /// <summary>
    ///   Check if a building is connected to the goal tile
    /// </summary>
    /// <param name="gridPosition">Building to check</param>
    /// <returns></returns>
    public bool isTileConnected(int gridPosition)
    {
        if (connectedBuildings.Contains(GridManager.GetTilePosition(gridPosition)))
        {
            return true;
        }
        else
        {
            return false;
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
            if (startingTile == new Vector2(-1, -1))
            {
                startingTile = MouseManager.gridPosition;
                buildingTiles.Add(startingTile);
            }
            lastTile = tilesPlaced.Count > 0 ? tilesPlaced[tilesPlaced.Count - 1] : startingTile; //Either gets the last tile or the starting tile
            secondLastTile = tilesPlaced.Count > 1 ? tilesPlaced[tilesPlaced.Count - 2] : startingTile; //Either gets the second last tile or the starting tile
            //If the player drags through a tile that is empty, they can place a wire
            if (GridManager.IsTileEmpty(GridManager.GetTileIndex(MouseManager.gridPosition)))
            {
                int rotation = 0;
                //If the player is trying to place a wire diagonally or across multiple tiles, remove the wires placed so far
                //This uses abs and the grid size to check if the difference in both directions is over 0
                if (illegalMoveCheck())
                {
                    foreach (Vector2 tile in tilesPlaced)
                    {
                        RemoveWire(GridManager.GetTileIndex(tile));
                    }
                    resetTileList();
                    return;
                }
                //Checks if the last wire needs to become a corner wire
                PlacingCornerWire(MouseManager.gridPosition);
                //Checking the rotation of the current wire
                if (MouseManager.gridPosition.x == lastTile.x)
                {
                    rotation = 90;
                }
                else
                {
                    rotation = 0;
                }
                //Place the wire on the current tile
                PlaceWire(GridManager.GetTileIndex(MouseManager.gridPosition), (int)MouseManager.gridPosition.x, (int)MouseManager.gridPosition.y, rotation, StraightWire);
                //Adding the placed tile to the list
                tilesPlaced.Add(MouseManager.gridPosition);
            }
            //If they drag over the goal tile, they lock in the wire
            else if (GridManager.Instance.tileStates[GridManager.GetTileIndex(MouseManager.gridPosition)] == goal && !illegalMoveCheck())
            {
                //Checks if the last wire needs to become a corner wire
                PlacingCornerWire(MouseManager.gridPosition);

                // //Instantiate the PowerParticle
                // gameObject.particle = Instantiate(PowerParticle, particle.transform.position, Quaternion.identity);
                // GameObject particle = Instantiate(PowerParticle, GridManager.CalculatePos(wireX, wireZ), Quaternion.Euler(0, rotation, 0));
                // // Set the particle's parent to the wire's transform
                // particle.transform.SetParent(particle.transform);

                //Can swap out the material based on the building when we add the materials
                switch (GridManager.Instance.tileStates[GridManager.GetTileIndex(startingTile)])
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
                        //If this is reached something is broken
                        selectedMaterial = CompletedConnection;
                        break;
                }
                //Change the colour of the wire to the target material
                //I need to invoke it or the colour change will happen before the last corner wire is placed for some reason
                Invoke("EndWirePlace", 0f);

            }
            //If the player drags over a tile that already has a wire from this list, they can remove everything placed since that tile was placed
            //If it is the source building it removes everything but lets the player keep placing wires
            else if ((tilesPlaced.Contains(MouseManager.gridPosition) || startingTile == MouseManager.gridPosition) && lastTile != MouseManager.gridPosition)
            {
                //While there are tiles left and it hasn't gotten to the tile the player is currently hovering over
                while (tilesPlaced.Count > 0 && tilesPlaced[tilesPlaced.Count - 1] != MouseManager.gridPosition)
                {
                    //Removed the wires from the tile and remove the tile from the list
                    RemoveWire(GridManager.GetTileIndex(tilesPlaced[tilesPlaced.Count - 1]));
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
        else if (!Input.GetMouseButton(0)) //If the player releases the left click
        {
            //If the player releases the left click before reaching the goal tile, remove the wires and refund the building
            foreach (Vector2 tile in tilesPlaced)
            {
                RemoveWire(GridManager.GetTileIndex(tile));
            }
            resetTileList();
        }
    }

    private void EndWirePlace()
    {
        //Change wire texture and power particle.
        //NOTE: This will need to be refactored when we merge chases score system.
        if(GridManager.Instance.tileBonus[GridManager.GetTileIndex(startingTile)])
        {
            foreach (Vector2 tile in tilesPlaced)
            {
                changeColour(GridCreator.tiles[GridManager.GetTileIndex(tile)].transform.GetChild(0).gameObject, CompletedConnection);
            }
        }
        
        //Add the starting spot to the start of the list of placed wires
        tilesPlaced.Insert(0, startingTile);
        //Add the starting tile to the list of connected buildings
        connectedBuildings.Add(startingTile);
        GameManager.Instance.CurrentScore = GameManager.Instance.CalculateTotalScore(GridManager.Instance.tileStates);
        wiresPlaced.Add(new List<Vector2>(tilesPlaced)); //Add the list of placed wires to the list of all placed wires
        buildingTiles.Remove(startingTile); //Remove the starting tile from the list of building tiles wihtout wires
        resetTileList();



        if (TutorialManager.Instance.tutorialActive && TutorialManager.Instance.currentSection == TutorialSections.Wiring )
        {
            Debug.Log("Tutorial Active from WirePlacement");
            if (isTileConnected(GridManager.GetTileIndex(new Vector2(0, 3))))
            {
                Debug.Log("Tutorial Active from WirePlacement and connected");
                TutorialManager.Instance.DeletionSection();
                TutorialManager.Instance.obstacleSectionBuildingsRemaining = 1;
            }
            else
            {
                Debug.Log("Tutorial Active from WirePlacement and not connected");
            }
        }
        else if (TutorialManager.Instance.tutorialActive && TutorialManager.Instance.currentSection == TutorialSections.Obstacles)
        {
            TutorialManager.Instance.obstacleSectionBuildingsRemaining = connectedBuildings.Count;
            Debug.Log("Buildings connected: " + TutorialManager.Instance.obstacleSectionBuildingsRemaining);
            if (TutorialManager.Instance.obstacleSectionBuildingsRemaining == 3)
            {
              
                TutorialManager.Instance.currentSection = TutorialSections.End;
                TutorialManager.Instance.EndSection();
            }
        }
    }
    /// <summary>
    /// This is used for placing a corner wire on the last tile if needed
    /// It is a separate function for code readability and because it is only used in two places
    /// </summary>
    /// <param name="currentTile">The tile that the player is currently hovering over. By default this is grid position</param>
    private void PlacingCornerWire(Vector2 currentTile)
    {
        // if (currentTile == -1)
        // {
        //     currentTile = MouseManager.gridPosition;
        // }
        int rotation = 0;
        //This checks if the player has placed a wire on a different axis as the last two tiles
        if ((lastTile.x != secondLastTile.x || lastTile.x != currentTile.x) &&
            (lastTile.y != secondLastTile.y || lastTile.y != currentTile.y))
        {
            // Determine the direction of movement from the second last tile to the last tile and from the last tile to the current tile
            bool isMovingRightOrUpSecondLastToLast = GridManager.GetTileIndex(secondLastTile) < GridManager.GetTileIndex(lastTile);
            bool isMovingRightOrUpLastToCurrent = GridManager.GetTileIndex(lastTile) < GridManager.GetTileIndex(currentTile);

            // Determine the rotation of the corner wire. I just put different rotations in until all directions worked.
            if (currentTile.x == lastTile.x)
            {
                if (isMovingRightOrUpSecondLastToLast)
                {
                    if (isMovingRightOrUpLastToCurrent)
                    {
                        rotation = 270;
                    }
                    else
                    {
                        rotation = 180;
                    }
                }
                else
                {
                    if (isMovingRightOrUpLastToCurrent)
                    {
                        rotation = 0;
                    }
                    else
                    {
                        rotation = 90;
                    }
                }
            }
            else
            {
                if (isMovingRightOrUpSecondLastToLast)
                {
                    if (isMovingRightOrUpLastToCurrent)
                    {
                        rotation = 90;
                    }
                    else
                    {
                        rotation = 180;
                    }
                }
                else
                {
                    if (isMovingRightOrUpLastToCurrent)
                    {
                        rotation = 0;
                    }
                    else
                    {
                        rotation = 270;
                    }
                }
            }
            lastWire = CornerWire; //Set the last wire to a corner wire
        }
        else
        {
            //If the player is placing a wire straight
            //This is so that when you backtrack over a corner wire it will properly straight if it needs to
            if (lastTile.x == secondLastTile.x)
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
            PlaceWire(GridManager.GetTileIndex(lastTile), (int)lastTile.x, (int)lastTile.y, rotation, lastWire);
        }
    }

    /// <summary>
    ///   Check if the player is trying to make an illegal move
    ///   If the move is diagonal but could be legal with a tile placed in an empty square between them it will do that and return false
    /// </summary>
    /// <returns>Returns true if move is illegal</returns>
    private bool illegalMoveCheck()
    {
        if ((Math.Abs(MouseManager.gridPosition.y - lastTile.y) > 1) ||
        (Math.Abs(MouseManager.gridPosition.x - lastTile.x) > 1)) //If it is a movement of more than one tile
        {
            //CONSIDER: I could probably do the same thing as the diagonal movement for this. Should I?
            return true; //Illegal move
        }
        else if ((Math.Abs(MouseManager.gridPosition.y - lastTile.y) > 0) &&
            (Math.Abs(MouseManager.gridPosition.x - lastTile.x) > 0)) //If a diagonal movement is made
        {
            // Calculate the positions of the two squares in between the diagonal move
            // I asked an AI for the correct calculation to figure out these two squares because it is 1am and my brain hurt trying to figure it out
            Vector2 square1 = MouseManager.gridPosition.y > lastTile.y ? new Vector2(lastTile.x, lastTile.y + 1) : new Vector2(lastTile.x, lastTile.y - 1);
            Vector2 square2 = MouseManager.gridPosition.x > lastTile.x ? new Vector2(lastTile.x + 1, lastTile.y) : new Vector2(lastTile.x - 1, lastTile.y);

            // Check if either of the squares is empty
            if (GridManager.IsTileEmpty(GridManager.GetTileIndex(square1)) || GridManager.IsTileEmpty(GridManager.GetTileIndex(square2)))
            {
                Vector2 emptySquare;
                // Place a wire on the empty square
                if (GridManager.IsTileEmpty(GridManager.GetTileIndex(square1)))
                {
                    emptySquare = square1;
                }
                else
                {
                    emptySquare = square2;
                }
                PlacingCornerWire(emptySquare); //Because there is suddenly a new wire that might be a corner
                PlaceWire(GridManager.GetTileIndex(emptySquare), (int)emptySquare.x, (int)emptySquare.y, 0, StraightWire);

                // Update lastTile, secondLastTile, and tilesPlaced
                secondLastTile = lastTile;
                lastTile = emptySquare;
                tilesPlaced.Add(emptySquare);

                return false; //Legal move with the tile placed in the empty square
            }
            else
            {
                return true; //Illegal move
            }
        }
        else
        {
            return false; //Legal move
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
        startingTile = new Vector2(-1, -1);
        lastTile = new Vector2(-1, -1);
        secondLastTile = new Vector2(-1, -1);
    }

    /// <summary>
    ///     Place a wire on the inputted tile
    /// </summary>
    /// <param name="position"> Position of the tile to place the wire on </param>
    /// <param name="wireX"> X position of the wire </param>
    /// <param name="wireZ"> Z position of the wire </param>
    /// <param name="rotation"> Rotation of the wire </param>
    /// <param name="wireType"> Type of wire to place </param>
    /// <param name="powerParticle"> Type of particle to place </param>
    private void PlaceWire(int position, int wireX, int wireZ, int rotation, GameObject wireType)
    {
        //Clear any wire that already exists on the tile
        RemoveWire(position);
        //Instantiate the wire at the position of the tile
        GameObject temp = Instantiate(wireType, GridManager.CalculatePos(wireX, wireZ), Quaternion.Euler(0, rotation, 0));
        //set the parent of the wire to the tile
        temp.transform.SetParent(GridCreator.tiles[position].transform);
        GridManager.Instance.tileStates[position] = TileTypes.Wires;
        GridManager.SetTileState(GridManager.GetTilePosition(position), TileTypes.Wires);
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
            // GridManager.Instance.tileStates[position] = TileTypes.None;
            GridManager.SetTileState(GridManager.GetTilePosition(position), TileTypes.None);
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
            /*
            try
            {
                child.gameObject.GetComponent<MeshRenderer>().material = targetMaterial;
            }
            catch (System.Exception)
            {
                child.gameObject.SetActive(true);
            }*/

            if(child.gameObject.activeSelf == false)
            {
                child.gameObject.SetActive(true);
            }

        }
    }

    /// <summary>
    /// Deletes a full wire when given a building tile starting location
    /// </summary>
    /// <param name="buildingTile">Building that is being deleted</param>
    public void RemoveFullWire(Vector2 buildingTile)
    {
        //Remove the building from the list of connected buildings
        connectedBuildings.Remove(buildingTile);
        buildingTiles.Remove(buildingTile);
        foreach (List<Vector2> wire in wiresPlaced)
        {
            if (buildingTile == wire[0]) //If the first tile is the same as the inputted building tile
            {
                foreach (Vector2 tile in wire)
                {
                    RemoveWire(GridManager.GetTileIndex(tile));
                }
                wiresPlaced.Remove(wire);
                return;
            }
        }
    }
}
