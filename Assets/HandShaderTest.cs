using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandShaderTest : MonoBehaviour
{
 public   SkinnedMeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        Material[] mats = meshRenderer.materials;
        mats[1].SetFloat("_FillPercent", 0.5f);

    }

    // Update is called once per frame
    void Update()
    {


    }
}
