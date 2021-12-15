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
        var property = bulbMat.GetColor("Emission");

        if (lightObj.activeSelf)
        {
            if (property != null)
                bulbMat.SetColor("Emission", Color.white);
            else
                bulbMat.color = Color.white;
        }
        else
        {
            if (property != null)
                bulbMat.SetColor("Emission", Color.black);
            else
                bulbMat.color = Color.black;
        }
    }
}
