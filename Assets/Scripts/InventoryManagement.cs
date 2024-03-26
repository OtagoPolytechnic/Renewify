using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagement : MonoBehaviour
{

    public List<PanelSelect> selectionPanels;
    public PanelSelect currentSelectionPanel;
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

}
