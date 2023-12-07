using UnityEngine;


public class FurnitureManager : MonoBehaviour
{
    public FurnitureSpawner spawner;
    public FurnitureReticle reticle;
    
    public void SwitchFurniture(GameObject newFurniture)
    {
        spawner.SwitchFurniture(newFurniture);
        reticle.SwitchFurniture(newFurniture);
    }
}