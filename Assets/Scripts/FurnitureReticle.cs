/*
    FurnitureReticle Script

    Description:
    This script manages a reticle (furniture object). This serves the purpose to display the user where the furniture
    would be placed and to know where they are pointing. The furniture's material displayed is transparent to avoid
    mistaken placed furniture with the reticle.
    The script provides functionality to change the reticle position, rotation,
    hide or display it, switch the furniture model, and change its color.

  
*/

using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FurnitureReticle : MonoBehaviour
{
    // The default furniture model.
    public GameObject furniture;
    
    // Materials for the reticle
    //Material with the default textures but transparent
    public Material transparentMaterial;
    //Material with a transparent plain color. If the "default transparent" is used, the results are strange.
    public Material transparentMaterialPlainColor;
    
    // ARFoundation components
    public ARRaycastManager raycastManager;

    // Private variables
    private readonly List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();
    private Vector3 _screenCenter;
    private GameObject _furnitureObject = null;
    private bool _firstTime = true;
    
    private void Start()
    {
        // Check if Camera.main is null and throw an exception if true
        if (Camera.main == null)
        {
            throw new NullReferenceException();
        }
        
        // Calculate the screen center for raycasting
        _screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        // Instantiate the furniture model
        InstantiateFurniture();
    }

    private void Update()
    {
        // Raycast to detect planes and update the reticle's position and rotation
        if (!raycastManager.Raycast(_screenCenter, _raycastHits, TrackableType.PlaneWithinPolygon)) return;

        _furnitureObject.transform.position = _raycastHits[0].pose.position;
        if (!_firstTime) return;
        _firstTime = false;
        _furnitureObject.transform.rotation = _raycastHits[0].pose.rotation;
    }

    private void OnEnable()
    {
        // Reset the firstTime flag when the script is enabled.
        // This is a workaround to avoid the rotation to be 0 and display the model incorrectly
        _firstTime = true;
    }

    // Change the rotation of the reticle
    public void ChangeRotation(float rotation)
    {
        _furnitureObject.transform.Rotate(Vector3.up, -rotation, Space.World);
    }

    // Get the rotation of the reticle
    public Quaternion GetQuaternion()
    {
        return _furnitureObject.transform.rotation;
    }
    
    // Hide the reticle
    public void HideReticle()
    {
        _furnitureObject.SetActive(false);
    }

    // Display the reticle
    public void DisplayReticle()
    {
        _furnitureObject.SetActive(true);
    }

    // Instantiate the furniture model and set its default color
    private void InstantiateFurniture()
    {
        // Destroy the existing furniture object if it exists
        if (_furnitureObject != null)
        {
            Destroy(_furnitureObject);
        }

        // Instantiate the new furniture object
        _furnitureObject = Instantiate(furniture);
        _furnitureObject.transform.rotation = quaternion.identity;
        SetDefaultColor();
    }

    // Get the renderer component of the reticle
    private Renderer GetRenderer()
    {
        // Check if the reticle has child objects; if so, return the renderer of the first child
        // The furniture objects used, some have children which are the actual model,
        // this makes possible to work with both types
        if (_furnitureObject.transform.childCount > 0)
        {
            return _furnitureObject.transform.GetChild(0).GetComponent<Renderer>();
        }

        // If no child objects, return the renderer of the reticle object itself
        return _furnitureObject.GetComponent<Renderer>();
    }
    
    // Switch the furniture model to a new one
    public void SwitchFurniture(GameObject newFurniture)
    {
        furniture = newFurniture;
        InstantiateFurniture();
        _firstTime = true;
    }

    // Change the color of the reticle
    public void ChangeColor(Color color)
    {
        var ren = GetRenderer();
        ren.material = transparentMaterialPlainColor;
        ren.material.color = new Color(color.r/255, color.g/255, color.b/255, 0.75f);
    }

    // Set the default color of the reticle
    public void SetDefaultColor()
    {
        var ren = GetRenderer();
        ren.material = transparentMaterial;
    } 
}
