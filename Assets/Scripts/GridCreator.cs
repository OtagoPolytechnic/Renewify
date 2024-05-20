using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public static GridCreator Instance;
    public GameObject tilePrefab; 

    public Material defaultMaterial;
    public Material highlightedMaterial;
    public Material debugMaterial;


    public static List<GameObject> tiles = new List<GameObject>();
    public Material Wind1;
    public Material Wind2;
    public Material Wind3;
    public Material Sun1;
    public Material Sun2;
    public Material Sun3;

    
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
        foreach(var tile in GridManager.Instance.test)
        {
            if(tile.building == TileTypes.Windmills)
            {
                AddBonusTile(tile.position, true);
            }
            else if(tile.building == TileTypes.SolarPanels)
            {
                AddBonusTile(tile.position, false);
            }
            else {Debug.LogError("Tile type not recognized");}


        }
        // for(int i = 0; i < GridManager.Instance.tileBonus.Count; i++)
        // {
            
        //     if(GridManager.Instance.tileBonus[i] == true)
        //     {
        //         AddBonusTile(GridManager.GetTilePosition(i), false);
        //     }
        // }
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
    private void AddBonusTile(Vector2 centerTile, bool wind)
    {
        int index = GridManager.GetTileIndex(centerTile);
        List<Vector2> adjTiles = new List<Vector2>()
        {
            new Vector2(centerTile.x + 1, centerTile.y),
            new Vector2(centerTile.x - 1, centerTile.y),
            new Vector2(centerTile.x, centerTile.y + 1),
            new Vector2(centerTile.x, centerTile.y - 1)
        };
        List<Vector2> diagTiles = new List<Vector2>()
        {
            new Vector2(centerTile.x + 1, centerTile.y + 1),
            new Vector2(centerTile.x - 1, centerTile.y - 1),
            new Vector2(centerTile.x + 1, centerTile.y - 1),
            new Vector2(centerTile.x - 1, centerTile.y + 1)
        };
        if(wind)
        {
            if(ValidCheck(centerTile))
            {
                BonusTileCreate(index, Wind1);
            }
            foreach(Vector2 v in adjTiles)
            {
                if(ValidCheck(v))
                {
                    BonusTileCreate(GridManager.GetTileIndex(v), Wind2);
                }
            }
            foreach(Vector2 v in diagTiles)
            {
                if(ValidCheck(v))
                {
                    BonusTileCreate(GridManager.GetTileIndex(v), Wind3);
                }
            }
        }
        else
        {
            if(ValidCheck(centerTile))
            {
                BonusTileCreate(index, Sun1);
            }
            foreach(Vector2 v in adjTiles)
            {
                if(ValidCheck(v))
                {
                    BonusTileCreate(GridManager.GetTileIndex(v), Sun2);
                }
            }
            foreach(Vector2 v in diagTiles)
            {
                if(ValidCheck(v))
                {
                    BonusTileCreate(GridManager.GetTileIndex(v), Sun3);
                }
            }
        }
    }

    private void BonusTileCreate(int index, Material material)
    {
        Vector3 tilePosition = tiles[index].transform.position;
        GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
        tile.name = "Bonus_Tile_" + index;
        tile.tag = "Untagged";
        tile.GetComponent<MeshCollider>().enabled = false;
        tile.GetComponent<Renderer>().material = material;
    }

    private bool ValidCheck(Vector2 v)
    {
        if(v.x < 0 || v.x >= GridManager.Instance.gridSize || v.y < 0 || v.y >= GridManager.Instance.gridSize || !GridManager.IsTileEmpty(GridManager.GetTileIndex(v)))
        {
            return false; //This is to prevent the game from looping the tiles around the map or placing underneath an obstacle
        }
        return true;
    }
}
