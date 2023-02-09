#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class InteractableItemsOverview : EditorWindow
{   
    // Gizmos Interactables Settings
    private static bool drawGizmos;
    private static Color _coffinColor = Color.magenta;
    private static Color _doorColor = Color.green;
    private static Color _teleportColor = Color.cyan;

    // Set Interactables
    private string _setDoorButton = "Set Door";
    private string _setKeyButton = "Set respective Key";
    private string _setTeleportButton = "Set teleport";
    private string _setCoffinButton = "Set hiding spot";
    private GameObject door;
    private GameObject key; 
    private GameObject teleport;
    private GameObject coffin;
    private Door doorscript;
    
    
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
        GUILayout.Label("Coffins the player can hide in.", EditorStyles.miniLabel);
        EditorGUILayout.Space();
        
        _doorColor = EditorGUILayout.ColorField("Door Color: ", _doorColor);
        GUILayout.Label("The doors that can be opened. A line connects the door to its key.", EditorStyles.miniLabel);
        EditorGUILayout.Space();
        
        _teleportColor = EditorGUILayout.ColorField("Teleport Space Color: ", _teleportColor);
        GUILayout.Label("Area where the character gets teleported. A line connects to the teleported spot.", EditorStyles.miniLabel);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUILayout.Label("Set Interactables", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        GUILayout.Label("Select a door object in your scene, then press the button.", EditorStyles.miniLabel);
        if(GUILayout.Button(_setDoorButton)) {
            door = (GameObject) Selection.activeObject;
            if (!door.TryGetComponent<Door>(out doorscript)) {
                door.AddComponent<InteractableObject>().type = InteractableObject.InteractableType.Door;
                doorscript = door.AddComponent<Door>();
                door.tag = "Interactable";
                door.layer = LayerMask.NameToLayer("Obstacles");
            }
        }

        EditorGUILayout.Space();

        GUILayout.Label("Select the respective key object in your scene, then press the button.", EditorStyles.miniLabel);
        if(GUILayout.Button(_setKeyButton)) {
            key = (GameObject) Selection.activeObject;
            if (!key.TryGetComponent<Item>(out Item keyscript)) {
                key.AddComponent<InteractableObject>().type = InteractableObject.InteractableType.Item;
                Item item = key.AddComponent<Item>();
                key.tag = "Interactable";
                key.layer = LayerMask.NameToLayer("Obstacles");
                doorscript.SetKey(key);
                item.SetContainer(GameObject.Find("Player").transform.Find("PlayerArmature").Find("ItemHolder"));
            }
        }

        EditorGUILayout.Space();

        GUILayout.Label("Select a teleport object, then press the button.", EditorStyles.miniLabel);
        if(GUILayout.Button(_setTeleportButton)) {
            teleport = (GameObject) Selection.activeObject;
            if (!key.TryGetComponent<PositionSwitch>(out PositionSwitch pos)) {
                key.AddComponent<PositionSwitch>();
                key.tag = "Untagged";
                key.layer = LayerMask.NameToLayer("Default");
            }
        }

        EditorGUILayout.Space();

        GUILayout.Label("Select an object for the skeleton to hide in, then press the button.", EditorStyles.miniLabel);
        if(GUILayout.Button(_setCoffinButton)) {
            coffin = (GameObject) Selection.activeObject;
            if (!coffin.TryGetComponent<Locker>(out Locker locker)) {
                coffin.AddComponent<InteractableObject>().type = InteractableObject.InteractableType.Locker;
                coffin.AddComponent<Locker>();
                coffin.tag = "Interactable";
                coffin.layer = LayerMask.NameToLayer("Obstacles");
            }
        }
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
#endif
