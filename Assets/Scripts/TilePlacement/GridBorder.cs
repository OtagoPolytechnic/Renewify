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

    public void SpawnBorder()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject border = Instantiate(borderPrefab, new Vector3(0,1,0), Quaternion.identity);
            border.name = "Border_" + i;
            //GridManager.Instance.gridSize;
        }
    }
}
