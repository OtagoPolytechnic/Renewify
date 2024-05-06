using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Levels", order = 1)]
public class LevelData : ScriptableObject
{
    public int levelIndex;
    //public List<TileData> tiles = new List<TileData>();

    //public List<> test = new List<>();

    public List<Vector2> positions = new List<Vector2>();
    public List<TileTypes> tileTypes = new List<TileTypes>();

    /*
    data to load:
    list of spots worth points.
    - type of power
    - manually enter main tile
    - calculate 8 nearby tiles at runtime
    - specify tile position

    */
}
