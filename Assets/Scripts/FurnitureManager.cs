/*
    FurnitureManager Script

    Description:
    This script manages the interaction and behavior of the furniture placement system in the AR environment.
    It handles touch inputs for rotating the furniture, switching furniture models, changing colors,
    and placing the furniture itself. 
    Additionally, it manages the visibility of the UI canvases (furniture and color canvases)
    
    Note: This script assumes that the associated FurnitureSpawner and FurnitureReticle scripts are properly configured.

*/

using System.Collections.Generic;
using UnityEngine;

public class FurnitureManager : MonoBehaviour
{
    // Reference to the FurnitureSpawner script.
    // The one that actually instantiates the final furniture object
    public FurnitureSpawner spawner;

    // Reference to the FurnitureReticle script.
    // The one that manages the furniture spawn indicator
    public FurnitureReticle reticle;

    // Material for furniture colorization
    // If the "default" is used, the results are strange.
    public Material plainMaterial;

    // UI canvases
    public GameObject furnitureCanvas;
    public GameObject colorCanvas;
    
    // Rotation speed for touch input
    public float rotationSpeed = 100.0f;

    private Vector2 _touchStartPos;
    
    // Dictionary to map color IDs to corresponding Color values
    // This is due to buttons not being able to use Color parameters in the methods
    // they call. So a workaround was achieved by using ints.
    private readonly Dictionary<int, Color> _int2Color = new Dictionary<int, Color>
    {
        { 1, new Color(39, 178, 178) },
        { 2, new Color(183, 46, 46)},
        { 3, new Color(106, 180, 121)},
        { 4, new Color(226, 148, 42)},
        { 5, new Color(29, 29, 29)},
        { 6, new Color(241, 241, 241)},
        { 7, new Color(250, 244, 103)},
        { 8, new Color(43, 84, 185)},
        { 9, new Color(147, 49, 146)},
    };

    // Current color ID. -9999 is the default material
    private int _colorId = -9999; 
    
    // Switch the furniture model for the furniture and the reticle
    public void SwitchFurniture(GameObject newFurniture)
    {
        _colorId = -9999;
        spawner.SwitchFurniture(newFurniture);
        reticle.SwitchFurniture(newFurniture);
    }
    
    private void Update()
    {
        // If the furniture or the color menu is displayed, ignore rotation
        if (furnitureCanvas.activeSelf || colorCanvas.activeSelf) return;
        
        // Check if there is any touch input
        if (Input.touchCount <= 0) return;
        
        var touch = Input.GetTouch(0);
        
        switch (touch.phase)
        {
            case TouchPhase.Began:
                // Store the initial touch position
                _touchStartPos = touch.position;
                break;

            case TouchPhase.Moved:
                // Calculate the rotation based on touch movement and time since last frame
                var deltaX = touch.position.x - _touchStartPos.x;
                var rotationAngle = deltaX * rotationSpeed * Time.deltaTime;

                // Apply rotation to the reticle
                // Updating the furniture is not needed as it is not displayed
                // and the reticle rotation will be assigned only if the furniture is placed
                reticle.ChangeRotation(rotationAngle);

                // Update the touch start position for the next frame
                _touchStartPos = touch.position;
                break;
                
            case TouchPhase.Stationary:
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
            default:
                break;
        }
    }
    
    // Change the reticle color based on the selected color ID
    // Changing the color for the furniture is not needed, the color is only
    // assigned if the furniture is placed
    public void ChangeColors(int c)
    {
        _colorId = c;
        
        // Check if the color ID exists in the color dictionary
        if (!_int2Color.ContainsKey(c))
        {
            // Set the reticle to the default color if the ID is invalid
            reticle.SetDefaultColor();
            return;
        }
        
        // Change the reticle color based on the selected color ID
        reticle.ChangeColor(_int2Color[c]);
    }
    
    // Place the furniture with the specified rotation and color
    public void PlaceFurniture()
    {
        // Get the rotation from the reticle
        var rotation = reticle.GetQuaternion();
        
        Material mat;
        
        // Check if a valid color ID is selected
        if (!_int2Color.ContainsKey(_colorId))
        {
            // Set the material to null if the color ID is invalid
            mat = null;
        }
        else
        {
            // Create a new material with the selected color
            var color = _int2Color[_colorId];
            mat = new Material(plainMaterial)
            {
                color = new Color(color.r/255, color.g/255, color.b/255, 0.75f)
            };
        }
        
        // Try to place the furniture using the spawner
        // It can fail if the user is not pointing to a plane, do nothing then
        if (!spawner.PlaceFurniture(rotation, mat)) return;
        
        // If successfully placed, hide the reticle and deactivate child objects
        reticle.HideReticle();
        for (var i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
