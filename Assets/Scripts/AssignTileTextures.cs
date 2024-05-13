using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerType
{
    Wind,
    Solar
}

public class AssignTileTextures : MonoBehaviour
{
    public Material Wind1;
    public Material Wind2;
    public Material Wind3;
    public Material Sun1;
    public Material Sun2;
    public Material Sun3;
    
    public void AssignTextures(Vector2 centerTile, PowerType powerType)
    {
        
    }
}
