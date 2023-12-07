using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using UnityEditor.Events;
using UnityEngine.Events;

public class ButtonGenerator : EditorWindow
{
    private GameObject gridGroup;
    private string prefabFolder;
    private string pngFolder;
    private GameObject buttonPrefab;
    private GameObject furnitureManager;
    private GameObject furnitureCanvasManager;

    [MenuItem("Window/Furniture Button Generator")]
    public static void ShowWindow()
    {
        GetWindow<ButtonGenerator>("Button Generator");
    }

    private void OnGUI()
    {
        gridGroup = EditorGUILayout.ObjectField("Grid Group", gridGroup, typeof(GameObject), true) as GameObject;
        prefabFolder = EditorGUILayout.TextField("Prefab Folder", prefabFolder);
        pngFolder = EditorGUILayout.TextField("PNG Folder", pngFolder);
        buttonPrefab = EditorGUILayout.ObjectField("Button Prefab", buttonPrefab, typeof(GameObject), true) as GameObject;
        furnitureManager = EditorGUILayout.ObjectField("FurnitureManager", furnitureManager, typeof(GameObject), true) as GameObject;
        furnitureCanvasManager = EditorGUILayout.ObjectField("FurnitureCanvasManager", furnitureCanvasManager, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Add Buttons"))
        {
            GenerateButtons();
        }
    }

    private void GenerateButtons()
    {
        if (gridGroup == null || buttonPrefab == null)
        {
            Debug.LogError("Grid Group and Button Prefab must be set!");
            return;
        }

        string[] prefabPaths = Directory.GetFiles(prefabFolder, "*.prefab");
        foreach (string prefabPath in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            GameObject button = Instantiate(buttonPrefab, gridGroup.transform);

            // Set button name based on prefab name
            button.name = prefab.name;

            // Load corresponding PNG image
            string pngPath = Path.Combine(pngFolder, $"{prefab.name}.png");
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(pngPath);

            if (sprite != null)
            {
                Image buttonImage = button.GetComponent<Image>();
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

            var buttonComponent = button.GetComponent<Button>();

            buttonComponent ??= button.AddComponent<Button>();
            buttonComponent.onClick ??= new Button.ButtonClickedEvent();
            
            // Add persistent listeners (the ones that appear in the inspector)
            AddSwitchFurniture(buttonComponent, prefab);
            AddEnableFurnitureSpawning(buttonComponent);
            AddHideFurnitureCanvas(buttonComponent);
        }

        Debug.Log("Buttons generated successfully!");
    }


    private void AddSwitchFurniture(Button buttonComponent, GameObject prefab)
    {
        var fmScript = furnitureManager.GetComponent<FurnitureManager>();
        var targetinfo = UnityEvent.GetValidMethodInfo(fmScript,
            "SwitchFurniture", new Type[] { typeof(GameObject) });
            
        UnityAction<GameObject> action = Delegate.CreateDelegate(typeof(UnityAction<GameObject>), fmScript, targetinfo, false) as UnityAction<GameObject>;            
        UnityEventTools.AddObjectPersistentListener<GameObject>(buttonComponent.onClick, action, prefab);
    }

    private void AddEnableFurnitureSpawning(Button buttonComponent)
    {
        var enaFmScript = furnitureManager.GetComponent<EnableFurnitureSpawn>();
        var targetinfo = UnityEvent.GetValidMethodInfo(enaFmScript,
            "EnableGameObject", new Type[] {});
        UnityAction action = System.Delegate.CreateDelegate(typeof(UnityAction), enaFmScript, targetinfo) as UnityAction;
        UnityEventTools.AddPersistentListener(buttonComponent.onClick, action);
    }

    private void AddHideFurnitureCanvas(Button buttonComponent)
    {
        var disFurnCanvasScript = furnitureCanvasManager.GetComponent<EnaDisGameObject>();
        var targetinfo = UnityEvent.GetValidMethodInfo(disFurnCanvasScript,
            "DisableGameObject", new Type[] {});
        UnityAction action = System.Delegate.CreateDelegate(typeof(UnityAction), disFurnCanvasScript, targetinfo) as UnityAction;
        UnityEventTools.AddPersistentListener(buttonComponent.onClick, action);
    }
}
