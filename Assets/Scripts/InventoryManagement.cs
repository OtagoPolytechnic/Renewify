using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManagement : MonoBehaviour
{

    [HideInInspector] public List<PanelSelect> selectionPanels;
    [HideInInspector] public PanelSelect currentSelectionPanel;

    [HideInInspector] public Toggle deleteMode;

    public static InventoryManagement instance;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        selectionPanels = new List<PanelSelect>();
        selectionPanels.AddRange(FindObjectsOfType<PanelSelect>());
        deleteMode = GameObject.Find("DeleteMode").GetComponent<Toggle>();
    }
    public void DeleteModeToggle()
    {
        //If Delete mode is turned on
        if (deleteMode.isOn)
        {
            BuildingPlacing.selectedBuilding = TileTypes.None; //Set the selected type to None
            if (currentSelectionPanel) //Null check on selected panel
            {
                currentSelectionPanel.SetInfo(); //Update selected panel info and then set it to null
                currentSelectionPanel = null;
            }
        }

    }

    public bool BuildingsLeft()
    {
        return  currentSelectionPanel.availableBuildings > 0;

    }
    public void PlaceSelectedBuilding()
    {
        currentSelectionPanel.availableBuildings--; //Decrease buildings by 1
        currentSelectionPanel.SetInfo(); //Update the display
    }

    //Finds which panel manages the selected building and restores 1 building to the inventory
    public void ReturnSelectedBuilding(TileTypes buildingType)
    {
        //Iterate through the list until the selected panel is found 
        foreach (PanelSelect ps in selectionPanels)
        {
            if (buildingType == ps.panelBuilding)
            {
                ps.availableBuildings++; //Increase the available buildings by 1
                ps.SetInfo(); //Update the display
                break; 
            }
        }
    }

}
