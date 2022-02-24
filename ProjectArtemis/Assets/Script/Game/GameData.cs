using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New GameData", menuName = "ProjectArtemis/Data/GameData")]
public class GameData : ScriptableObject
{
    public PlayerProfileData activePlayerProfile;
    public int activePlayerProfileIndex;

    public void SaveActiveProfile()
    {
        if (activePlayerProfile == null) return;
        Serialization.Save(activePlayerProfile, activePlayerProfileIndex + ".profile");
    }

    public void UnlockLevel(int indx)
    {
        if (activePlayerProfile == null) return;
        activePlayerProfile.UnlockLevel(indx);
    }

    public void SetLastPlayedLevel(int indx)
    {
        if (activePlayerProfile == null) return;
        activePlayerProfile.lastPlayedLevel = indx;
    }

    public void UpdatDestructionPoints(int points)
    {
        if (activePlayerProfile == null) return;
        activePlayerProfile.UpdateDestructionPoints(activePlayerProfile.lastPlayedLevel, points);
    }

    public void SetTotalDestructionPoints()
    {
        if (activePlayerProfile == null) return;
        activePlayerProfile.TotalDestructionPoints();

    }

    public int GetTotalDestructionPoints()
    {
        if (activePlayerProfile == null) return 0;
        return activePlayerProfile.GetTotalDestructionPoints();
    }

}
