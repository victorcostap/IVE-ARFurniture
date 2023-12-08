using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class FurnitureManager : MonoBehaviour
{
    public FurnitureSpawner spawner;
    public FurnitureReticle reticle;
    public Material plainMaterial;

    public GameObject furnitureCanvas;
    public GameObject colorCanvas;
    
    public float rotationSpeed = 100.0f;

    private Vector2 _touchStartPos;
    private readonly Dictionary<int, Color> _int2Color = new Dictionary<int, Color>
    {
        { 1, new Color(39,178,178) },
        { 2, new Color(183,46,46)},
        { 3, new Color(106,180,121)},
        { 4, new Color(226,148,42)},
        { 5, new Color(29,29,29)},
        { 6, new Color(241,241,241)},
        { 7, new Color(250,244,103)},
        { 8, new Color(43,84,185)},
        { 9, new Color(147,49,146)},
    };
    private int _colorId = -9999; 
    
    public void SwitchFurniture(GameObject newFurniture)
    {
        _colorId = -9999;
        spawner.SwitchFurniture(newFurniture);
        reticle.SwitchFurniture(newFurniture);
    }
    
    private void Update()
    {
        //If the color or the furniture menu is displayed, ignore rotation
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
                // Calculate the rotation based on touch movement and frame time
                var deltaX = touch.position.x - _touchStartPos.x;
                var rotationAngle = deltaX * rotationSpeed * Time.deltaTime;

                // Apply rotation
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
    
    public void ChangeColors(int c)
    {
        _colorId = c;
        if (!_int2Color.ContainsKey(c))
        {
            reticle.SetDefaultColor();
            return;
        }
        
        reticle.ChangeColor(_int2Color[c]);
    }
    
    public void PlaceFurniture()
    {
        var rotation = reticle.GetQuaternion();
        Material mat;
        if (!_int2Color.ContainsKey(_colorId))
        {
            mat = null;
        }
        else
        {
            var color = _int2Color[_colorId];
            mat = new Material(plainMaterial)
            {
                color = new Color(color.r/255, color.g/255, color.b/255, 0.75f)
            };
        }
        
        if(!spawner.PlaceFurniture(rotation, mat)) return;
        
        reticle.HideReticle();
        for (var i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}