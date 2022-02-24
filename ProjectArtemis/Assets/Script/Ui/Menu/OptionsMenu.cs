using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    #region Serialized Fields

    [Header("Data")]
    [SerializeField] private SettingsData data;
    public InputActionAsset inputAsset;

    [Header("Slider")]
    [SerializeField] private Slider sensitivitySlider;    
    [SerializeField] private Slider masterVolumeSlider;  
    [SerializeField] private Slider musicVolumeSlider;   
    [SerializeField] private Slider sfxVolumeSlider;   
    [SerializeField] private Slider gammaSlider;
   
    [Header("SliderTexts")]
    [SerializeField] private TextMeshProUGUI sensitivitySliderText;
    [SerializeField] private TextMeshProUGUI masterVolumeSliderText;
    [SerializeField] private TextMeshProUGUI musicVolumeSliderText;
    [SerializeField] private TextMeshProUGUI sfxVolumeSliderText;
    [SerializeField] private TextMeshProUGUI gammaSliderText;

    [Header("Dropdowns")]
    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    //[SerializeField] private TMP_Dropdown qualityLevelDropdown;
    //[SerializeField] private TMP_Dropdown fullScreenDropdown;
    //[SerializeField] private TMP_Dropdown vSyncDropdown;

    [Header("OptionTexts")]
    [SerializeField] private TextMeshProUGUI fullScreenOptionText;
    [SerializeField] private TextMeshProUGUI vsyncOptionText;
    [SerializeField] private TextMeshProUGUI resolutionOptionText;
    [SerializeField] private TextMeshProUGUI qualtiyOptionText;

    [Header("Volumes")]
    public VolumeProfile gammaProfile;

    #endregion

    #region Private Fields

    private string[] qualityLevel;
    private float mouseSensitivity = 1f;
    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private int currentResolutionIndex = 0;
    private int currentResolutionWidth = 640;
    private int currentResolutionHeight = 480;
    Resolution[] resolutions;
    private bool fullScreen = true;
    private bool vsync = true;
    private int currentQualityLevel = 0;

    #endregion

    #region Unity Functions

    private void Awake()
    {
        //Setup();
    }

    #endregion

    #region Setup

    public void Setup()
    {
        resolutions = Screen.resolutions;
        qualityLevel = QualitySettings.names;

        Load();
        SetSFXVolume();
        SetMusicVolume();
        SetMasterVolume();
        SetFullScreen();
        SetVsync();
        SetMouseSensitivity();
        //SetResolution();
        ResolutionDropdownSetup();
        SetQuality();
        SetSensitivityInPlayer();
        //QualityLevelSetup();
        


        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
                data.SetResolutionValues(resolutions[i].width, resolutions[i].height, currentResolutionIndex);
            }
        }
    }

    private void ResolutionDropdownSetup()
    {
        resolutionsDropdown.ClearOptions();

        List<string> resolutionStrings = new List<string>();   

        for (int i = 0; i < resolutions.Length; i++)
        {
            var resolutionString = resolutions[i].width + " x " + resolutions[i].height;
            resolutionStrings.Add(resolutionString);           
        }

        CheckCurrentResolution();       
        resolutionsDropdown.AddOptions(resolutionStrings);
        resolutionsDropdown.value = currentResolutionIndex;
    }

    public void SetSensitivityInPlayer()
    {
        if (Level.instance.player == null) return;
        Level.instance.player.SetSensitivity(data.Sensitivity);
    }

    private void CheckCurrentResolution()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
                currentResolutionWidth = resolutions[i].width;
                currentResolutionHeight = resolutions[i].height;
                data.SetResolutionValues(currentResolutionWidth, currentResolutionHeight, currentResolutionIndex);
                SetResolution();
                return;
                
            }
        }

        if (currentResolutionIndex == 0)
        {
            SetDefaultResolution();
        }

    }

    private void SetDefaultResolution()
    {
        currentResolutionWidth = data.dfaultCurrentResolution_width;
        currentResolutionHeight = data.dfaultCurrentResolution_height;

        Screen.SetResolution(currentResolutionWidth, currentResolutionHeight, true);
        CheckCurrentResolution();
    }

    /*private void QualityLevelSetup()
    {
        qualityLevelDropdown.ClearOptions();

        List<string> qualityLevelStrings = new List<string>();

        for (int i = 0; i < qualityLevel.Length; i++)
        {
            var qualityLevelString = qualityLevel[i];
            qualityLevelStrings.Add(qualityLevelString);
        }

        qualityLevelDropdown.AddOptions(qualityLevelStrings);

        currentQualityLevel = data.QualityLevelIndex;
        qualityLevelDropdown.value = currentQualityLevel;

    }*/

    #endregion

    #region Change Options

    public void ChangeMouseSensitivity(float value)
    {     
        data.SetSensitivity(value);
        SetMouseSensitivity();
    }

    public void ChangeMasterVolume(float volumeValue)
    {
        data.SetMasterVolume(volumeValue);
        SetMasterVolume();
    }

    public void ChangeMusicVolume(float volumeValue)
    {
        data.SetMusicVolume(volumeValue);
        SetMusicVolume();
    }

    public void ChangeSFXVolume(float volumeValue)
    {
        data.SetSFXVolume(volumeValue);
        SetSFXVolume();
    }

    /*public void ChangeQuality(int indx)
    {
        currentQualityLevel = indx;
        data.SetQualityLevelIndex(currentQualityLevel);
        QualitySettings.SetQualityLevel(currentQualityLevel);
    }*/

    public void ChangeQualityMinus()
    {
        if (currentQualityLevel > 0) currentQualityLevel--;     
        data.SetQualityLevelIndex(currentQualityLevel);
        SetQuality();
    }

    public void ChangeQualityPlus()
    {
        if(currentQualityLevel < qualityLevel.Length -1) currentQualityLevel++;
        data.SetQualityLevelIndex(currentQualityLevel);
        SetQuality();
    }


    // Change Resolution with Dropdown
    public void ChangeResolution(int idx)
    {
        currentResolutionIndex = idx;
        currentResolutionWidth = resolutions[idx].width;
        currentResolutionHeight = resolutions[idx].height;
        data.SetResolutionValues(currentResolutionWidth, currentResolutionHeight, currentResolutionIndex);
        SetResolution();
    }

    public void ChangeResolutionMinus()
    {
        if (currentResolutionIndex > 0)
        {
            currentResolutionIndex--;
        }
        else
        {
            currentResolutionIndex = resolutions.Length - 1;
        }

        currentResolutionWidth = resolutions[currentResolutionIndex].width;
        currentResolutionHeight = resolutions[currentResolutionIndex].height;
        data.SetResolutionValues(currentResolutionWidth, currentResolutionHeight, currentResolutionIndex);
        SetResolution();
    }

    public void ChangeResolutionPlus()
    {
        if (currentResolutionIndex < resolutions.Length - 1)
        {
            currentResolutionIndex++;
        }
        else
        {
            currentResolutionIndex = 0;
        }

        currentResolutionWidth = resolutions[currentResolutionIndex].width;
        currentResolutionHeight = resolutions[currentResolutionIndex].height;
        data.SetResolutionValues(currentResolutionWidth, currentResolutionHeight, currentResolutionIndex);
        SetResolution();
    }

    public void ChangeFullScreen(bool isFullscreen)
    {
        fullScreen = isFullscreen;
        data.SetFullscreenStatus(fullScreen);
        
        SetFullScreen();

        /*if (idx == 1)
        {
            fullScreen = true;
            data.SetFullscreenStatus(fullScreen);
            Screen.fullScreen = fullScreen;
        }
        else
        {
            fullScreen = false;
            data.SetFullscreenStatus(fullScreen);
            Screen.fullScreen = fullScreen;
        }*/
    }

    public void ChangeVsync(bool isVsync)
    {
        vsync = isVsync;
        data.SetVsyncStatus(vsync);       
        SetVsync();

        /*if (idx == 1)
        {
            vsync = true;
            data.SetVsyncStatus(vsync);
            QualitySettings.vSyncCount = idx;
        }
        else
        {
            vsync= false;
            data.SetVsyncStatus(vsync);
            QualitySettings.vSyncCount = idx;
        }*/
    }

    public void ChangeGamma(float value)
    {
        LiftGammaGain lgg;

        if (gammaProfile.TryGet(out lgg))
        {
            var gvalue = (value / 10);
            var newGamma = new Vector4(1, 1, 1, gvalue);
            lgg.gamma.Override(newGamma);
            //lgg.lift.Override(newGamma);
            //lgg.gain.Override(newGamma);
        }

        //Debug.Log(lgg.gamma + "/" + lgg.lift + "/" + lgg.gain);
        
        if(gammaSliderText) gammaSliderText.text = value.ToString();
    }

    #endregion

    #region Set Options

    public void SetMouseSensitivity()
    {
        var sensitivity = 1f;

        if (data.Sensitivity == 0)
        {
            sensitivity = data.dfaultSensitivity;
        }
        else
        {
            sensitivity = data.Sensitivity;
        }

        if (sensitivitySlider) sensitivitySlider.value = sensitivity;
        if (sensitivitySliderText) sensitivitySliderText.text = sensitivity.ToString("0.0");
    }

    public void SetMasterVolume()
    {
        var volume = 0f;

        if (data.MasterVolume == 0)
        {
            volume = data.dfaultMasterVolume;
        }
        else
        {
            volume = data.MasterVolume;
        }
        if(masterVolumeSlider) masterVolumeSlider.value = volume;
        if(masterVolumeSliderText) masterVolumeSliderText.text = volume.ToString("000");
        AkSoundEngine.SetRTPCValue("MasterVolumeFader", volume);
    }

    public void SetMusicVolume()
    {
        var volume = 0f;

        if (data.MusicVolume == 0)
        {
            volume = data.dfaultMusicVolume;
        }
        else
        {
            volume = data.MusicVolume;
        }       
        if(musicVolumeSlider) musicVolumeSlider.value = volume;
        if(musicVolumeSliderText) musicVolumeSliderText.text = volume.ToString("000");
        AkSoundEngine.SetRTPCValue("MusicVolumeFader", volume);
    }

    public void SetSFXVolume()
    {
        var volume = 0f;

        if (data.SfxVolume == 0)
        {
            volume = data.dfaultSfxVolume;
        }
        else
        {
            volume = data.SfxVolume;
        }
        if(sfxVolumeSlider) sfxVolumeSlider.value= volume;
        if(sfxVolumeSliderText) sfxVolumeSliderText.text = volume.ToString("000");
        AkSoundEngine.SetRTPCValue("SfxVolumeFader", volume);      
    }

    public void SetFullScreen()
    {
        fullScreen = data.FullScreen;
        Screen.fullScreen = fullScreen;

        if (fullScreen)
        {
            fullScreenOptionText.text = "ON";
            Screen.fullScreen = true;
        }
        else
        {
            fullScreenOptionText.text = "OFF";
            Screen.fullScreen = false;
        }
    }

    public void SetVsync()
    {
        vsync = data.VSync;
        if (vsync)
        {
            vsyncOptionText.text = "ON";
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            vsyncOptionText.text = "OFF";
            QualitySettings.vSyncCount = 0;
        }
    }

    public void SetResolution()
    {
        currentResolutionIndex = data.CurrentResolutionIndex;
        currentResolutionWidth = data.CurrentResolution_width;
        currentResolutionHeight = data.CurrentResolution_height;
        resolutionsDropdown.value = currentResolutionIndex;

        resolutionOptionText.text = currentResolutionWidth + " x " + currentResolutionHeight + " , " + resolutions[currentResolutionIndex].refreshRate;
        Screen.SetResolution(currentResolutionWidth, currentResolutionHeight, fullScreen);
    }

    public void SetQuality()
    {
        currentQualityLevel = data.QualityLevelIndex;

        QualitySettings.SetQualityLevel(currentQualityLevel);
        qualtiyOptionText.text = qualityLevel[currentQualityLevel];
    }

    #endregion


    public void Save()
    { 
        string binds = inputAsset.SaveBindingOverridesAsJson();
        data.SetBindings(binds);
        data.SaveValues();
    }

    public void Load()
    {
        data.LoadValues();   
        if (data.InputActionRebindings != string.Empty)
        {
            inputAsset.LoadBindingOverridesFromJson(data.InputActionRebindings);
        }
              
    }

    public void LoadDefaults()
    {
        data.SetDefaultValues();
        SetSFXVolume();
        SetMusicVolume();
        SetMasterVolume();
        SetFullScreen();
        SetVsync();
        SetMouseSensitivity();
        SetResolution();
        SetQuality();
        inputAsset.RemoveAllBindingOverrides();
        Serialization.DeleteFile("rebindings.data");
    }
}


