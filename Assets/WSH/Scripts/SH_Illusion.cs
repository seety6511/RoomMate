using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Illusion : MonoBehaviour
{
    Renderer render;
    public Material originMat;
    public Material illusionMat;

    public float noiseSize = 0.48f;
    public float noiseSpeed = 0.07f;
    public bool on;
    public bool disappear;

    private void Awake()
    {
        render = GetComponent<Renderer>();
        originMat = render.material;
    }
    public void DisappearingControl(bool value)
    {
        disappear = value;
        IllusionControl(true);
        render.material.SetInt("_UseAlphaClipping", value ? 1 : 0);
    }

    public void NoiseSizeControl(float value)
    {
        IllusionControl(true);
        noiseSize += value;
        render.material.SetFloat("_SphereRadius", noiseSize);
    }

    public void NoiseSpeedControl(float value)
    {
        IllusionControl(true);
        noiseSpeed += value;
        render.material.SetFloat("_SphereSpeed", noiseSpeed);
    }

    public void IllusionControl(bool value)
    {
        on = value;
        if (!on)
            render.material = render.material != originMat ? originMat : null;
        else
        {
            render.material = render.material != illusionMat ? illusionMat : null;
            render.material.SetFloat("_SphereRadius", noiseSize);
            render.material.SetFloat("_SphereSpeed", noiseSpeed);
            render.material.SetInt("_UseAlphaClipping", disappear ? 1 : 0);
        }
    }
}
