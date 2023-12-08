using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FurnitureSpawner : MonoBehaviour
{
    public GameObject furniture;
    public ARRaycastManager raycastManager;
    
    private List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();
    private Vector3 _screenCenter;

    private void Awake()
    {
        if (Camera.main == null)
        {
            throw new NullReferenceException();
        }
        _screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
    }
    
    public bool PlaceFurniture(Quaternion rotation, Material mat = null)
    {
        if (!raycastManager.Raycast(_screenCenter, _raycastHits, TrackableType.PlaneWithinPolygon)) return false;
        
        var obj = Instantiate(furniture);
        obj.transform.position = _raycastHits[0].pose.position;
        obj.transform.rotation = rotation;

        if (mat != null)
        {
            GetRenderer(obj).material = mat;
        }
        
        return true;
    }
    
    private Renderer GetRenderer(GameObject go)
    {
        if (go.transform.childCount > 0)
        {
            return go.transform.GetChild(0).GetComponent<Renderer>();
        }

        return go.GetComponent<Renderer>();
    }
    
    public void SwitchFurniture(GameObject newFurniture)
    {
        furniture = newFurniture;
    }
    
    
}
