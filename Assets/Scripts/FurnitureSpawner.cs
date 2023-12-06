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

    public XROrigin xrOrigin;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    private List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();
    private Vector3 _screenCenter;

    private void Start()
    {
        if (Camera.main == null)
        {
            throw new NullReferenceException();
        }
        _screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
    }
    
    private void Update()
    {
        //If no touch, do nothing
        if (Input.touchCount <= 0) return;
        
        //If the touch is been held, do nothing
        var input = Input.GetTouch(0);
        if (input.phase != TouchPhase.Began) return;
        
        // If no collision, do nothing
        if (!raycastManager.Raycast(_screenCenter, _raycastHits, TrackableType.PlaneWithinPolygon)) return;
        
        var obj = Instantiate(furniture);
        obj.transform.position = _raycastHits[0].pose.position;
        obj.transform.rotation = _raycastHits[0].pose.rotation;
    }
    
}
