using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public int gridSize = 5; //width and height of grid (5x5, 9x9, etc)
    public float tileSize = 10.0f; //size each tile, shouldn't have a reason not to be 10
    public GameObject tilePrefab; 

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for(int x = 0; x < gridSize; x++)
        {
            for(int y = 0; y < gridSize; y++)
            {
                float xPos = (x * tileSize) - (gridSize / 2 * tileSize) + (tileSize / 2);
                float yPos = (y * tileSize) - (gridSize / 2 * tileSize) + (tileSize / 2);

                Vector3 tilePosition = new Vector3(xPos, 1, yPos);
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tile.name = "Tile_" + x + "_" + y;
            
            }
        }
    }
}
