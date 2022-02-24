using UnityEditor;
using UnityEngine;

public class AssetsEditorWindow : EditorWindow
{
    private static AssetEditorData assetEditorData;
    private string assetListName;
    private string[] assetListNames;

    public static int AssetListCount
    {
        get
        {
            return assetEditorData.createdAssetDataLists.Count;
        }
    }

    [MenuItem("Tools/ProjectArtemisLevelEditor/AssetEditorWindow")]
    public static void OpenWindow()
    {
        var window = GetWindow<AssetsEditorWindow>("AssetsLists Editor");
        window.minSize = new Vector2(250, 100);
    }

    private void OnGUI()
    {
       assetListName = GUILayout.TextField(assetListName);
        if (GUILayout.Button("Create New List"))
        {
            if (assetListName == string.Empty)
            {
                return;
            }

            CreateNewAssetList();
        }

        EditorGUILayout.Separator();
        GUILayout.BeginHorizontal();
        GUILayout.SelectionGrid(0, assetListNames, 1, GUILayout.MinWidth(50), GUILayout.MinHeight(75));
        GUILayout.EndHorizontal();
    }

    private void OnEnable()
    {
        assetEditorData = AssetDatabase.LoadAssetAtPath<AssetEditorData>("Assets/LevelEditor/DataEditor/AssetEditorData.asset");
        if (assetEditorData == null)
        {
            Debug.LogError("COULD NOT LOAD ASSETDATALIST");
        }

        assetListNames = new string[assetEditorData.createdAssetDataLists.Count];
        for (int i = 0; i < assetEditorData.createdAssetDataLists.Count; i++)
        {
            assetListNames[i] = assetEditorData.createdAssetDataLists[i].assetList.name;
        }
    }

    private void CreateNewAssetList()
    {
        var assetDataList = new AssetData();
        var assetList = ScriptableObject.CreateInstance<LevelParts>();
        var path = "Assets/LevelEditor/Data/" + assetListName + ".asset";
        AssetDatabase.CreateAsset(assetList, path);
        AssetDatabase.SaveAssets();

        assetDataList.assetList = AssetDatabase.LoadAssetAtPath<LevelParts>(path);
        assetDataList.path = "Assets/LevelEditor/Data/" + assetListName + ".asset";

        assetEditorData.createdAssetDataLists.Add(assetDataList);

    }


}
