using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings", menuName = "ProjectArtemis/Data/Settings")]
public class SettingsData : ScriptableObject
{
    #region Fields

    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private float sensitivity = 1;
    private int currentResolutionIndex = 0;
    private int currentResolution_width = 640;
    private int currentResolution_height = 480;
    private bool fullScreen = true;
    private bool vSync = true;
    private int qualityLevelIndex = 6;
    private string inputActionRebindings;

 
    public float dfaultMasterVolume = 80f;
    public float dfaultMusicVolume = 70f;
    public float dfaultSfxVolume = 80f;
    public float dfaultSensitivity = 10f;
    public int dfaultCurrentResolutionIndex = 0;
    public int dfaultCurrentResolution_width = 1920;
    public int dfaultCurrentResolution_height = 1080;
    public bool dfaultFullScreen = true;
    public bool dfaultVsync = true;
    public int dfaultQualityLevelIndex = 6;
    public string defaultBindings;

    public float MasterVolume => masterVolume;
    public float MusicVolume => musicVolume;
    public float SfxVolume => sfxVolume;
    public float Sensitivity => sensitivity;
    public int CurrentResolutionIndex => currentResolutionIndex;
    public int CurrentResolution_width => currentResolution_width;
    public int CurrentResolution_height => currentResolution_height;
    public bool FullScreen => fullScreen;
    public bool VSync => vSync;
    public int QualityLevelIndex => qualityLevelIndex;
    public string InputActionRebindings => inputActionRebindings;

    #endregion

    #region SetValues

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
    }

    public void SetSensitivity(float value)
    {
        sensitivity = value;
    }

    public void SetResolutionValues(int width, int height, int indx)
    {
        currentResolutionIndex = indx;
        currentResolution_width = width;
        currentResolution_height = height;
    }

    public void SetFullscreenStatus(bool status)
    {
        fullScreen = status;
    }

    public void SetVsyncStatus(bool status)
    {
        vSync = status;
    }

    public void SetQualityLevelIndex(int value)
    {
        qualityLevelIndex = value;
    }

    public void SetBindings(string bindings)
    {
        if (string.IsNullOrEmpty(bindings)) return;
        inputActionRebindings = bindings;
    }

    public void SetDefaultValues()
    {
        masterVolume = dfaultMasterVolume;
        musicVolume = dfaultMusicVolume;
        sfxVolume = dfaultSfxVolume;
        sensitivity = dfaultSensitivity;
        currentResolutionIndex = dfaultCurrentResolutionIndex;
        currentResolution_width = dfaultCurrentResolution_width;
        currentResolution_height = dfaultCurrentResolution_height;
        fullScreen = dfaultFullScreen;
        vSync = dfaultVsync;
        qualityLevelIndex = dfaultQualityLevelIndex;

        SaveValues();
        Debug.Log("<color=cyan>Default Settings Loaded</color>");
    }

    #endregion

    #region Load and Save

    public void SaveValues()
    {
        string content = string.Empty;

        content += masterVolume.ToString() + "\n";
        content += musicVolume.ToString() + "\n";
        content += sfxVolume.ToString() + "\n";
        content += sensitivity.ToString() + "\n";
        content += currentResolutionIndex.ToString() + "\n";
        content += currentResolution_width.ToString() + "\n";
        content += currentResolution_height.ToString() + "\n";
        content += fullScreen.ToString() + "\n";
        content += vSync.ToString() + "\n";
        content += qualityLevelIndex.ToString();

        Serialization.SaveText(content, "settings.cfg");

        if (string.IsNullOrEmpty(inputActionRebindings)) return;
        Serialization.SaveText(inputActionRebindings, "rebindings.data");
    }

    public void LoadValues()
    {
        if (Serialization.FileExistenceCheck("settings.cfg"))
        {
            List<string> content = Serialization.LoadTextByLine("settings.cfg");

            masterVolume = float.Parse(content[0]);
            musicVolume = float.Parse(content[1]);
            sfxVolume = float.Parse(content[2]);
            sensitivity = float.Parse(content[3]);
            currentResolutionIndex = int.Parse(content[4]);
            currentResolution_width = int.Parse(content[5]);
            currentResolution_height = int.Parse(content[6]);
            fullScreen = bool.Parse(content[7]);
            vSync = bool.Parse(content[8]);
            qualityLevelIndex = int.Parse(content[9]);

            Debug.Log("<color=cyan>Saved Settings Loaded</color>");
        }
        else
        {
            //LoadDefaultValues();
            SetDefaultValues();
           
        }

        if (!Serialization.FileExistenceCheck("rebindings.data"))
        {
            inputActionRebindings = "";
        }
        else
        {
            inputActionRebindings = Serialization.LoadTextAll("rebindings.data");
        }        
    }

    /*public void LoadDefaultValues()
    {
        List<string> content = Serialization.LoadTextByLine("settings_default.cfg");

        dfaultMasterVolume = float.Parse(content[0]);
        dfaultMusicVolume = float.Parse(content[1]);
        dfaultSfxVolume = float.Parse(content[2]);
        dfaultSensitivity = float.Parse(content[3]);
        dfaultCurrentResolutionIndex = int.Parse(content[4]);
        dfaultCurrentResolution_width = int.Parse(content[5]);
        dfaultCurrentResolution_height = int.Parse(content[6]);
        dfaultFullScreen = bool.Parse(content[7]);
        dfaultVsync = bool.Parse(content[8]);
        dfaultQualityLevelIndex = int.Parse(content[9]);

        Debug.Log("<color=cyan>Default Settings Loaded</color>");
    }*/

    #endregion
}