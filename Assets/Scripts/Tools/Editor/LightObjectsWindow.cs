using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LightObjectsWindow : EditorWindow
{
    private static Light[] sceneLights;

    private string myString = "Hellow";
    private bool groupEnabled;
    private bool myBool = true;
    private float myFloat = 1.23f;

    // Gizmos Interactables Settings
    private static bool drawGizmos;


    // Add menu named "Interactable Items Overview" to the Window Menu
    [MenuItem("Window/Light Objects Window")]
    static void Init()
    {
        // Get existing open window or create a new one
        LightObjectsWindow window =
            (LightObjectsWindow)EditorWindow.GetWindow(typeof(LightObjectsWindow));
        window.Show();
    }

    private void OnGUI()
    {
        groupEnabled = EditorGUILayout.BeginToggleGroup("Change Lighting", groupEnabled);
        EditorGUILayout.EndToggleGroup();

        sceneLights = FindObjectsOfType<Light>();
        Debug.Log("Found lights: " + sceneLights.Length);
        Debug.Log("Example Objects: " + sceneLights[1]);

        GUILayout.Label("Interactables Gizmo Settings", EditorStyles.boldLabel);
        drawGizmos = EditorGUILayout.Toggle("Draw Gizmos", drawGizmos);

    }

}
