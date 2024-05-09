/// <remarks>
/// Author: Chase Bennett-Hill
/// Last Edited: 07 / 05 / 2024
/// Known Bugs: None at the moment
/// <remarks>
/// <summary>
/// Manages the tutorial creates tooltips for each section of the tutorial
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public Tooltip mainTooltip;
    public Material glowMaterial;
    public bool tutorialActive;

    // Start is called before the first frame update


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        mainTooltip.SetTitle("Welcome to the Tutorial!");
        for (int i = 0; i < GridManager.Instance.tileStates.Count; i++)
        {
            GridManager.Instance.tileStates[i] = TileTypes.Rocks;
        }
        GridManager.Instance.tileStates[GridManager.GetTileIndex(new Vector2(3, 9))] = TileTypes.Goal; //Tutorial goal
        GridManager.Instance.tileStates[GridManager.GetTileIndex(new Vector2(0, 3))] = TileTypes.None; //Tutorial Building location
        List<Vector2> locations = new() {

        new Vector2(1, 3),
        new Vector2(2, 3),
        new Vector2(3, 3),
        new Vector2(3, 4),
        new Vector2(3, 5),
        new Vector2(3, 6),
        new Vector2(3, 7),
        new Vector2(3, 8)
    };
        foreach (Vector2 location in locations)
        {
            GridManager.Instance.tileStates[GridManager.GetTileIndex(location)] = TileTypes.None;
        }

    }

    /// <summary>
    /// Starts the Wiring Section of the Tutorial, highlighting the path to the power source
    /// </summary>
    public void WiringSection()
    {
        //Highlight the tiles leading to the power source
        List<Vector2> locations = new() {

        new Vector2(1, 3),
        new Vector2(2, 3),
        new Vector2(3, 3),
        new Vector2(3, 4),
        new Vector2(3, 5),
        new Vector2(3, 6),
        new Vector2(3, 7),
        new Vector2(3, 8)
    };
        foreach (Vector2 location in locations)
        {
            GameObject tile = GridCreator.tiles[GridManager.GetTileIndex(location)];
            GameObject guide = Instantiate(
                GridCreator.Instance.tilePrefab,
                tile.transform.position,
                Quaternion.identity
            );
            guide.GetComponent<Renderer>().material = glowMaterial;
            guide.GetComponent<MeshCollider>().enabled = false;
            guide.name = "GuideTile";
            guide.tag = "Untagged";
        }
    }

    public void DeletionSection()
    {
        Debug.Log("Deletion Section");
        BuildingPlacing.instance.placeBuilding(new Vector2(4,1), TileTypes.Windmills);
        BuildingPlacing.instance.placeBuilding(new Vector2(7, 3), TileTypes.SolarPanels);
    }
}
