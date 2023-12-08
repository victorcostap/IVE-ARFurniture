using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableFurnitureSpawn : EnaDisGameObject
{
    public GameObject reticle;
    private FurnitureReticle _reticleScript;

    private void Start()
    {
        _reticleScript = reticle.GetComponent<FurnitureReticle>();
    }
    
    public new void EnableGameObject()
    {  
        base.EnableGameObject();
        _reticleScript.DisplayReticle();
    }

    public new void DisableGameObject()
    {
        _reticleScript.HideReticle();
        base.DisableGameObject();
    }
}
