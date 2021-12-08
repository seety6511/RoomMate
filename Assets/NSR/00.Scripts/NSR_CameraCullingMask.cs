using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_CameraCullingMask : MonoBehaviourPun
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    int layer;
    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            layer = 1 << 9;
        }
        else
        {
            layer = ~(1 << 9);
        }
        cam.cullingMask = layer;
    }
}
