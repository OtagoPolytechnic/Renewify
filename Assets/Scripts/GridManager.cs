using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{

    [Serializable]
    public struct TileInfo
    {
       [HideInInspector] public TileTypes building;
        public TilePoints type;
        [Range(0, 9)]
        public int x;
        [Range(0, 9)]
        public int y;
        [HideInInspector] public List<TileInfo> adjacent;
        [HideInInspector] public List<TileInfo> diagonals;


        [HideInInspector] public Vector2 position;

        //Constructor will be used for the central bonus tiles
        public TileInfo(TilePoints type, int x, int y, bool central)
        {
            this.type = type;
            switch (type)
            {
                case TilePoints.Solar:
                    building = TileTypes.SolarPanels;
                    break;
                case TilePoints.Wind:
                    building = TileTypes.Windmills;
                    break;
                default:
                    building = TileTypes.None;
                    break;
            }
            this.x = x;
            this.y = y;
            position = new Vector2(x, y);
            if (central)
            {

                List<TileInfo> tempAdjacent = new List<TileInfo> {
                    new (this.type,this.building,x+1,y),
                    new (this.type,this.building,x-1,y),
                    new (this.type,this.building,x,y+1),
                    new (this.type,this.building,x,y-1)

                    };
                adjacent = new List<TileInfo>();
                foreach (var adj in tempAdjacent)
                {
                    if (GridCreator.Instance.ValidCheck(adj.position))
                    {
                        adjacent.Add(adj);
                    }
                }

                List<TileInfo> tempDiagonals = new List<TileInfo> {

                    new (this.type,this.building,x+1,y+1),
                    new (this.type,this.building,x-1,y+1),
                    new (this.type,this.building,x+1,y-1),
                    new (this.type,this.building,x-1,y-1)

                };
                diagonals = new List<TileInfo>();
                foreach (var diag in tempDiagonals)
                {
                    if (GridCreator.Instance.ValidCheck(diag.position))
                    {
                        diagonals.Add(diag);
                    }
                }

            }
            else
            {
                adjacent = null;
                diagonals = null;
            }

        }
        public TileInfo(TilePoints type, TileTypes building, int x, int y) //Constructor for the adjacent and diagonal tiles
        {
            this.type = type;
            this.building = building;
            this.x = x;
            this.y = y;
            position = new Vector2(x, y);
            adjacent = null;
            diagonals = null;


        }

    }
    public List<TileInfo> scoreTiles;
    public static GridManager Instance;
    public int gridSize = 5; //width and height of grid (5x5, 9x9, etc)
    public float tileSize = 10.0f; //size each tile, shouldn't have a reason not to be 10

    public List<TileTypes> tileStates = new List<TileTypes>();
    //public Dictionary<TilePoints, int> tileBonus = new Dictionary<TilePoints, int>();

    public List<Vector2> GetGoalTiles()
    {
        List<Vector2> goalTiles = new List<Vector2>();
        for (int i = 0; i < tileStates.Count; i++)
        {
            if (tileStates[i] == TileTypes.Goal)
            {
                goalTiles.Add(GetTilePosition(i));
            }
        }
        return goalTiles;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        tileStates.Clear();
        for (int i = 0; i < gridSize * gridSize; i++)
        {
            // Add default value to the list
            tileStates.Add(TileTypes.None);
        }

        //TODO: load new tileStates preset

        //THIS IS TEMPORARY CODE TO HARDCODE GOALS AND OBSTACLES FOR A DEMONSTRATION.
        tileStates[2] = TileTypes.Plants;
        tileStates[12] = TileTypes.Plants;
        tileStates[69] = TileTypes.Rocks;
        tileStates[70] = TileTypes.Rocks;
        tileStates[93] = TileTypes.Rocks;
        tileStates[86] = TileTypes.Trees;
        if (!TutorialManager.Instance.tutorialActive)
        {

            tileStates[44] = TileTypes.Goal;
            tileStates[45] = TileTypes.Goal;
            tileStates[54] = TileTypes.Goal;
            tileStates[55] = TileTypes.Goal;
        }
        List<Vector2> goalTiles = GetGoalTiles();
        Debug.Log("Expected Slots: " + GameManager.Instance.CalculateOpenSlots(goalTiles));


        for (int index = 0; index < scoreTiles.Count; index++) //Iterate through the list of the struct tileInfo and initialize each
        {
            TileInfo tile = scoreTiles[index];
            if (tile.building != TileTypes.Windmills && tile.building != TileTypes.SolarPanels)
            {
                tile.building = TileTypes.Windmills; //The Default building type will override invalid types
            }
            tile = new TileInfo(tile.type, tile.x, tile.y, true); //initialize the struct with the info
            scoreTiles[index] = tile;
        }

    }

    private Vector3 onCalculatePos(float x, float z)
    {
        Vector3 position;
        float xPos = (x * tileSize) - (gridSize / 2 * tileSize) + (tileSize / 2);
        float zPos = (z * tileSize) - (gridSize / 2 * tileSize) + (tileSize / 2);
        return position = new Vector3(xPos, 1, zPos);
    }

    // Update is called once per frame
    public static Vector3 CalculatePos(float x, float z)
    {
        return Instance.onCalculatePos(x, z);
    }



    private bool OnIsTileEmpty(int index)
    {
        return tileStates[index] == TileTypes.None;
    }

    public static bool IsTileEmpty(int index)
    {
        return Instance.OnIsTileEmpty(index);
    }


    public static int GetTileIndex(Vector2 gridPosition)
    {
        return (int)(gridPosition.x * GridManager.Instance.gridSize + gridPosition.y);
    }

    public static Vector2 GetTilePosition(int index)
    {
        return new Vector2(index / GridManager.Instance.gridSize, index % GridManager.Instance.gridSize);

    }
    public static void SetTileState(Vector2 tilePos, TileTypes tileType)
    {
        Instance.tileStates[GetTileIndex(tilePos)] = tileType;
    }
}