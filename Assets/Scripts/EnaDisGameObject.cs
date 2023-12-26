/*
    EnaDisGameObject Script

    Description:
    This script provides functionality to enable, disable, or toggle the active state of a specified GameObject.
    It can be useful for controlling the visibility or interaction of a GameObject during runtime.

    Note: Ensure the 'obj' field is assigned before using the script.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnaDisGameObject : MonoBehaviour
{
    // Reference to the target GameObject
    [SerializeField] public GameObject obj;

    // Enable the specified GameObject
    public void EnableGameObject()
    {  
        obj.SetActive(true);
    }

    // Disable the specified GameObject
    public void DisableGameObject()
    {
        obj.SetActive(false);
    }

    // Toggle the active state of the specified GameObject
    public void ToggleGameObject()
    {
        obj.SetActive(!obj.activeSelf);
    }
}