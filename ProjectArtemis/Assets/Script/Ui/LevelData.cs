using System;

[Serializable]
public enum LevelStatus
{
    Locked,
    Unlocked,
}

[Serializable]
public class LevelData
{
    public string levelName;
    public LevelStatus status;
    public int destructionPoints;
}
