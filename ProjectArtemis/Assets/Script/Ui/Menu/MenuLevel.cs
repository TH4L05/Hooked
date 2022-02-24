using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuLevel : MonoBehaviour
{
    public Button[] levels;
    
    public string[] GetLevelNames()
    {
        string[] names = new string[levels.Length];

        for (int i = 0; i < levels.Length; i++)
        {
            var label = levels[i].GetComponent<LevelButton>().levelname;
            if (label != null)
            {
                names[i] = label;
            }
            else
            {
                names[i] = "MissingLabel"; 
            }
        }

        //Debug.Log(names.Length);
        return names;
    }

    public void SetLevelStatus()
    {
        var playerprofile = Level.instance.gameData.activePlayerProfile;

        for (int i = 0; i < levels.Length; i++)
        {
            var levelButton = levels[i].GetComponent<LevelButton>();
            levelButton.SetNameAndNumber(i+1, playerprofile.leveldata[i].levelName);

            var status = playerprofile.leveldata[i].status;
            //if (status == LevelStatus.Locked) continue;
            levelButton.SetSprite(status);
        }
    }
}
