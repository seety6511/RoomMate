using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class KHJ_ButtonUI : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;

        if (PhotonNetwork.IsConnected)
            photonView.RPC("OnClick", RpcTarget.All);
        else
            OnClick();

    }
    [PunRPC]
    public void OnClick()
    {
        GetComponent<Button>().onClick.Invoke();
    }
}
