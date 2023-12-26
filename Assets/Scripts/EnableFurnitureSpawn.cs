/*
    EnableFurnitureSpawn Script

    Description:
    This script extends the EnaDisGameObject script.
    This script manages enabling, disabling or toggling the placing of furniture in the AR environment.
    It also manages the reticle (furniture spawn indicator), enabling it and disabling it when needed.

    Note: This script assumes that the 'reticle' GameObject has a FurnitureReticle script attached.

*/

using UnityEngine;

public class EnableFurnitureSpawn : EnaDisGameObject
{
    // Reticle to display as a furniture spawn indicator
    public GameObject reticle;
    
    // Reference to the FurnitureReticle script on the reticle GameObject.
    // Used to display or hide the reticle when needed
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
