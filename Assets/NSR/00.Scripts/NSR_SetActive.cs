using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_SetActive : MonoBehaviourPun
{
    public GameObject trueObj;
    public GameObject falseObj;

    public void SetActiveTrue()
    {
        Rpc_SetActiveTrue();
        photonView.RPC("Rpc_SetActive()", RpcTarget.Others);
    }

    public void SetActiveFalse()
    {
        Rpc_SetActiveFalse();
        photonView.RPC("Rpc_SetActiveFalse", RpcTarget.Others);
    }

    [PunRPC]
    void Rpc_SetActiveTrue()
    {
        trueObj.SetActive(true);
    }

    [PunRPC]
    void Rpc_SetActiveFalse()
    {
        falseObj.SetActive(false);
    }
}
