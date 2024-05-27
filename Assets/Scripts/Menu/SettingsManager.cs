using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown frameRateDropdown;
    [SerializeField] private Toggle vsyncToggle;


    private List<Resolution> resolutions = new List<Resolution>();

    // Calling this in awake so it sets display settings asap when the game is opened
    void Awake()
    {
        // Populates the three dropdowns
        PopulateResolutions();
        PopulateFrameRates();
        PopulateDisplayModes();

        // Loads the settings from PlayerPrefs
        LoadSettings();
    }

    //I made public methods for each dropdown that call the private ones so I didn't need to just make everything public

    /// <summary>
    /// Public method to handle the display mode dropdown value changed event
    /// </summary>
    /// <param name="index">Selected display mode</param>
    public void OnDisplayModeChanged(int index)
    {
        ApplyDisplayMode(index);
    }
    
    /// <summary>
    /// Public method to handle the resolution dropdown value changed event
    /// </summary>
    /// <param name="index">Selected resolution</param>
    public void OnResolutionChanged(int index)
    {
        ApplyResolution(index);
    }

    /// <summary>
    /// Public method to handle the frame rate dropdown value changed event
    /// </summary>
    /// <param name="index">Selected frame rate</param>
    public void OnFrameRateChanged(int index)
    {
        ApplyFrameRate(index);
    }

    /// <summary>
    /// Public method to handle the vsync toggle value changed event
    /// </summary>
    /// <param name="value">Selected vsync toggle</param>
    public void OnVsyncChanged(bool value)
    {
        ApplyVsync(value);
    }

    /// <summary>
    /// Populates the display mode dropdown
    /// </summary>
    private void PopulateDisplayModes()
    {
        displayModeDropdown.ClearOptions();
        displayModeDropdown.AddOptions(new List<TMP_Dropdown.OptionData> {
            new TMP_Dropdown.OptionData("Borderless"),
            new TMP_Dropdown.OptionData("Windowed")
        });
    }

    /// <summary>
    /// Populates the resolution dropdown
    /// </summary>
    private void PopulateResolutions()
    {
        Resolution[] originalResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        //This uses a hashset as it is the most efficient way to check for duplicates
        //If not used then there will be duplicate resolutions for monitors that can do the same res but with different HZ
        HashSet<string> uniqueResolutions = new HashSet<string>();

        foreach (var res in originalResolutions)
        {
            string resolutionString = res.width + "x" + res.height;
            if (!uniqueResolutions.Contains(resolutionString))
            {
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolutionString));
                uniqueResolutions.Add(resolutionString);
                resolutions.Add(res);
            }
        }
    }

    /// <summary>
    /// Populates the frame rate dropdown
    /// </summary>
    private void PopulateFrameRates()
    {
        frameRateDropdown.ClearOptions();
        frameRateDropdown.AddOptions(new List<string> { "30 FPS", "60 FPS", "90 FPS", "120 FPS" , "144 FPS", "240 FPS", "Uncapped" });
    }

    /// <summary>
    /// Applies the selected display mode
    /// </summary>
    /// <param name="index">Display mode index</param>
    private void ApplyDisplayMode(int index)
    {
        switch (index)
        {
            case 0: // Borderless
                Screen.fullScreen = true;
                break;
            case 1: // Windowed
                Screen.fullScreen = false;
                break;
            default:
                Debug.LogError("Invalid display mode selected!");
                break;
        }
    }
    
    /// <summary>
    /// Applies the selected resolution
    /// </summary>
    /// <param name="index">Resolution index</param>
    private void ApplyResolution(int index)
    {
        Resolution selectedResolution = resolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }

    /// <summary>
    /// Applies the selected frame rate
    /// </summary>
    /// <param name="index">Frame rate index</param>
    private void ApplyFrameRate(int index)
    {
        string selectedOption = frameRateDropdown.options[index].text;
        
        switch (selectedOption)
        {
            case "30 FPS":
                Application.targetFrameRate = 30;
                break;
            case "60 FPS":
                Application.targetFrameRate = 60;
                break;
            case "90 FPS":
                Application.targetFrameRate = 90;
                break;
            case "120 FPS":
                Application.targetFrameRate = 120;
                break;
            case "144 FPS":
                Application.targetFrameRate = 144;
                break;
            case "240 FPS":
                Application.targetFrameRate = 240;
                break;
            case "Uncapped":
                Application.targetFrameRate = -1; // Uncapped
                break;
            default:
                Debug.LogError("Invalid frame rate option selected!");
                break;
        }
    }

    /// <summary>
    /// Applies the VSync setting
    /// </summary>
    private void ApplyVsync(bool value)
    {
        if (value) //VSyncCount expects a 0 or a 1 not true or false so I have to convert it here
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    /// <summary>
    /// Loads the settings from PlayerPrefs
    /// </summary> 
    private void LoadSettings()
    {
        //To test default values uncomment the line below
        //PlayerPrefs.DeleteAll();

        //These are all inside try catch blocks as the PlayerPrefs values could have index out of range errors if the player changes monitors to one that doesn't support the resolution they had set
        //It's also just a good idea so it doesn't crash in case of manual editing of the file or random errors/data corruption.

        int resolutionIndex = resolutionDropdown.options.Count - 1; //Default is the top resolution the monitor supports in case the PlayerPrefs value is invalid
        try
        {
            resolutionIndex = PlayerPrefs.GetInt("Resolution", resolutionDropdown.options.Count - 1); //Default is the top resolution the monitor supports
        }
        catch
        {
            Debug.LogWarning("Resolution set to invalid index, setting to default.");
        }
        resolutionDropdown.value = resolutionIndex;
        ApplyResolution(resolutionIndex);        
        
        int frameRateIndex = 60; //Default is highest non uncapped in case the PlayerPrefs value is invalid
        try
        {
            frameRateIndex = PlayerPrefs.GetInt("FrameRate", 60); //Default is uncapped
        }
        catch
        {
            Debug.LogWarning("Frame rate set to invalid index, setting to default.");
        }
        frameRateDropdown.value = frameRateIndex;
        ApplyFrameRate(frameRateIndex);
        
        bool vsyncEnabled = false;    //Default value is no VSync in case the PlayerPrefs value is invalid
        try
        {
            vsyncEnabled = PlayerPrefs.GetInt("Vsync", 0) == 1;    //PlayerPrefs doesn't support bools so this converts a 1 to true and a 0 to false (If the return value is a 1 then == 1 will be true)
        }
        catch
        {
            Debug.LogWarning("VSync set to invalid value, setting to default.");
        }
        vsyncToggle.isOn = vsyncEnabled;                            //Default value is no VSync
        ApplyVsync(vsyncEnabled);
        
        int displayModeIndex = 0; //Default is borderless full screen in case the PlayerPrefs value is invalid
        try
        {
            displayModeIndex = PlayerPrefs.GetInt("DisplayMode", 0); //Default is borderless full screen
        }
        catch
        {
            Debug.LogWarning("Display mode set to invalid index, setting to default.");
        }
        displayModeDropdown.value = displayModeIndex;
        ApplyDisplayMode(displayModeIndex);
    }

    /// <summary>
    /// Saves the current settings to PlayerPrefs
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("DisplayMode", displayModeDropdown.value);
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("FrameRate", frameRateDropdown.value);
        PlayerPrefs.SetInt("Vsync", vsyncToggle.isOn ? 1 : 0); //PlayerPrefs doesn't support bools so this converts a 1 to true and a 0 to false
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Settings are already saved when changed. This just makes sure they are saved when the game is closed just in case
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveSettings();
    }
}
