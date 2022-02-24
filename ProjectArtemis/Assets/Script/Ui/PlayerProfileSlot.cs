using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileSlot : MonoBehaviour
{
    public PlayerProfileData playerData;
    public int slotIndex;
    bool created;
    public InputField inputfield;
    public GameObject createPanel;
    public GameObject startPanel;
    public GameObject buttonStart;
    public GameObject buttonContinue;
    public GameObject buttonLevelSelect;
    public TMPro.TextMeshProUGUI playerName;
    public MenuLevel menuLevel;
    public LoadScene loadScene;

    public void CreateProfile()
    {
        if (inputfield.text != "")
        {
            created = true;
            playerData = new PlayerProfileData(inputfield.text);

            string[] levelnames = menuLevel.GetLevelNames();         
            playerData.SetLevelData(levelnames);
            Save();
            SetPanels();
        }      
    }

    public void LoadProfile()
    {
        var file = slotIndex + ".profile";

        if (Serialization.FileExistenceCheck(file))
        {
            playerData = new PlayerProfileData();
            playerData = Load();
            created = true;
            SetPanels();
        }
    }

    public void DeleteProfile()
    {
        startPanel.SetActive(false);
        createPanel.SetActive(true);
        Serialization.DeleteFile(slotIndex + ".profile");
        playerData = null;
        created = false;
        Level.instance.gameData.activePlayerProfile = null;
    }

    public void SetPanels()
    {
        if (!created) return;
        
        createPanel.SetActive(false);
        startPanel.SetActive(true);
        playerName.text = playerData.name;
        CheckIfAlreadyPlayed();       
    }

    public void CheckIfAlreadyPlayed()
    {
        if (!playerData.startedOnce)
        {
            buttonStart.SetActive(true);
            buttonContinue.SetActive(false);
            buttonLevelSelect.SetActive(false);
        }
        else
        {
            buttonStart.SetActive(false);
            buttonContinue.SetActive(true);
            buttonLevelSelect.SetActive(true);
        }      
    }

    public void Save()
    {
        Serialization.Save(playerData, slotIndex + ".profile");
    }

    public PlayerProfileData Load()
    {
        return (PlayerProfileData)Serialization.Load(slotIndex + ".profile");
    }

    public void OnProfileClick()
    {
        Level.instance.SetActivePlayerProfile(playerData, slotIndex);
    }

    public void SetLevelIndexFormProfileData()
    {
        loadScene.SetLevelIndex(playerData.lastPlayedLevel + 2);
    }

    public void SetPlayedStatus()
    {
        playerData.SetStartPlayStatus();
    }

}
