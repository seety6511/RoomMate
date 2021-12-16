using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_CameraCullingMask : MonoBehaviourPun
{
    public Camera[] cam;

    int layer;
    void Update()
    {
        if (NSR_AutoHandManager.instance.bodyplayer)
        {
            layer = ~(1 << 9);
        }
        else
        {
            layer = 1 << 9;
        }
        

        for (int i = 0; i < cam.Length; i++)
        {
            cam[i].cullingMask = layer;
        }
    }
}
