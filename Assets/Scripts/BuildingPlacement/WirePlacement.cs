using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WirePlacement : MonoBehaviour
{
    public GameObject StraightWire;
    public GameObject CornerWire;
    public Material NewMaterial;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BuildingPlacing.WiresPlacing
        && GridManager.IsTileEmpty(MouseManager.gridPosition)
        && MouseManager.isHovering)
        {
            PlaceWire();
        }
    }

    public void PlaceWire()
    {
        int playerX = MouseManager.Instance.playerX;
        int playerZ = MouseManager.Instance.playerZ;

        GameObject temp = Instantiate(StraightWire, GridManager.CalculatePos(playerX, playerZ), Quaternion.identity);
        changeColour(temp, NewMaterial);


        GridManager.Instance.tileStates[MouseManager.gridPosition] = TileTypes.Wires;
    }

    /// <summary>
    ///    Change the colour of the wire to the target material
    /// </summary>
    /// <param name="wire"> Game Object to be changed </param>
    /// <param name="targetMaterial"> Material to change it to </param>
    private void changeColour(GameObject wire, Material targetMaterial)
    {
        //change material of the all components in the wire to the target material
        foreach (Transform child in wire.transform)
        {
            if (child.gameObject.GetComponent<MeshRenderer>() != null)
            {
                child.gameObject.GetComponent<MeshRenderer>().material = targetMaterial;
                break;
            }
        }
    }
}
