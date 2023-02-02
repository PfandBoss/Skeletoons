using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InteractableItemsOverview : EditorWindow
{
    private string myString = "Hellow";
    private bool groupEnabled;
    private bool myBool = true;
    private float myFloat = 1.23f;
    
    // Gizmos Interactables Settings
    private static bool drawGizmos;
    private static Color _coffinColor = Color.green;
    private static Color _doorColor = Color.green;
    private static Color _teleportColor = Color.green;
    
    
    // Add menu named "Interactable Items Overview" to the Window Menu
    [MenuItem("Window/Interactable Items Overview")]
    static void Init()
    {
        // Get existing open window or create a new one
        InteractableItemsOverview window =
            (InteractableItemsOverview) EditorWindow.GetWindow(typeof(InteractableItemsOverview));
        window.Show();
    }

    private void OnGUI()
    {
        //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        //EditorGUILayout.EndToggleGroup();
        
        GUILayout.Label("Interactables Gizmo Settings", EditorStyles.boldLabel);
        drawGizmos = EditorGUILayout.Toggle("Draw Gizmos", drawGizmos);
        
        EditorGUILayout.Space();
        
        GUILayout.Label("Object Colors", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        _coffinColor = EditorGUILayout.ColorField("Coffin Color: ", _coffinColor);
        GUILayout.TextArea("Coffins the player can hide in.");
        EditorGUILayout.Space();
        
        _doorColor = EditorGUILayout.ColorField("Door Color: ", _doorColor);
        GUILayout.TextArea("The doors that can be opened. A line connects the door to its key.");
        EditorGUILayout.Space();
        
        _teleportColor = EditorGUILayout.ColorField("Teleport Space Color: ", _teleportColor);
        GUILayout.TextArea("Area where the character gets teleported. A line connects to the teleported spot.");
        

    }

    public static bool DrawingGizmos()
    {
        return drawGizmos;
    }
    
    public static Color CoffinColor()
    {
        return _coffinColor;
    }
    
    public static Color DoorColor()
    {
        return _doorColor;
    }
    
    public static Color TeleportColor()
    {
        return _teleportColor;
    }
}
