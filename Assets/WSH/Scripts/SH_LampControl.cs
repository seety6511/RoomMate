using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;


public class SH_LampControl : MonoBehaviourPun
{
    public GameObject lightObj;
    public MeshRenderer mesh;
    string proName = "_EmissionColor";

    public void Grab()
    {
        photonView.RPC("OnOff", RpcTarget.All);
    }

    [PunRPC]
    void OnOff()
    {
        lightObj.SetActive(!lightObj.activeSelf);
        var meshMaterials = mesh.materials;

        Material result = null;
        for(int i =0;i< meshMaterials.Count();++i)
        {
            var s = meshMaterials[i].name.Split('_');
            if (s[0] == "Lamp")
            {
                result = meshMaterials[i];
                break;
            }
        }
        var w = result.shader.FindPropertyIndex(proName);
        if (lightObj.activeSelf)
        {
            if (w != -1)
                result.SetColor(proName, Color.white);
            else
                result.color = Color.white;
        }
        else
        {
            if (w != -1)
                result.SetColor(proName, Color.black);
            else
                result.color = Color.black;
        }
    }
}
