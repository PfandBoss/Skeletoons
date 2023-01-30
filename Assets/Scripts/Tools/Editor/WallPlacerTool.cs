using System;
using System.Globalization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[EditorTool("Place Walls Tool")]
public class WallPlacerTool : EditorTool
{
    private static Texture2D _toolIcon;

    private VisualElement _toolRootElement;
    private ObjectField _prefabObjectField;
    private Slider _sliderField;
    private Label _sliderValue;

    private static Plane _sGroundPlane = new(Vector3.up, Vector3.zero);

    private bool _receivedClickDownEvent;
    private bool _receivedClickUpEvent;

    private bool HasPlaceableObject => _prefabObjectField?.value != null;

    public override GUIContent toolbarIcon { get; } = new()
    {
        image = _toolIcon,
        text = "Place Objects Tool",
        tooltip = "Place Objects Tool"
    };

    [InitializeOnLoadMethod]
    private static void InitPlaceWallHandler()
    {
        HandleUtility.placeObjectCustomPasses += PlaneRaycast;
    }

    private static bool PlaneRaycast(Vector2 mousePosition, out Vector3 position, out Vector3 normal)
    {
        var worldRay = HandleUtility.GUIPointToWorldRay(mousePosition);
        if (_sGroundPlane.Raycast(worldRay, out var distance))
        {
            Debug.Log(distance);
            position = worldRay.GetPoint(distance);
            normal = _sGroundPlane.normal;
            return true;
        }

        position = Vector3.zero;
        normal = Vector3.up;
        return false;
    }
    public override void OnActivated()
    {
        //Create the UI
        _toolRootElement = new VisualElement
        {
            style =
            {
                width = 200
            }
        };
        var backgroundColor = EditorGUIUtility.isProSkin
            ? new Color(0.21f, 0.21f, 0.21f, 0.8f)
            : new Color(0.8f, 0.8f, 0.8f, 0.8f);
        _toolRootElement.style.backgroundColor = backgroundColor;
        _toolRootElement.style.marginLeft = 10f;
        _toolRootElement.style.marginBottom = 10f;
        _toolRootElement.style.paddingTop = 5f;
        _toolRootElement.style.paddingRight = 5f;
        _toolRootElement.style.paddingLeft = 5f;
        _toolRootElement.style.paddingBottom = 5f;
        var titleLabel = new Label("Place Objects Tool")
        {
            style =
            {
                unityTextAlign = TextAnchor.UpperCenter
            }
        };
        
        var sliderLabel = new Label("Adjust Snapping")
        {
            style =
            {
                unityTextAlign = TextAnchor.UpperCenter
            }
        };

        _sliderValue = new Label("0")
        {
            style =
            {
                unityTextAlign = TextAnchor.UpperCenter
            }
        };
        _prefabObjectField = new ObjectField { allowSceneObjects = true, objectType = typeof(GameObject) };
        _sliderField = new Slider(0, 10);

        _toolRootElement.Add(titleLabel);
        _toolRootElement.Add(_prefabObjectField);
        sliderLabel.style.marginTop = 10f;
        
        _toolRootElement.Add(sliderLabel);
        _toolRootElement.Add(_sliderField);
        _toolRootElement.Add(_sliderValue);

        var sv = SceneView.lastActiveSceneView;
        sv.rootVisualElement.Add(_toolRootElement);
        sv.rootVisualElement.style.flexDirection = FlexDirection.ColumnReverse;
        
        SceneView.beforeSceneGui += BeforeSceneGUI;
    }

    public override void OnWillBeDeactivated()
    {
        _toolRootElement?.RemoveFromHierarchy();
        SceneView.beforeSceneGui -= BeforeSceneGUI;
    }

    private void BeforeSceneGUI(SceneView sceneView)
    {
        if (!ToolManager.IsActiveTool(this))
            return;

        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            ShowMenu();
            Event.current.Use();
        }

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {
            ToolManager.RestorePreviousTool();
            return;
        }

        if (!HasPlaceableObject)
        {
            _receivedClickDownEvent = false;
            _receivedClickUpEvent = false;
        }
        else
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                _receivedClickDownEvent = true;
                Event.current.Use();
            }

            if (_receivedClickDownEvent && Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                _receivedClickDownEvent = false;
                _receivedClickUpEvent = true;
                Event.current.Use();
            }
        }
    }
    
    public override void OnToolGUI(EditorWindow window)
    {
        //If we're not in the scene view, we're not the active tool, we don't have a placeable object, exit.
        if (window is not SceneView)
            return;

        if (!ToolManager.IsActiveTool(this))
            return;

        if (!HasPlaceableObject)
            return;
        
        _sliderValue.text = _sliderField.value.ToString(CultureInfo.InvariantCulture);

        //Draw a positional Handle.
        Handles.DrawWireDisc(GetCurrentMousePositionInScene(), Vector3.up, 0.5f);

        //If the user clicked, clone the selected object, place it at the current mouse position.
        if (_receivedClickUpEvent)
        {
            var newObject = _prefabObjectField.value;

            GameObject newObjectInstance;
            if (PrefabUtility.IsPartOfAnyPrefab(newObject))
            {
                var prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(newObject);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                newObjectInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            }
            else
            {
                newObjectInstance = Instantiate((GameObject)newObject);
            }

            newObjectInstance.transform.position = GetCurrentMousePositionInScene();

            Undo.RegisterCreatedObjectUndo(newObjectInstance, "Place new object");

            _receivedClickUpEvent = false;
        }

        //Force the window to repaint.
        window.Repaint();
    }

    Vector3 GetCurrentMousePositionInScene()
    {
        Vector3 mousePosition = Event.current.mousePosition;
        HandleUtility.PlaceObject(mousePosition, out var newPosition, out _);
        newPosition.Set(Mathf.Round(newPosition.x) , newPosition.y, Mathf.RoundToInt(newPosition.z));
        return newPosition;
    }

    void ShowMenu()
    {
        var picked = HandleUtility.PickGameObject(Event.current.mousePosition, true);
        if (!picked) return;

        var menu = new GenericMenu();
        menu.AddItem(new GUIContent($"Pick {picked.name}"), false, () => { _prefabObjectField.value = picked; });
        menu.ShowAsContext();
    }
}
