using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HandShaderTest : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public Material[] Ghostshaders;
    public GameObject HandMat;
    void Start()
    {
        Material[] mats = meshRenderer.materials;
        mats[0].SetFloat("_FillPercent", 1.5f);
    }
    public bool tmp = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            print("Change");
            //Material[] mats = meshRenderer.materials;
            //mats[0].SetFloat("_FillPercent", 0.5f);
            // meshRenderer.material = Ghostshaders[1];
            // block = new MaterialPropertyBlock();
            //meshRenderer.SetPropertyBlock(block);
            //meshRenderer.material.SetFloat("_FillPercent", 0);
            meshRenderer.materials[0].DOFloat(0, "_FillPercent", 2);
            //DOTween.To(() => meshRenderer.materials[0].GetFloat("_FillPercent"), x => meshRenderer.materials[0].SetFloat("_FillPercent", x), 1.3, 1);
        }
    }
}
