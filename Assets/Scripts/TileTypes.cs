using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileTypes
{
    None,
    SolarPanels,
    Windmills,
    Trees,
    Rocks,
    Plants,
    Wires, //This is for both straight and corner wires as outside of the placement script there is no need to know the difference
    Goal
}
