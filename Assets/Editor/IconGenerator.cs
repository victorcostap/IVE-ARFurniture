using System.Linq;
using UnityEditor;
using UnityEngine;

public class IconGenerator : EditorWindow
{
    private string prefabFolderPath = "Assets/Prefabs"; // Default path, change as needed
    private string outputFolderPath = "Assets/Icons";
    
    
    [MenuItem("Window/Generate Icons from Prefabs")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(IconGenerator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Icon Generator", EditorStyles.boldLabel);

        prefabFolderPath = EditorGUILayout.TextField("Prefab Folder Path", prefabFolderPath);
        outputFolderPath = EditorGUILayout.TextField("Output icon folder", outputFolderPath);

        if (GUILayout.Button("Generate Icons"))
        {
            GenerateIcons();
        }
    }

    private void GenerateIcons()
    {
        string[] prefabPaths = AssetDatabase.FindAssets("t:GameObject", new[] { prefabFolderPath })
            .Select(AssetDatabase.GUIDToAssetPath)
            .ToArray();

        if (prefabPaths.Length == 0)
        {
            Debug.LogWarning("No prefabs found in the specified folder: " + prefabFolderPath);
            return;
        }

        foreach (string prefabPath in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab != null)
            {
                Texture2D icon = AssetPreview.GetAssetPreview(prefab);
                
                if (icon != null)
                {
                    byte[] bytes = icon.EncodeToPNG();
                    string path = outputFolderPath + "/" + prefab.name + ".png";

                    System.IO.File.WriteAllBytes(path, bytes);
                    Debug.Log("Icon generated for " + prefab.name + " and saved to: " + path);
                }
            }
        }
    }
}