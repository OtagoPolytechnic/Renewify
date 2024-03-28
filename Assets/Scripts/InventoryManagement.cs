using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManagement : MonoBehaviour
{

    public List<PanelSelect> selectionPanels;
    public PanelSelect currentSelectionPanel;
    public static InventoryManagement instance;
    public Toggle deleteMode;

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

    public bool BuildingsLeft()
    {
        if (currentSelectionPanel.availableBuildings > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void PlaceSelectedBuilding()
    {
        currentSelectionPanel.availableBuildings--;
        currentSelectionPanel.SetInfo();
    }

    //Finds which panel manages the selected building and restores 1 building to the inventory
    public void ReturnSelectedBuilding(TileTypes buildingType)
    {
        foreach(PanelSelect ps in selectionPanels)
        {
            if (buildingType == ps.panelBuilding)
            {
                ps.availableBuildings++;
                ps.SetInfo();
                break;
            }
        }
    }

}
