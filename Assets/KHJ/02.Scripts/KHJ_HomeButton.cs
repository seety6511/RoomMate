using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KHJ_HomeButton : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;

        photonView.RPC("OnTrigger", RpcTarget.All);
        OnTrigger();
    }

    [PunRPC]
    public void OnTrigger()
    {
        if (KHJ_SmartPhone.instance.IsRunningApp && KHJ_SmartPhone.instance.IsSolved)
        {
            KHJ_SmartPhone.instance.EndApp();
        }
    }
}
