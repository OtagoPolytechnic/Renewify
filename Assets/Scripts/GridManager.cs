using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
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

        switch(SceneManager.GetActiveScene().name)
        {
            case "Level1":
                tileStates[37] = TileTypes.Goal;
                tileStates[38] = TileTypes.Goal;
                break;
            case "Level2":
                break;
            case "Main Scene":
                tileStates[2] = TileTypes.Plants;
                tileStates[12] = TileTypes.Plants;
                tileStates[69] = TileTypes.Rocks;
                tileStates[70] = TileTypes.Rocks;
                tileStates[93] = TileTypes.Rocks;
                tileStates[86] = TileTypes.Trees;
                tileStates[44] = TileTypes.Goal;
                tileStates[45] = TileTypes.Goal;
                tileStates[54] = TileTypes.Goal;
                tileStates[55] = TileTypes.Goal;
                break;
            case "Tutorial":
                tileStates[2] = TileTypes.Plants;
                tileStates[12] = TileTypes.Plants;
                tileStates[69] = TileTypes.Rocks;
                tileStates[70] = TileTypes.Rocks;
                tileStates[93] = TileTypes.Rocks;
                tileStates[86] = TileTypes.Trees;
                break;
        }

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