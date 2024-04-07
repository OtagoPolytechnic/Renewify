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
    void Start()
    {
        SpawnBorder();
    }

    Vector3[] spawnPositions = new Vector3[]
    {
        new Vector3(-50, 1, 0),
        new Vector3(50, 1, 0), 
        new Vector3(0, 1, -50),
        new Vector3(0, 1, 50) 
    };

    public void SpawnBorder()
    {
        for (int i = 0; i < 4; i++)
        {
            Quaternion spawnRotation;
            
            if (spawnPositions[i].x != 0)
            {
                spawnRotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                spawnRotation = Quaternion.identity;
            }

            GameObject border = Instantiate(borderPrefab, spawnPositions[i], spawnRotation);
            border.name = "Border_" + i;
            //GridManager.Instance.gridSize;
        }
    }
}
