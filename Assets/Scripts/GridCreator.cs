using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    
    public GameObject tilePrefab; 

    public Material defaultMaterial;
    public Material highlightedMaterial;

    public int playerX;
    public int playerY; 

    private List<GameObject> tiles = new List<GameObject>();
    

    void Start()
    {
        GenerateGrid();
    }

    void Update()
    {
        CheckMouseHover();
    }

    void GenerateGrid()
    {
        for(int x = 0; x < GridManager.Instance.gridSize; x++)
        {
            for(int y = 0; y < GridManager.Instance.gridSize; y++)
            {
                Vector3 tilePosition = GridManager.CalculatePos(x,y);
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tile.name = "Tile_" + x + "_" + y;

                tile.GetComponent<Renderer>().material = defaultMaterial;

                tiles.Add(tile);
            }
        }
    }

    void CheckMouseHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hoveredTile = hit.collider.gameObject;
            //All grid planes are given the "Tile" tag, checks if the player is hovering over one
            if (hoveredTile.CompareTag("Tile"))
            {
                //The tile gameobject names are automatically set   
                //grabs only the x and y position from the name
                string[] tileNameParts = hoveredTile.name.Split('_');
                playerX = int.Parse(tileNameParts[1]);
                playerY = int.Parse(tileNameParts[2]);

                SetTileAtPlayerPosition();


            }
        }
    }

    void SetTileAtPlayerPosition()
    {
        //sets all tiles to unselected
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Renderer>().material = defaultMaterial;
        }

        //Checks if player's x,y grid position is within bounds
        if (playerX >= 0 && playerX < GridManager.Instance.gridSize && playerY >= 0 && playerY < GridManager.Instance.gridSize)
        {
            int index = playerX * GridManager.Instance.gridSize + playerY;

            //Change tile material to highlighted
            tiles[index].GetComponent<Renderer>().material = highlightedMaterial;
        }
    }
}
