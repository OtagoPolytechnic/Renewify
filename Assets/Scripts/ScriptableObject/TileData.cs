using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public Vector2 position;
    public TileTypes tileType;

    /*
    data to load:
    list of spots worth points.
    - type of power
    - manually enter main tile
    - calculate 8 nearby tiles at runtime
    - specify tile position

    */
    public TileData(Vector2 pos, TileTypes type)
    {
        position = pos;
        tileType = type;
    }
}
