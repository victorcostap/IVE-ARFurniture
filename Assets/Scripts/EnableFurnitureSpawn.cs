using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableFurnitureSpawn : EnaDisGameObject
{
    private FurnitureReticle _reticle;

    private void Start()
    {
        _reticle = obj.GetComponent<FurnitureReticle>();
    }
    
    public new void EnableGameObject()
    {  
        base.EnableGameObject();
        _reticle.DisplayReticle();
    }

    public new void DisableGameObject()
    {
        _reticle.HideReticle();
        base.DisableGameObject();
    }
}
