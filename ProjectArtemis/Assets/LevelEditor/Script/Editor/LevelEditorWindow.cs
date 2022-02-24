using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelEditorWindow : EditorWindow
{
    #region Fields

    public Material preViewMaterial;
    public static int toolbarTabIndex = 0;
    private Vector2 scrollposition = Vector2.zero;

    private static EditorData editorData;
    private static AssetEditorData assetEditorData;

    private string[] texts = { "Edit", "Draw", "Erase",};
    public static bool EditorIsActive = false;

    private LevelEditorHandle handle;
    private LevelEditorObjectPlacement objecthandle;

    private Vector3 scale = Vector3.zero;
    private float objRotation;
    private int height;

    public static int assetDataListCount;
    private LevelParts[] levelAssetLists;
    private List<GUIContent[]> gridContents = new List<GUIContent[]>();
    private int[] gridIndxs;
    private int[] lastgridIndxs;
    private string[] foldOutTexts;
    private bool[] foldOuts;
    private Vector2[] scrollPositions;
    private GameObject activeObj;

    #endregion

    public LevelEditorWindow()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    [MenuItem("Tools/ProjectArtemisLevelEditor/LevelEditorWindow")]
    public static void OpenWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    #region UnityFunctions

    private void OnGUI()
    {
        GUILayout.Label("ProjectArtemis Level Editor", LevelEditorStyles.textstyle1);
        GUILayout.Space(5);
        toolbarTabIndex = GUILayout.Toolbar(toolbarTabIndex, texts,GUILayout.Height(35));        
        GUILayout.Space(10);
        EditorGUILayout.Separator();
        preViewMaterial = (Material)EditorGUILayout.ObjectField("PreView Material",preViewMaterial,typeof(Material), true);
        GUILayout.Space(10);
        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Rotate +90"))
        {
            objRotation += 90f;
            handle.SetHandleRotation(objRotation);
            Debug.Log(objRotation);
        }
        if (GUILayout.Button("Rotate -90"))
        {
            objRotation -= 90f;
            handle.SetHandleRotation(objRotation);
            Debug.Log(objRotation);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5);
        height = EditorGUILayout.IntSlider(height, -50, 50);
        handle.SetHandleHeight(height);
        GUILayout.Space(10);
        EditorGUILayout.Separator();
        scrollposition = GUILayout.BeginScrollView(scrollposition, false, true);
        for (int i = 0; i < assetDataListCount; i++)
        {
            foldOuts[i] = EditorGUILayout.Foldout(foldOuts[i], foldOutTexts[i], true);
            if (foldOuts[i])
            {
                scrollPositions[i] = GUILayout.BeginScrollView(scrollPositions[i]);
                gridIndxs[i] = GUILayout.SelectionGrid(gridIndxs[i], gridContents[i], 2);
                SetActiveObject(i, gridIndxs[i]);
                GUILayout.EndScrollView();
                //GUILayout.Space(5);
                //EditorGUILayout.Separator();
            }
        }
        GUILayout.EndScrollView();
        CheckToolBarIndex();
    }

    private void OnEnable()
    {
        LoadData();
        Setup();
    }

    private void OnDestroy()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        EditorIsActive = false;
        handle.DestroyHandle();
        handle = null;
        objecthandle = null;
        ToolbarOption1();
        SceneView.RepaintAll();
    }

    #endregion

    #region ToolbarOptions

    void ToolbarOption1()
    {
        Tools.hidden = false;
        //Debug.Log("OPtion1 Selected");       
        SceneView.RepaintAll();
    }

    void ToolbarOption2()
    {
        Tools.hidden = true;
        //Debug.Log("OPtion2 Selected");       
        SceneView.RepaintAll();
        handle.SetDrawColor(Color.yellow);
        handle.SetObjectScale(scale);
    }

    void ToolbarOption3()
    {
        Tools.hidden = true;
        //Debug.Log("OPtion3 Selected");
        SceneView.RepaintAll();
        handle.SetDrawColor(Color.magenta);
        handle.SetActiveObject(null, null);
        handle.SetObjectScale(new Vector3(1f, 1f, 1f));
    }

    #endregion

    private void Setup()
    {
        toolbarTabIndex = 0;
        //LoadLevelPartsAssetTest();
        EditorIsActive = true;
        handle = new LevelEditorHandle();
        handle.IntializeHandle();
        objecthandle = new LevelEditorObjectPlacement();
        LoadLevelPartsAssetList();
    }

    void OnSceneGUI(SceneView sceneView)
    {
        //Debug.Log("TEST");
        LevelEditorObjectPlacement();
        
    }

    void LevelEditorObjectPlacement()
    {
        if (toolbarTabIndex == 0) return;

        //By creating a new ControlID here we can grab the mouse input to the SceneView and prevent Unitys default mouse handling from happening
        //FocusType.Passive means this control cannot receive keyboard input since we are only interested in mouse input
        int controlId = GUIUtility.GetControlID(FocusType.Passive);
      
        if (Event.current.type == EventType.MouseDown &&
            Event.current.button == 0 &&
            Event.current.alt == false &&
            Event.current.shift == false &&
            Event.current.control == false)
        {
            if (handle.IsMouseInValidArea == true)
            {
                if (toolbarTabIndex == 2)
                {
                    objecthandle.RemoveBlock(handle.CurrentHandlePosition);
                }

                if (toolbarTabIndex == 1)
                {
                    //if (gridIndex < levelParts.Parts.Count)
                    //{
                        objecthandle.AddBlock(handle.CurrentHandlePosition, activeObj, objRotation);
                    //}
                }
            }
        }

        if (Event.current.type == EventType.KeyDown &&
            Event.current.keyCode == KeyCode.Escape)
        {
            toolbarTabIndex = 0;
        }

        HandleUtility.AddDefaultControl(controlId);

    }

    void CheckToolBarIndex()
    {
        switch (toolbarTabIndex)
        {
            case 0:
            default:
                ToolbarOption1();
                break;
            case 1:
                ToolbarOption2();
                break;
            case 2:
                ToolbarOption3();
                break;
        }
    }

    void SetActiveObject(int indx, int gridIndex)
    {
        activeObj = levelAssetLists[indx].Parts[gridIndex].Template;
        handle.SetActiveObject(activeObj, preViewMaterial);
    }

    private void LoadData()
    {
        editorData = AssetDatabase.LoadAssetAtPath<EditorData>("Assets/LevelEditor/DataEditor/EditorDefault.asset");
        assetEditorData = AssetDatabase.LoadAssetAtPath<AssetEditorData>("Assets/LevelEditor/DataEditor/AssetEditorData.asset");
        LevelEditorStyles.SetStyles();
        preViewMaterial = editorData.previewMaterial;
    }

    private void LoadLevelPartsAssetList()
    {
        assetDataListCount = assetEditorData.createdAssetDataLists.Count;
        levelAssetLists = new LevelParts[assetDataListCount];
        gridIndxs = new int[assetDataListCount];
        lastgridIndxs = new int[assetDataListCount];
        foldOutTexts = new string[assetDataListCount];
        foldOuts = new bool[assetDataListCount];
        scrollPositions = new Vector2[assetDataListCount];

        for (int i = 0; i < assetDataListCount; i++)
        {
            levelAssetLists[i] = AssetDatabase.LoadAssetAtPath<LevelParts>("Assets/LevelEditor/Data/" + assetEditorData.createdAssetDataLists[i].assetList.name + ".asset");
            foldOutTexts[i] = levelAssetLists[i].name;
            scrollPositions[i] =  Vector2.zero;

            if (i == 0)
            {
                foldOuts[i] = true;
            }
            else
            {
                foldOuts[i] = false;
            }

            var gridAmount = levelAssetLists[i].Parts.Count;
            if (gridAmount == 0) return;
            var gridContent = new GUIContent[gridAmount];

            for (int p = 0; p < gridAmount; p++)
            {
                string text = levelAssetLists[i].Parts[p].partName;
                Texture image = AssetPreview.GetAssetPreview(levelAssetLists[i].Parts[p].Template);
                gridContent[p] = new GUIContent(text,image, text);
            }

            gridContents.Add(gridContent);
        }      
    }
}