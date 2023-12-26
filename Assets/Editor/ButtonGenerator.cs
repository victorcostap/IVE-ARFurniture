/*
    ButtonGenerator Editor Script

    Description:
    This editor script generates buttons for each gameObject in a designated folder.
    These generated buttons are then added to a specified grid, and an image
    is assigned as the icon from a specified folder. The icon must follow the 
    naming convention "<gameObject name>.png". 
    
    Additionally, the script assigns onClick events to the buttons, 
    allowing each button to perform actions such as switching the furniture model,
    enabling the reticle, and spawning mode, and hiding the furniture menu
    upon being clicked.
*/

using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using UnityEditor.Events;
using UnityEngine.Events;

public class ButtonGenerator : EditorWindow
{
    private GameObject _gridGroup;
    private string _prefabFolder;
    private string _pngFolder;
    private GameObject _buttonPrefab;
    private GameObject _furnitureManager;
    private GameObject _furnitureCanvasManager;

    // Menu item to open the ButtonGenerator window
    [MenuItem("Window/Furniture Button Generator")]
    public static void ShowWindow()
    {
        GetWindow<ButtonGenerator>("Button Generator");
    }

    private void OnGUI()
    {
        // Input fields for specifying parameters
        _gridGroup = EditorGUILayout.ObjectField("Grid Group", _gridGroup, typeof(GameObject), true) as GameObject;
        _prefabFolder = EditorGUILayout.TextField("Prefab Folder", _prefabFolder);
        _pngFolder = EditorGUILayout.TextField("PNG Folder", _pngFolder);
        _buttonPrefab = EditorGUILayout.ObjectField("Button Prefab", _buttonPrefab, typeof(GameObject), true) as GameObject;
        _furnitureManager = EditorGUILayout.ObjectField("FurnitureManager", _furnitureManager, typeof(GameObject), true) as GameObject;
        _furnitureCanvasManager = EditorGUILayout.ObjectField("FurnitureCanvasManager", _furnitureCanvasManager, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Add Buttons"))
        {
            GenerateButtons();
        }
    }

    private void GenerateButtons()
    {
        // Check if necessary objects are set
        if (_gridGroup == null || _buttonPrefab == null)
        {
            Debug.LogError("Grid Group and Button Prefab must be set!");
            return;
        }

        // Find all prefab paths in the specified folder
        var prefabPaths = Directory.GetFiles(_prefabFolder, "*.prefab");
        foreach (var prefabPath in prefabPaths)
        {
            // Load the prefab and instantiate a button
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            var button = Instantiate(_buttonPrefab, _gridGroup.transform);

            // Set button name based on prefab name
            button.name = prefab.name;

            // Load corresponding PNG image and set it as the button's sprite
            var pngPath = Path.Combine(_pngFolder, $"{prefab.name}.png");
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(pngPath);

            if (sprite != null)
            {
                var buttonImage = button.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.sprite = sprite;
                }
                else
                {
                    Debug.LogWarning("Button Prefab does not have an Image component!");
                }
            }
            else
            {
                Debug.LogWarning($"No PNG found for prefab {prefab.name}");
            }

            // Get the Button component of the button GameObject
            var buttonComponent = button.GetComponent<Button>();

            // Add persistent listeners for button click events
            AddSwitchFurniture(buttonComponent, prefab);
            AddEnableFurnitureSpawning(buttonComponent);
            AddHideFurnitureCanvas(buttonComponent);
        }

        Debug.Log("Buttons generated successfully!");
    }

    // Add a persistent listener to switch furniture in FurnitureManager on button click
    private void AddSwitchFurniture(Button buttonComponent, GameObject prefab)
    {
        var fmScript = _furnitureManager.GetComponent<FurnitureManager>();
        var targetinfo = UnityEventBase.GetValidMethodInfo(fmScript,
            "SwitchFurniture", new Type[] { typeof(GameObject) });
            
        UnityAction<GameObject> action = Delegate.CreateDelegate(typeof(UnityAction<GameObject>), fmScript, targetinfo, false) as UnityAction<GameObject>;            
        UnityEventTools.AddObjectPersistentListener<GameObject>(buttonComponent.onClick, action, prefab);
    }

    // Add a persistent listener to enable furniture spawning in EnableFurnitureSpawn on button click
    private void AddEnableFurnitureSpawning(Button buttonComponent)
    {
        var enaFmScript = _furnitureManager.GetComponent<EnableFurnitureSpawn>();
        var targetinfo = UnityEvent.GetValidMethodInfo(enaFmScript,
            "EnableGameObject", new Type[] {});
        var action = System.Delegate.CreateDelegate(typeof(UnityAction), enaFmScript, targetinfo) as UnityAction;
        UnityEventTools.AddPersistentListener(buttonComponent.onClick, action);
    }

    // Add a persistent listener to hide the furniture canvas in EnaDisGameObject on button click
    private void AddHideFurnitureCanvas(Button buttonComponent)
    {
        var disFurnCanvasScript = _furnitureCanvasManager.GetComponent<EnaDisGameObject>();
        var targetinfo = UnityEvent.GetValidMethodInfo(disFurnCanvasScript,
            "DisableGameObject", new Type[] {});
        var action = System.Delegate.CreateDelegate(typeof(UnityAction), disFurnCanvasScript, targetinfo) as UnityAction;
        UnityEventTools.AddPersistentListener(buttonComponent.onClick, action);
    }
}
