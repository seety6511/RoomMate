using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Illusion : MonoBehaviour
{
    Renderer render;
    public Material originMat;
    public Material illusionMat;

    public bool on;

    private void Awake()
    {
        render = GetComponent<Renderer>();
        originMat = render.material;
    }

    public void IllusionControl(bool value)
    {
        on = value;
        if (on)
            render.material = originMat;
        else
            render.material = illusionMat;
    }
}
