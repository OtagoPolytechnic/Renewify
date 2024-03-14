using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public static GridCreator Instance;
    public GameObject tilePrefab; 

    public Material defaultMaterial;
    public Material highlightedMaterial;

    public static List<GameObject> tiles = new List<GameObject>();
    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for(int x = 0; x < GridManager.Instance.gridSize; x++)
        {
            for(int z = 0; z < GridManager.Instance.gridSize; z++)
            {
                Vector3 tilePosition = GridManager.CalculatePos(x,z);
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tile.name = "Tile_" + x + "_" + z;

                tile.GetComponent<Renderer>().material = defaultMaterial;

                tiles.Add(tile);
            }
        }
    }

}
