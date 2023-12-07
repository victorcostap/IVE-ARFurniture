using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FurnitureReticle : MonoBehaviour
{
    public GameObject furniture;
    public Material transparentMaterial;    
    
    public XROrigin xrOrigin;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    private readonly List<ARRaycastHit> _raycastHits = new List<ARRaycastHit>();
    private Vector3 _screenCenter;
    private GameObject _furnitureObject = null;
    private bool _firstTime = true;
    
    private void Start()
    {
        if (Camera.main == null)
        {
            throw new NullReferenceException();
        }
        _screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        InstantiateFurniture();
    }

    private void Update()
    {
        if (!raycastManager.Raycast(_screenCenter, _raycastHits, TrackableType.PlaneWithinPolygon)) return;

        _furnitureObject.transform.position = _raycastHits[0].pose.position;
        if (!_firstTime) return;
        _firstTime = false;
        _furnitureObject.transform.rotation = _raycastHits[0].pose.rotation;
    }

    private void OnEnable()
    {
        _firstTime = true;
    }

    public void ChangeRotation(float rotation)
    {
        _furnitureObject.transform.Rotate(Vector3.up, -rotation, Space.World);
    }

    public Quaternion GetQuaternion()
    {
        return _furnitureObject.transform.rotation;
    }
    
    public void HideReticle()
    {
        _furnitureObject.SetActive(false);
    }

    public void DisplayReticle()
    {
        _furnitureObject.SetActive(true);
    }

    private void InstantiateFurniture()
    {
        if (_furnitureObject != null)
        {
            Destroy(_furnitureObject);
        }

        _furnitureObject = Instantiate(furniture);
        _furnitureObject.transform.rotation = quaternion.identity;
        if (_furnitureObject.transform.childCount > 0)
        {
            _furnitureObject.transform.GetChild(0).GetComponent<Renderer>().material = transparentMaterial;
        }
        else
        {
            _furnitureObject.GetComponent<Renderer>().material = transparentMaterial;
        }
        
    }
    
    public void SwitchFurniture(GameObject newFurniture)
    {
        furniture = newFurniture;
        InstantiateFurniture();
        _firstTime = true;
    }
    
}