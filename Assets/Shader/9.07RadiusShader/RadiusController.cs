using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusController : MonoBehaviour
{
    public Material radiusMaterial;

    public float radius = 1;
    public Color color=Color.white;

    // Update is called once per frame
    void Update()
    {
        if (radiusMaterial != null)
        {
            radiusMaterial.SetVector("_Center",transform.position);
            radiusMaterial.SetFloat("_Radius",radius);
            radiusMaterial.SetColor("_RadiusColor",color);
        }
    }
}
