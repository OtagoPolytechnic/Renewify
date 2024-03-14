using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public int gridSize = 5; //width and height of grid (5x5, 9x9, etc)
    public float tileSize = 10.0f; //size each tile, shouldn't have a reason not to be 10
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
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
}
