using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSelect : MonoBehaviour
{
    public TileTypes panelBuilding;
    //This is set on spawn to the type of building this panel is for
    public GameObject WindmillPrefab;
    public GameObject SolarPanelPrefab;
    public Sprite selectedSprite;
    public Sprite normalSprite;

    void Start()
    {
        //Set the info of the panel
        SetInfo();
    }

    void Update()
    {
        //If the selected building isn't the same as this one, disable the selected text
        if(BuildingPlacing.selectedBuilding != panelBuilding)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = normalSprite;
        }
    }


    public void SetInfo()
    {
        //Disable the selected text
        transform.GetChild(1).gameObject.SetActive(false);
        //Set the sprite to the normal sprite
        gameObject.GetComponent<UnityEngine.UI.Image>().sprite = normalSprite;

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
                Debug.Log("No valid building type selected");
                Debug.Log(panelBuilding);
                //This should never get selected
                break;
        }
    }


    /// <summary>
    /// This is called when the user clicks on the panel to select a building
    /// </summary>
    public void SelectBuilding()
    {
        //Set the selected building to the building type of this panel
        if (BuildingPlacing.selectedBuilding == panelBuilding)
        {
            BuildingPlacing.selectedBuilding = TileTypes.None;
        }
        else
        {
            BuildingPlacing.selectedBuilding = panelBuilding;
        }
        //Enable the selected text
        //This will either become a model attached to the cursor or a better way of showing selection like an effect
        transform.GetChild(1).gameObject.SetActive(!transform.GetChild(1).gameObject.activeSelf);
        //If the selected building is the same as this one, set the sprite to the selected sprite
        if (BuildingPlacing.selectedBuilding == panelBuilding)
        {
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = selectedSprite;
        }
        else
        {
            gameObject.GetComponent<UnityEngine.UI.Image>().sprite = normalSprite;
        }
    }
}
