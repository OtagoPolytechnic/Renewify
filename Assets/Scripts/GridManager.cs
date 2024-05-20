using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{

    [Serializable]
    public struct TileInfo
    {
        public TileTypes building;
        [Range(0, 10)]
        public int x;
        [Range(0, 10)]
        public int y;
        public TileInfo[] adjacent;
        public TileInfo[] diagonals;


        [HideInInspector] public Vector2 position;

        public TileInfo(TileTypes building, int x, int y,bool central = false)
        {
            this.building = building;
            this.x = x;
            this.y = y;
            position = new Vector2(x, y);
                if(central)
                {

                adjacent = new TileInfo[4] {
                    new TileInfo(this.building,x+1,y), 
                    new TileInfo(this.building,x-1,y), 
                    new TileInfo(this.building,x,y+1), 
                    new TileInfo(this.building,x,y-1)
                    
                    };
                diagonals = new TileInfo[4] {

                    new TileInfo(this.building,x+1,y+1), 
                    new TileInfo(this.building,x-1,y+1), 
                    new TileInfo(this.building,x+1,y-1), 
                    new TileInfo(this.building,x-1,y-1)
                    
                }; 
            }
            else
            {
                adjacent = null;
                diagonals = null;
            }

        }
            public TileInfo(TileTypes building, int x, int y)
        {
            this.building = building;
            this.x = x;
            this.y = y;
            position = new Vector2(x, y);
            adjacent = null;
            diagonals = null;

        }
        
    }
    public List<TileInfo> test;
    public static GridManager Instance;
    public int gridSize = 5; //width and height of grid (5x5, 9x9, etc)
    public float tileSize = 10.0f; //size each tile, shouldn't have a reason not to be 10

    public List<TileTypes> tileStates = new List<TileTypes>(); 
    public List<bool> tileBonus = new List<bool>(); 
    //public Dictionary<TilePoints, int> tileBonus = new Dictionary<TilePoints, int>();

    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        tileStates.Clear();
        for (int i = 0; i < gridSize * gridSize; i++) {
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
        if(!TutorialManager.Instance.tutorialActive)
        {

        tileStates[44] = TileTypes.Goal;
        tileStates[45] = TileTypes.Goal;
        tileStates[54] = TileTypes.Goal;
        tileStates[55] = TileTypes.Goal;
        }
        for (int index = 0; index < test.Count; index++) //Iterate through the list of the struct tileInfo and initialize each
        {
            TileInfo tile = test[index];
            if(tile.building != TileTypes.Windmills || tile.building != TileTypes.SolarPanels)
            {
               tile.building = TileTypes.Windmills; //The Default building type will override invalid types
            }
            tile = new TileInfo(tile.building,tile.x, tile.y,true); //initialize the struct with the info
            test[index] = tile;
            Debug.Log(test[index].position);
            tileBonus[GetTileIndex(test[index].position)] = true;
        }

    }
    
    void Start()
    {


    }
    // Start is called before the first frame update
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