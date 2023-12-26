/*
    IconGenerator Editor Script

    Description:
    This editor script creates icons from gameObjects within a specified folder and saves them as PNG files.
    The icons are generated using AssetPreview.GetAssetPreview and saved to a specified output folder.

*/

using System.Linq;
using UnityEditor;
using UnityEngine;

public class IconGenerator : EditorWindow
{
    // Default paths for prefab and icon folders
    private string prefabFolderPath = "Assets/Prefabs"; // Default path, change as needed
    private string outputFolderPath = "Assets/Icons";
    
    // Menu item to open the IconGenerator window
    [MenuItem("Window/Generate Icons from Prefabs")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(IconGenerator));
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Prefab Icon Generator", EditorStyles.boldLabel);

        // Input fields for prefab and output folder paths
        prefabFolderPath = EditorGUILayout.TextField("Prefab Folder Path", prefabFolderPath);
        outputFolderPath = EditorGUILayout.TextField("Output icon folder", outputFolderPath);

        // Button to trigger icon generation
        if (GUILayout.Button("Generate Icons"))
        {
            GenerateIcons();
        }
    }

    // Generate icons and save them to the specified output folder
    private void GenerateIcons()
    {
        // Find all GameObjects in the specified prefab folder
        string[] prefabPaths = AssetDatabase.FindAssets("t:GameObject", new[] { prefabFolderPath })
            .Select(AssetDatabase.GUIDToAssetPath)
            .ToArray();

        // Check if prefabs were found
        if (prefabPaths.Length == 0)
        {
            Debug.LogWarning("No prefabs found in the specified folder: " + prefabFolderPath);
            return;
        }

        // Iterate through each GO and generate and save its icon
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