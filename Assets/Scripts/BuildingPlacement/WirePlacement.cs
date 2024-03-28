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
        //change material of the first component in the wire to "New Material"
        temp.transform.GetChild(0).GetComponent<MeshRenderer>().material = NewMaterial;


        GridManager.Instance.tileStates[MouseManager.gridPosition] = TileTypes.Wires;
    }
}
