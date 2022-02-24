using UnityEngine;

public struct LevelEditorStyles
{
    public static GUIStyle textstyle1 = new GUIStyle();
    public static GUIStyle textstyle2 = new GUIStyle();
    public static GUIStyle gridStyle = new GUIStyle();
    public static GUIStyle toolbarStyle = new GUIStyle();
    public static GUIStyle boxStyle = new GUIStyle();


    public static void SetStyles()
    {
        textstyle1.alignment = TextAnchor.MiddleCenter;
        textstyle1.normal.textColor = Color.red;
        textstyle1.fontStyle = FontStyle.Bold;
        textstyle1.fontSize = 25;

        textstyle2.alignment = TextAnchor.MiddleCenter;
        textstyle2.normal.textColor = Color.white;

        boxStyle.fixedWidth = 100f;
        boxStyle.normal.textColor = Color.white;


        //gridStyle.fixedHeight = 825;

    }
}
