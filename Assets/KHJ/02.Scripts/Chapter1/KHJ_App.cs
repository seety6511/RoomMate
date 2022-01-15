using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KHJ_App : MonoBehaviourPun
{
    public GameObject App;
    public KHJ_SmartPhone.AppName appName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;

        if (PhotonNetwork.IsConnected)
            photonView.RPC("Run_App_Trigger", RpcTarget.All);
        else
            Run_App_Trigger();
    }
    [PunRPC]
    public void Run_App_Trigger()
    {
        if (!KHJ_SmartPhone.instance.IsSolved)
        {
            return;
        }
        if(!KHJ_SmartPhone.instance.IsRunningApp)
            KHJ_SmartPhone.instance.StartApp(gameObject);
    }

}
