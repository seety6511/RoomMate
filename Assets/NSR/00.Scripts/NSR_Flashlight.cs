using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_Flashlight : MonoBehaviourPun
{
    public GameObject blacklight;
    public void Grab(bool on)
    {
        photonView.RPC("BlackLight", RpcTarget.All, on);
    }
    [PunRPC]
    void BlackLight(bool on)
    {
        blacklight.SetActive(on);
    }
}
