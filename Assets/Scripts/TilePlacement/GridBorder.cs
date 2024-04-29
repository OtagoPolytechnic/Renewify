using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will spawn in and resizing the grid border.
/// 
/// This script should be added to an empty gameobject at position 0,0,0
/// </summary>
public class GridBorder : MonoBehaviour
{
    public GameObject borderPrefab; //assign this in inspector
    public float positionOffset;
    public float oddOffset;
    void Start()
    {
        
        positionOffset = (GridManager.Instance.gridSize * 5); //if grid size is 10, the borders should be at 50
        oddOffset = GridManager.Instance.gridSize % 2 * 5; //moves grid over by 5 if the gridize is an odd number, is 0 if gridsize is even

        Vector3[] spawnPositions = new Vector3[] //array of spawn positions, adjusts based on grid size
        {
            new Vector3(-positionOffset + oddOffset, 1, oddOffset),
            new Vector3(positionOffset + oddOffset, 1, oddOffset), 
            new Vector3(oddOffset, 1, -positionOffset + oddOffset),
            new Vector3(oddOffset, 1, positionOffset + oddOffset) 
        };
        
        SpawnBorder(spawnPositions);
    }

    /// <summary>
    /// Instantiates and resizes border prefabs.
    /// </summary>
    /// <param name="spawnPositions"></param>
    public void SpawnBorder(Vector3[] spawnPositions)
    {
        for (int i = 0; i < 4; i++)
        {
            Quaternion spawnRotation;

            if (spawnPositions[i].x != oddOffset) //rotates the border
            {
                spawnRotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                spawnRotation = Quaternion.identity;
            }

            GameObject border = Instantiate(borderPrefab, spawnPositions[i], spawnRotation);
            border.name = "Border_" + i;
            border.transform.localScale = new Vector3((GridManager.Instance.gridSize + 0.1f), 1f, 0.1f); //adding the 0.1 to will make the edges look squared.
        }
    }
}
