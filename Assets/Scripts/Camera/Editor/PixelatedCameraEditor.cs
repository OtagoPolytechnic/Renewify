using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Code from:
//https://github.com/whateep/PixelatedCamera-Unity
//https://www.youtube.com/watch?v=L-tNbbov6Bo&t=115s

[CustomEditor(typeof(PixelatedCamera))]
public class PixelatedCameraEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PixelatedCamera pc = (PixelatedCamera)target;

        // When the inspector is drawn (or any values are changed) re-initialize the render texture
        if (DrawDefaultInspector() || pc.CheckScreenResize()) pc.Init();
    }
}