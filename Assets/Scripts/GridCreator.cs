using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public static GridCreator Instance;
    public GameObject tilePrefab; 

    public Material defaultMaterial;
    public Material highlightedMaterial;
    public Material debugMaterial;

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
        for(int i = 0; i < GridManager.Instance.tileBonus.Count; i++)
        {
            
            if(GridManager.Instance.tileBonus[i] == true)
            {
                DebugTileScore(i);
            }
        }
    }

    void GenerateGrid()
    {
        tiles.Clear(); //Previously the tile list would increase when exiting and reloading the scene
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

    public void DebugTileScore(int index)
    {
        Vector3 tilePosition = tiles[index].transform.position;
        GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
        tile.name = "Bonus_Tile_" + index;
        tile.tag = "Untagged";
        tile.GetComponent<MeshCollider>().enabled = false;

        tile.GetComponent<Renderer>().material = debugMaterial;
    }

}
