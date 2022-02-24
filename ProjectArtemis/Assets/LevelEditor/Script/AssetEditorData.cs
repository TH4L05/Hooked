using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AssetData
{
    public LevelParts assetList;
    public string path;
}

[CreateAssetMenu(fileName = "New AssetEditorData", menuName = "LevelEditor/AssetEditorData")]
public class AssetEditorData : ScriptableObject
{
    public List<AssetData> createdAssetDataLists = new List<AssetData> ();
    public int lastUsedListIndex;
}
