using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerProfileData
{   
    public string name = "";
    public LevelData[] leveldata;
    public int lastPlayedLevel;
    public int destructionPointsTotal;
    public bool startedOnce;

    public PlayerProfileData()
    {
    }

    public PlayerProfileData(string name)
    {
        this.name = name;
    }  

    public void SetLevelData(string[] levelNames)
    {
        Debug.Log(levelNames.Length);

        leveldata = new LevelData[levelNames.Length];

        for (int i = 0; i < levelNames.Length; i++)
        {
            leveldata[i] = new LevelData();
            leveldata[i].levelName = levelNames[i];

            if (i < 1)
            {
                leveldata[i].status = LevelStatus.Unlocked;
                continue;
            }            
            leveldata[i].status = LevelStatus.Locked;
        }
    }

    public void UnlockLevel(int idx)
    {
        if (idx > leveldata.Length) return;
        leveldata[idx].status = LevelStatus.Unlocked;
    }

    public void SetLastPlayedLevel(int levelIndex)
    {
        lastPlayedLevel = levelIndex;
    }

    public void UpdateDestructionPoints(int levelIndex, int points)
    {  
        if (levelIndex > leveldata.Length || leveldata.Length == 0) return;
        leveldata[levelIndex].destructionPoints = points;
    }

    public void SetStartPlayStatus()
    {
        startedOnce = true;
    }

    public void TotalDestructionPoints()
    {
        int points = 0;

        foreach (var level in leveldata)
        {
            points += level.destructionPoints;
        }

        destructionPointsTotal = points;
    }

    public int GetTotalDestructionPoints()
    {
        int points = 0;

        foreach (var level in leveldata)
        {
            points += level.destructionPoints;
        }

        destructionPointsTotal = points;

        return destructionPointsTotal;
    }

}
