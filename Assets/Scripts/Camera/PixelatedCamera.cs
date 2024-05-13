using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Code from:
//https://github.com/whateep/PixelatedCamera-Unity
//https://www.youtube.com/watch?v=L-tNbbov6Bo&t=115s

public class PixelatedCamera : MonoBehaviour
{
    [System.Serializable]
    public struct ScreenSize
    {
        // Basically an integer Vector2 to store screen size information
        public int width;
        public int height;
    }

    public static PixelatedCamera main;

    private Camera renderCamera;
    private RenderTexture renderTexture;
    private int screenWidth, screenHeight;

    [Header("Screen scaling settings")]
    public ScreenSize targetScreenSize = new ScreenSize { width = 480, height = 270 };  // Only used with PixelScreenMode.Resize
    public uint screenScaleFactor = 270;  //set to the height of the desired screen size

    
    [Header("Display")]
    public RawImage display;

    private void Awake()
    {
        // Try to set as main pixel camera
        if (main == null) main = this;
    }

    private void Start()
    {
        // Initialize the system
        Init();
    }

    private void Update()
    {
        // Re initialize system if the screen has been resized
        if (CheckScreenResize()) Init();
    }

    public void Init() {

        // Initialize the camera and get screen size values
        if (!renderCamera) renderCamera = GetComponent<Camera>();
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        // Prevent any error
        if (screenScaleFactor < 1) screenScaleFactor = 1;
        if (targetScreenSize.width < 1) targetScreenSize.width = 1;
        if (targetScreenSize.height < 1) targetScreenSize.height = 1;


        float aspectRatio = Camera.main.aspect;
        // Calculate the render texture size
        int width = Mathf.RoundToInt(screenScaleFactor * aspectRatio);
        int height = Mathf.RoundToInt(screenScaleFactor);

        // Set the target screen size
        targetScreenSize = new ScreenSize
        {
            width = width,
            height = height
        };


        // Initialize the render texture
        renderTexture = new RenderTexture(width, height, 24) {
            filterMode = FilterMode.Point,
            antiAliasing = 1
        };

        // Set the render texture as the camera's output
        renderCamera.targetTexture = renderTexture;

        // Attaching texture to the display UI RawImage
        display.texture = renderTexture;
    }

    public bool CheckScreenResize() {
        // Check whether the screen has been resized
        return Screen.width != screenWidth || Screen.height != screenHeight;
    }
}