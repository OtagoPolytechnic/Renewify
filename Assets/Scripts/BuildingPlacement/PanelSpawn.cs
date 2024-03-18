using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSpawn : MonoBehaviour
{
    public GameObject PanelPrefab;
    public GameObject BackgroundPanel;
    // Start is called before the first frame update
    void Start()
    {
        //Find how many of each building it needs
        //Spawn the building selection panels
        //Spawning one to start with
        // SpawnBuildingPanel();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="building">Building panels to spawn</param> THESE WILL BE ADDED LATER
    /// <param name="amount">Amount of panels to spawn</param>
    // private void SpawnBuildingPanel()
    // {
    //     //Hardcoding this to windmills for testing. Will be changed to an input parameter
    //     TileTypes building = TileTypes.Windmills;
    //     //Spawn the building selection panels
    //     //Instantiate a panel prefab
    //     GameObject temp = Instantiate(PanelPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    //     //Set the parent of the panel to the background panel
    //     temp.transform.SetParent(BackgroundPanel.transform);
    //     //Set the BuildingType panelBuilding to the input building
    //     temp.GetComponent<PanelSelect>().panelBuilding = building;
    //     //Set the name of the panel to the building type
    //     temp.name = building.ToString();
    //     //Set the position of the panel. Need to figure out what position I want this to be
    //     temp.transform.position = new Vector3(0, 0, 0);
    //     //Set the text of the panel to the building type
    //     temp.GetComponent<PanelSelect>().SetInfo();
    // }
}
