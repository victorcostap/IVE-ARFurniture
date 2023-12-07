using System;
using UnityEngine;


public class FurnitureManager : MonoBehaviour
{
    public FurnitureSpawner spawner;
    public FurnitureReticle reticle;
    
    public float rotationSpeed = 100.0f;

    private Vector2 _touchStartPos;
    
    public void SwitchFurniture(GameObject newFurniture)
    {
        spawner.SwitchFurniture(newFurniture);
        reticle.SwitchFurniture(newFurniture);
    }
    
    private void Update()
    {
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
                spawner.ChangeRotation(rotationAngle);
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

    public void PlaceFurniture()
    {
        var rotation = reticle.GetQuaternion();
        if(!spawner.PlaceFurniture(rotation)) return;
        
        reticle.HideReticle();
        for (var i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    
}