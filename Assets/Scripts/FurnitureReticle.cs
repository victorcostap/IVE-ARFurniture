using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FurnitureReticle : MonoBehaviour
{
    public GameObject furniture;

    public XROrigin xrOrigin;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    private readonly List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();
    private Camera _camera;
    private Vector3 _screenCenter;
    private GameObject _obj;
    
    private void Start()
    {
        if (Camera.main == null)
        {
            throw new NullReferenceException();
        }

        _obj = Instantiate(furniture);
        // var material = GetComponent<Renderer>().material;
        // var col = material.color;
        // material.color = new Color(col.r, col.g, col.b, 0.5f);
        _screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
    }

    private void Update()
    {
        if (!raycastManager.Raycast(_screenCenter, _raycastHits, TrackableType.PlaneWithinPolygon)) return;

        _obj.transform.position = _raycastHits[0].pose.position;
        _obj.transform.rotation = _raycastHits[0].pose.rotation;
    }
    
}