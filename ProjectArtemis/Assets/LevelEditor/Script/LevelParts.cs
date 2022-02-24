using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelPart
{
    public string partName;
    public GameObject Template;
}

[CreateAssetMenu(fileName = "New LevelParts", menuName = "LevelEditor/LevelParts")]
public class LevelParts : ScriptableObject
{
   public List<LevelPart> Parts = new List<LevelPart>();
}
