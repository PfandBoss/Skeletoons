using System;
using UnityEditor;
using UnityEngine;

public class WallPlacerTool : EditorWindow
{
    private GameObject wallToUse;
    
    [MenuItem("Tools/Wall Placer Tool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(WallPlacerTool));
    }

    private void OnGUI()
    {
        GUILayout.Label("Place Wall", EditorStyles.boldLabel);
        wallToUse = EditorGUILayout.ObjectField("Wall Object to place", wallToUse, typeof(GameObject), true) as GameObject;
    }
}
