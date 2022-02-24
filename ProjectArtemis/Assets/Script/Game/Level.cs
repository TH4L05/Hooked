using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level instance;
    public Player player;
    public HookSystem HookSystem { get; private set; }
    public GameData gameData;
    public SettingsData settings;
    public InputHandler inputHandler;
    private int destructionPoints;
    public bool disableCursorOnStart = true;
    public AudioEvents audioEvents;
    public OptionsMenu optionsMenu;
    public bool optionsSetupOnly;

    void Awake()
    {   
        destructionPoints = 0;
        instance = this;
        StartSetup();
        Application.targetFrameRate = 60;
    }

    public void StartSetup()
    {       
        if (player != null)
        {
            HookSystem = player.HookSystem;
        }
        
        if (optionsMenu != null)
        {
            optionsMenu.Setup();          
        }

        if (inputHandler == null) return;
        inputHandler.SetReferences(player, HookSystem);

        if (disableCursorOnStart)
        {
            inputHandler.ChangeCursorVisibility(false);
        }
    }

    public Player GetPlayer()
    {
        return player;
    }

    public HookSystem GetHookSystem()
    {
        return HookSystem;
    }

    public void SetActivePlayerProfile(PlayerProfileData data, int indx)
    {
        gameData.activePlayerProfile = data;
        gameData.activePlayerProfileIndex = indx;
    }

    public void UpdateDestructionPoints(int points)
    {
        destructionPoints += points;
        GameEvents.OnUpdateDestructionPoints(points);
    }

    public void SaveDestructionPoints()
    {
        gameData.UpdatDestructionPoints(destructionPoints);
    }

    public void ResetDestructionPoints()
    {
        gameData.UpdatDestructionPoints(0);
    }

    public int GetDestructionPoint(int levelIndex)
    {
        return gameData.activePlayerProfile.leveldata[levelIndex].destructionPoints;
    }

}
