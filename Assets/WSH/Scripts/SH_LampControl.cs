using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_LampControl : MonoBehaviour
{
    public GameObject lightObj;
    public Material bulbMat;    

    public void OnOff()
    {
        lightObj.SetActive(!lightObj.activeSelf);
        if (lightObj.activeSelf)
            bulbMat.color = Color.white;
        else
            bulbMat.color = Color.black;
    }
}
