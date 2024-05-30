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
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialSections
{
    Building,
    Wiring,
    Deletion,
    Scoreboard,
    DeletionPart2,
    Obstacles,
    End
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public Tooltip mainTooltip;
    public Material glowMaterial;
    public bool tutorialActive;
    public GameObject bonusTiles;
    public GameObject centralBuilding;
    public GameObject buildings;
    public GameObject deleteButton;
    public GameObject scoreDisplay;
    public Outline outline;
    public TutorialSections currentSection;
    [HideInInspector] public int deleteSectionBuildings;
    [HideInInspector] public int obstacleSectionBuildingsRemaining = 0;

    [SerializeField]
    private GameObject flowersPrefab;
    [SerializeField]
    private GameObject rocksPrefab;
    [SerializeField]
    private GameObject exitBTN;
    [SerializeField]
    

    private Dictionary<TutorialSections, string> narrativeTexts = new Dictionary<TutorialSections, string>()
    {
        { TutorialSections.Building, "Let's start by learning how to build. Place buildings on the empty tiles." },
        { TutorialSections.Wiring, "Now, let's connect the buildings to the power source using wiring." },
        { TutorialSections.Scoreboard, "Congratulation you have score a point." },
        { TutorialSections.Deletion, "Next, we'll learn how to delete buildings. Try deleting the windmill and solar panel." },
        { TutorialSections.DeletionPart2, "Good job! Let's practice deletion once more with some additional buildings." },
        { TutorialSections.Obstacles, "In this section, you'll encounter obstacles. Navigate around them to reach your goal." },
        { TutorialSections.End, "Congratulations! You've completed the tutorial. You are now ready to play the game." }
    };

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
        if (tutorialActive)
        {
            exitBTN.SetActive(false);
            outline = mainTooltip.GetComponent<Outline>();

            GameObject.Find("DeleteMode").GetComponent<Toggle>().interactable = false;
            mainTooltip.SetTitle("Welcome to the tutorial!");
            currentSection = TutorialSections.Building;
            for (int i = 0; i < GridManager.Instance.tileStates.Count; i++)
            {
                GridManager.Instance.tileStates[i] = TileTypes.Rocks;
            }
            GridManager.Instance.tileStates[GridManager.GetTileIndex(new Vector2(3, 9))] =
                TileTypes.Goal; //Tutorial goal
            GridManager.Instance.tileStates[GridManager.GetTileIndex(new Vector2(0, 3))] =
                TileTypes.None; //Tutorial Building location
            GridManager.Instance.tileStates[GridManager.GetTileIndex(new Vector2(9, 6))] =
                TileTypes.None; //Tutorial Building location
           GridManager.Instance.tileStates[GridManager.GetTileIndex(new Vector2(0, 6))] =
                TileTypes.None; //Tutorial Building location

            List<Vector2> flowers =
                new()
                {
                new Vector2(0, 2),
                new Vector2(0, 4),
                new Vector2(0, 5),
                new Vector2(0, 7),
                new Vector2(1, 2),
                new Vector2(2, 4),
                new Vector2(2, 8)
                };
            foreach (Vector2 location in flowers)
            {
                GridManager.Instance.tileStates[GridManager.GetTileIndex(location)] = TileTypes.Plants;
                GameObject temp = Instantiate(
                    flowersPrefab,
                    GridManager.CalculatePos(location.x, location.y),
                    Quaternion.identity
                );
            }

            List<Vector2> rocks = new()
        {
            new Vector2(6,8),
            new Vector2(6,9),
            new Vector2(7,0),
            new Vector2(7,1),
            new Vector2(7,8),
            new Vector2(7,9),
            new Vector2(8,0),
            new Vector2(8,1),
            new Vector2(8,8),
            new Vector2(8,9),
        };
            foreach (Vector2 location in rocks)
            {
                GridManager.Instance.tileStates[GridManager.GetTileIndex(location)] = TileTypes.Rocks;
                GameObject temp = Instantiate(rocksPrefab,
                    GridManager.CalculatePos(location.x, location.y),
                    Quaternion.identity
                );
            }
            
            DisplayNarrativeText(TutorialSections.Building);
        }
    }


    /// <summary>
    /// Toggles the outline visibility
    /// </summary>
    private void ToggleOutlineOn()
    {
        outline.enabled = true;
    }

    private void ToggleOutlineOff()
    {
        outline.enabled = false;
    }


    /// <summary>
    /// Displays the narrative text for a given tutorial section
    /// </summary>
    private void DisplayNarrativeText(TutorialSections section)
    {
        if (narrativeTexts.TryGetValue(section, out string narrative))
        {
            mainTooltip.SetContent(narrative);
            
        }
    }


    /// <summary>
    /// Starts the Wiring Section of the Tutorial, highlighting the path to the power source
    /// </summary>
    public void WiringSection()
    {
        currentSection = TutorialSections.Wiring;
        DisplayNarrativeText(currentSection);
        List<Vector2> locations =
            new()
            {
                new Vector2(1, 3),
                new Vector2(2, 3),
                new Vector2(3, 3),
                new Vector2(3, 4),
                new Vector2(3, 5),
                new Vector2(3, 6),
                new Vector2(3, 7),
                new Vector2(3, 8)
            };
        //Highlight the tiles leading to the power source
        foreach (Vector2 location in locations)
        {
            GridManager.Instance.tileStates[GridManager.GetTileIndex(location)] = TileTypes.None;
            GameObject tile = GridCreator.tiles[GridManager.GetTileIndex(location)];
            GameObject guide = Instantiate(
                GridCreator.Instance.tilePrefab,
                tile.transform.position,
                Quaternion.identity
            );
            guide.GetComponent<Renderer>().material = glowMaterial;
            guide.GetComponent<MeshCollider>().enabled = false;
            guide.name = "GuideTile";
            guide.tag = "GuideTile";
        }
        
    }

    public void scoreDisplaying()
    {
        currentSection = TutorialSections.Scoreboard;
        DisplayNarrativeText(currentSection);
        scoreDisplay.SetActive(true);
        scoreDisplay.GetComponent<Outline>().enabled = true;
    }

    public void DeletionSection()
    {
        currentSection = TutorialSections.Deletion;
        DisplayNarrativeText(currentSection);
        deleteButton.SetActive(true);
        GameObject[] guides = GameObject.FindGameObjectsWithTag("GuideTile");
        foreach (GameObject guide in guides)
        {
            Destroy(guide);
        }

        Debug.Log("Deletion Section");
        mainTooltip.SetTitle("Deleting Buildings");
        mainTooltip.SetContent(
            "To delete a building, click the delete button in the bottom right corner of the screen. Then click on the building you want to delete. Try deleting the windmill and solar panel that were placed for you."
        );
        //Highlight the tiles two tiles that need to be deleted
        GameObject.Find("DeleteMode").GetComponent<Toggle>().interactable = true;
        for (int i = 0; i < GridManager.Instance.tileStates.Count; i++)
        {
            if (GridManager.Instance.tileStates[i] == TileTypes.Rocks)
            {
                GridManager.Instance.tileStates[i] = TileTypes.None;
            }
        }
        BuildingPlacing.instance.placeBuilding(new Vector2(6, 1), TileTypes.Windmills);
        BuildingPlacing.instance.placeBuilding(new Vector2(7, 3), TileTypes.SolarPanels);
        deleteSectionBuildings = 2;
        List<Vector2> DeleteGuides = new() { new Vector2(6, 1), new Vector2(7, 3) };
        foreach (Vector2 location in DeleteGuides)
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
            guide.tag = "GuideTile";
            guide.transform.parent = tile.transform;
        }
        bonusTiles.SetActive(true);
        bonusTiles.GetComponent<Outline>().enabled = true;
        
        
    }

    public void ObstacleSection()
    {
        currentSection = TutorialSections.Obstacles;
        DisplayNarrativeText(currentSection);
        TutorialManager.Instance.mainTooltip.SetTitle("Obstacles");
        TutorialManager.Instance.mainTooltip.SetContent(
            "Obstacles are rocks, plants and trees that you cannot build on. You must work around them to reach the goal. Try to build around the rocks and trees to reach the goal. You now have 1 windmill and 1 solar panel to use."
        );
    }

    public void EndSection()
    {
        currentSection = TutorialSections.End;
        mainTooltip.SetTitle("You Have Completed The tutorial");
        mainTooltip.SetContent("Well Done, you have connected all the buildings to the goal. You can now play the game.");
        GameObject.Find("DeleteMode").GetComponent<Toggle>().interactable = false;
        exitBTN.SetActive(true);

    }
}
