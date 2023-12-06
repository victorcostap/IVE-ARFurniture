using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var r = GetComponent<Renderer>();
        r.material.color = Color.cyan;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
