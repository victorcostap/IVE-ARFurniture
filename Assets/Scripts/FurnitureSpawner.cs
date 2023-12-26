/*
    FurnitureSpawner Script

    Description:
    This script manages the placement of furniture in an AR environment.
    It detects planes and instantiate furniture objects at the detected positions.
    It also provides functionality to switch the furniture model.

*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FurnitureSpawner : MonoBehaviour
{
    // Default furniture model
    public GameObject furniture;

    // ARRaycastManager for detecting planes
    public ARRaycastManager raycastManager;
    
    // List to store ARRaycastHit results
    private List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();
    
    private Vector3 _screenCenter;
    
    private void Awake()
    {
        // Check if Camera.main is null and throw an exception if true
        if (Camera.main == null)
        {
            throw new NullReferenceException();
        }
        
        // Calculate the screen center
        _screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
    }
    
    // Place furniture at the detected plane position with the specified rotation and material
    // If there are no planes detected, return false, otherwise true.
    public bool PlaceFurniture(Quaternion rotation, Material mat = null)
    {
        // Raycast to detect planes
        if (!raycastManager.Raycast(_screenCenter, _raycastHits, TrackableType.PlaneWithinPolygon)) return false;
        
        // Instantiate furniture at the detected plane position
        var obj = Instantiate(furniture);
        obj.transform.position = _raycastHits[0].pose.position;
        obj.transform.rotation = rotation;

        // Apply material if provided
        if (mat != null)
        {
            GetRenderer(obj).material = mat;
        }
        
        return true;
    }
    
    // Get the Renderer component of the furniture object
    private Renderer GetRenderer(GameObject go)
    {
        // Check if the furniture object has child objects; if so, return the renderer of the first child
        // The furniture objects used, some have children which are the actual model,
        // this makes possible to work with both types
        if (go.transform.childCount > 0)
        {
            return go.transform.GetChild(0).GetComponent<Renderer>();
        }

        // If no child objects, return the renderer of the furniture object itself
        return go.GetComponent<Renderer>();
    }
    
    // Switch the default furniture model to a new one
    public void SwitchFurniture(GameObject newFurniture)
    {
        furniture = newFurniture;
    }
}
