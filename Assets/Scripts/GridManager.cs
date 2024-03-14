using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public int gridSize = 5; //width and height of grid (5x5, 9x9, etc)
    public float tileSize = 10.0f; //size each tile, shouldn't have a reason not to be 10

    public List<TileTypes> tileStates = new List<TileTypes>(); 
    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        for (int i = 0; i < gridSize * gridSize; i++) {
            // Add default value to the list
            tileStates.Add(TileTypes.None);
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
}
