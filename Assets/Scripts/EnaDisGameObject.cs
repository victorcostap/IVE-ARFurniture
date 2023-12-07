using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnaDisGameObject : MonoBehaviour
{
    [SerializeField] public GameObject obj;
    
    public void EnableGameObject()
    {  
        obj.SetActive(true);
    }

    public void DisableGameObject()
    {
        obj.SetActive(false);
    }

    public void ToggleGameObject()
    {
        obj.SetActive(!obj.activeSelf);
    }
}