using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_Flashlight : MonoBehaviourPun
{
    public GameObject blacklight;
    KHJ_BlackLight blackLight;
    private void Start()
    {
        blackLight = GetComponent<KHJ_BlackLight>();
    }
    public void Grab(bool on)
    {
        if(blackLight.isBattery)
        photonView.RPC("BlackLight", RpcTarget.All, on);
    }
    [PunRPC]
    void BlackLight(bool on)
    {
        blacklight.SetActive(on);
    }
}
