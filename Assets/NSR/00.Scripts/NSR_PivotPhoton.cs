using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_PivotPhoton : MonoBehaviourPun
{
    public GameObject[] bodyPivot;
    public GameObject[] handPivot;

    private void Update()
    {
        if (!NSR_AutoHandManager.instance.handPlayer) return;

        for (int i = 0; i < handPivot.Length; i++)
        {
            if (handPivot[i].GetComponent<NSR_Pivot>().active)
            {
                photonView.RPC("SetActivePivot", RpcTarget.All, i, handPivot[i].activeSelf);
                handPivot[i].GetComponent<NSR_Pivot>().active = false;
            }
        }
    }

    [PunRPC]
    void SetActivePivot(int i, bool onOff)
    {
        bodyPivot[i].SetActive(onOff);
    }
}
