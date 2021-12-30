using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_Pivot : MonoBehaviourPun
{
    public GameObject mainPivot;
    public GameObject samePivot;

    private void OnEnable()
    {
        mainPivot.SetActive(false);
        if(NSR_AutoHandManager.instance.handPlayer)
            photonView.RPC("SetPivot", RpcTarget.Others, true);
    }

    private void OnDisable()
    {
        mainPivot.SetActive(true);
        if (NSR_AutoHandManager.instance.handPlayer)
            photonView.RPC("SetPivot", RpcTarget.Others, false);
    }

    [PunRPC]
    void SetPivot(bool onOff)
    {
        samePivot.SetActive(onOff);
    }

}
