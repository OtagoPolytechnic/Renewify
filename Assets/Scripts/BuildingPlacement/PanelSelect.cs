using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PanelSelect : MonoBehaviour
{
    public TileTypes panelBuilding;

    //This is set on spawn to the type of building this panel is for
    public GameObject WindmillPrefab;
    public GameObject SolarPanelPrefab;
    public Sprite selectedSprite;
    public Sprite normalSprite;

    public int availableBuildings;

    void Start()
    {
        //Set the info of the panel
        SetInfo();
    }

    void Update()
    {
        //If the selected building isn't the same as this one, disable the selected text
        if (BuildingPlacing.selectedBuilding != panelBuilding)
        {
            gameObject.GetComponent<Image>().sprite = normalSprite;
        }
        //The button will be clickable as long as the available buildings is more than 0
        gameObject.GetComponent<Button>().interactable = availableBuildings > 0;

    }

    public void SetInfo()
    {

        //Set the sprite to the normal sprite
        gameObject.GetComponent<Image>().sprite = normalSprite;

        //The Count text is updated to display the available buildings
        transform.Find("count").GetComponent<TMPro.TextMeshProUGUI>().text = availableBuildings.ToString();

        //This will show an image of the selected item on the panel eventually
        switch (panelBuilding)
        {
            case TileTypes.Windmills:
                //Set child's text to Windmill as a placeholder
                transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Windmill";
                break;
            case TileTypes.SolarPanels:
                //Set child's text to Windmill as a placeholder
                transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Solar Panels";

                break;
            default:
                Debug.LogError("Panel does not have a valid building type");
                //This should never get selected
                break;
        }
    }

    /// <summary>
    /// This is called when the user clicks on the panel to select a building
    /// </summary>
    public void SelectBuilding()
    {
        //Ensures the user cannot place buildings they don't own
        if (availableBuildings > 0)
        {
            //Turn off delete mode
            InventoryManagement.instance.deleteMode.isOn = false;

            //Set the selected building to the building type of this panel
            if (BuildingPlacing.selectedBuilding == panelBuilding)
            {
                BuildingPlacing.selectedBuilding = TileTypes.None;
                InventoryManagement.instance.currentSelectionPanel = null;
            }
            else
            {
                BuildingPlacing.selectedBuilding = panelBuilding;
                //Sets the inventory managers current selection
                InventoryManagement.instance.currentSelectionPanel = this;
            }
        }

        //If the selected building is the same as this one, set the sprite to the selected sprite
        gameObject.GetComponent<UnityEngine.UI.Image>().sprite = BuildingPlacing.selectedBuilding == panelBuilding ? selectedSprite : normalSprite;

    }
}
