/// <summary>
/// This script is used to place buildings on the grid
/// </summary>
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingPlacing : MonoBehaviour
{
    [SerializeField]
    private GameObject Windmill;
    [SerializeField]
    private GameObject SolarPanelField;
    [SerializeField]
    private GameObject ghostWindmill;
    [SerializeField]
    private GameObject ghostSolarPanelField;
    [SerializeField]
    private Material ghostTexture;
    [SerializeField]
    private Material ghostTextureRed;
    private bool isRed = false;
    private GameObject ghostBuilding = null;
    public static bool WiresPlacing = false;
    private Vector2 hoveredPos = new Vector2(-1, -1);
    [SerializeField]
    private GameObject redSolarPanel;
    [SerializeField]
    private GameObject redWindmill;
    private GameObject redBuilding = null;
    //Enum building variable
    public static TileTypes selectedBuilding = TileTypes.None;
    public static BuildingPlacing instance;
    //Note: This is just to get it working as I don't have anywhere to attach the event trigger to yet. Will be changed out of update

    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        //If the user clicks the mouse
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
            if (ghostBuilding != null)
            {
                Destroy(ghostBuilding);
            }
        }
        //If the user is hovering over a tile and has selected a building create a ghost building at the mouse position
        else if (selectedBuilding != TileTypes.None && MouseManager.isHovering)
        {
            //If there isnt a ghost building already created then create one
            if (ghostBuilding == null)
            {
                GameObject temp = null;
                //Instantiate a ghost version of the selected building at the mouse position. The ghostWindmill and ghostSolarPanelField are the prefabs of the ghost buildings
                switch (selectedBuilding)
                {
                    case TileTypes.Windmills:
                        //Instantiate the ghost building at the mouse position
                        temp = ghostWindmill;
                        break;
                    case TileTypes.SolarPanels:
                        temp = ghostSolarPanelField;
                        break;
                    default:
                        Debug.Log("No valid building type selected");
                        break;
                }
                if (temp != null)
                {
                    ghostBuilding = Instantiate(temp, GridManager.CalculatePos(MouseManager.gridPosition.x, MouseManager.gridPosition.y), Quaternion.identity);
                    ghostBuilding.transform.Rotate(0, 180, 0);
                }
                isRed = false;
            }
            //If the ghost building exists and is not at the mouse position then move it to the mouse position
            else if (ghostBuilding.transform.position != GridManager.CalculatePos(MouseManager.gridPosition.x, MouseManager.gridPosition.y))
            {
                ghostBuilding.transform.position = GridManager.CalculatePos(MouseManager.gridPosition.x, MouseManager.gridPosition.y);
            }
            //If tile is not empty change the colour of the ghost building to red
            if (!GridManager.IsTileEmpty(GridManager.GetTileIndex(MouseManager.gridPosition)))
            {
                if (!isRed)
                {
                    materialChange(ghostBuilding, ghostTextureRed);
                    isRed = true;
                }
            }
            //Else reset the colour
            else
            {
                if (isRed)
                {
                    materialChange(ghostBuilding, ghostTexture);
                    isRed = false;
                }
            }
        }
        else if (InventoryManagement.instance.deleteMode.isOn)
        {

            //Remove the thing that shows the building would be deleted
            if ((hoveredPos != new Vector2(-1, -1)
            && hoveredPos != MouseManager.gridPosition)
            || !MouseManager.isHovering) //If the mouse is no longer on a playable space
            {
                InventoryManagement.instance.deleteBuildingHover(false);
                if (redBuilding != null)
                {
                    Destroy(redBuilding);
                }
                hoveredPos = new Vector2(-1, -1);
            }
            if (MouseManager.isHovering
            && hoveredPos != MouseManager.gridPosition
                && (GridManager.Instance.tileStates[GridManager.GetTileIndex(MouseManager.gridPosition)] == TileTypes.Windmills || GridManager.Instance.tileStates[GridManager.GetTileIndex(MouseManager.gridPosition)] == TileTypes.SolarPanels))
            {
                InventoryManagement.instance.deleteBuildingHover(true);
                switch (GridManager.Instance.tileStates[GridManager.GetTileIndex(MouseManager.gridPosition)])
                {
                    case TileTypes.Windmills:
                        redBuilding = redWindmill;
                        break;
                    case TileTypes.SolarPanels:
                        redBuilding = redSolarPanel;
                        break;
                    default:
                        Debug.Log("No valid building type selected");
                        break;
                }
                if (redBuilding != null)
                {
                    //Instantiate the ghost building at the mouse position
                    redBuilding = Instantiate(redBuilding, GridManager.CalculatePos(MouseManager.gridPosition.x, MouseManager.gridPosition.y), Quaternion.identity);
                    redBuilding.transform.Rotate(0, 180, 0);
                }
                hoveredPos = MouseManager.gridPosition;
            }
        }
    }

    /// <summary>
    /// Will recursively change the material of all renderers in all children of the parent object
    /// </summary>
    /// <param name="parent">Parent object</param>
    /// <param name="mat">Material to change it to</param>
    private void materialChange(GameObject parent, Material mat)
    {
        Renderer renderer = parent.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = mat;
        }

        foreach (Transform child in parent.transform)
        {
            materialChange(child.gameObject, mat);
        }
    }


    public void OnMouseDown()
    {
        if (selectedBuilding != TileTypes.None && MouseManager.isHovering && !InventoryManagement.instance.deleteMode.isOn)
        {
            if (TutorialManager.Instance.currentSection == TutorialSections.Building && TutorialManager.Instance.tutorialActive)
            {
                if (GridManager.GetTileIndex(MouseManager.gridPosition) == GridManager.GetTileIndex(new Vector2(0, 3)))
                {

                    placeBuilding();
                    GameObject tile = GridCreator.tiles[GridManager.GetTileIndex(new Vector2(0, 3))];
                    Destroy(tile.transform.Find("GuideTile").gameObject);
                    TutorialManager.Instance.mainTooltip.SetTitle("Wiring Buildings");
                    TutorialManager.Instance.mainTooltip.SetContent("Now that you have placed a building, you need to connect it to the power source. Click on the building and drag to the power source to connect them.");
                    TutorialManager.Instance.WiringSection();
                }
            }
            else
            {
                placeBuilding();
            }


        }
        //If No Building is selected and the player clicks the tile then the building returns to the inventory and the game-object is destroyed and the tile-state returns to none
        else if (MouseManager.isHovering &&
                (GridManager.Instance.tileStates[GridManager.GetTileIndex(MouseManager.gridPosition)] == TileTypes.Windmills || GridManager.Instance.tileStates[GridManager.GetTileIndex(MouseManager.gridPosition)] == TileTypes.SolarPanels) &&
                InventoryManagement.instance.deleteMode.isOn)
        {
            
            if (TutorialManager.Instance.tutorialActive && (TutorialManager.Instance.currentSection == TutorialSections.Deletion || TutorialManager.Instance.currentSection == TutorialSections.DeletionPart2 || TutorialManager.Instance.currentSection == TutorialSections.Obstacles || TutorialManager.Instance.currentSection == TutorialSections.End))
            {
                if (GridManager.GetTileIndex(MouseManager.gridPosition) == GridManager.GetTileIndex(new Vector2(0, 3)) || TutorialManager.Instance.currentSection == TutorialSections.End)
                    return;
                
                    if (TutorialManager.Instance.currentSection != TutorialSections.Obstacles && TutorialManager.Instance.currentSection != TutorialSections.End)
                         Destroy(GetTileObject(GridManager.GetTileIndex(MouseManager.gridPosition)).transform.Find("GuideTile").gameObject);
                
                    if(TutorialManager.Instance.deleteSectionBuildings  > 0)
                    {
                        TutorialManager.Instance.deleteSectionBuildings--;
                    }
                    if(TutorialManager.Instance.deleteSectionBuildings == 0)
                    {
                        TutorialManager.Instance.mainTooltip.SetContent("Click on the delete button again to turn off delete mode.");
                        TutorialManager.Instance.currentSection = TutorialSections.DeletionPart2;
                    }
                    if (TutorialManager.Instance.currentSection == TutorialSections.Obstacles)
                    {
                        TutorialManager.Instance.obstacleSectionBuildingsRemaining = WirePlacement.Instance.ConnectedBuildings.Count;
                    }
                
            }
            InventoryManagement.instance.ReturnSelectedBuilding(GridManager.Instance.tileStates[GridManager.GetTileIndex(MouseManager.gridPosition)]);
            //GridManager.Instance.tileStates[GetTileIndex(MouseManager.gridPosition)] = TileTypes.None;
            GridManager.SetTileState(MouseManager.gridPosition, TileTypes.None);
            Destroy(GetTileObject(GridManager.GetTileIndex(MouseManager.gridPosition)).transform.GetChild(0).gameObject);
            WirePlacement.Instance.RemoveFullWire(MouseManager.gridPosition);
            hoveredPos = new Vector2(-1, -1);
            if (redBuilding != null)
            {
                Destroy(redBuilding);
            }
            InventoryManagement.instance.deleteBuildingHover(false);
        }

    }

    /// <summary>
    /// Returns the gameobject of the tile
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject GetTileObject(int index)
    {
        return GridCreator.tiles[index];
    }

    /// <summary>
    /// Places a building where the user clicks    
    /// </summary>
    private void placeBuilding()
    {
        int playerX = (int)MouseManager.gridPosition.x;
        int playerZ = (int)MouseManager.gridPosition.y;
        if (GridManager.IsTileEmpty(GridManager.GetTileIndex(MouseManager.gridPosition)) && InventoryManagement.instance.BuildingsLeft())
        {
            //Pass through the building I want to be placed
            GridManager.SetTileState(MouseManager.gridPosition, selectedBuilding);
            //Remove a building from the inventory
            InventoryManagement.instance.PlaceSelectedBuilding();
            //Place the building
            //Get the prefab with the same name as the building variable
            switch (selectedBuilding)
            {
                case TileTypes.Windmills:
                    spawnBuilding(Windmill, playerX, playerZ, GetTileObject(GridManager.GetTileIndex(MouseManager.gridPosition)));
                    WiresPlacing = true;
                    break;
                case TileTypes.SolarPanels:
                    spawnBuilding(SolarPanelField, playerX, playerZ, GetTileObject(GridManager.GetTileIndex(MouseManager.gridPosition)));
                    WiresPlacing = true;
                    break;
                default:
                    break;
            }
            selectedBuilding = TileTypes.None;
        }

    }

    /// <summary>
    /// Places a building at the specified tile
    /// </summary>
    /// <param name="tile">The Tile the building is spawned to</param>
    /// <param name="building">The building to be spawned</param>
    public void placeBuilding(Vector2 tile, TileTypes building)
    {

        Debug.Log("Placing Building");
        //Pass through the building I want to be placed
        GridManager.SetTileState(tile, building);
        //Remove a building from the inventory
        //Place the building
        //Get the prefab with the same name as the building variable
        switch (building)
        {
            case TileTypes.Windmills:
                spawnBuilding(Windmill, (int)tile.x, (int)tile.y, GetTileObject(GridManager.GetTileIndex(tile)));
                break;
            case TileTypes.SolarPanels:
                spawnBuilding(SolarPanelField, (int)tile.x, (int)tile.y, GetTileObject(GridManager.GetTileIndex(tile)));
                break;
            default:
                break;
        }


    }
    private void spawnBuilding(GameObject building, int playerX, int playerZ, GameObject parent)
    {
        GameObject temp = Instantiate(building, GridManager.CalculatePos(playerX, playerZ), Quaternion.identity);
        //Rotate temp by 180 degrees as the model is facing the wrong way no matter how I rotate the prefab?
        temp.transform.Rotate(0, 180, 0);

        temp.transform.parent = parent.transform; //Set the building to be a child of the tile
    }
}
