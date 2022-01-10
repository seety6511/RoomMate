using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SH_PageControl : MonoBehaviourPun
{
    AnimatedBookController abc;

    private void Awake()
    {
        abc = FindObjectOfType<AnimatedBookController>();
    }
    public enum PageDir
    {
        Next,
        Prev,
        Close,
    }
    public PageDir dir;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;

        switch (dir)
        {
            case PageDir.Next:
                if (PhotonNetwork.IsConnected)
                    photonView.RPC("RPC_TurnToNextPage", RpcTarget.All);
                else
                    abc.TurnToNextPage();
                break;

            case PageDir.Prev:
                if (PhotonNetwork.IsConnected)
                    photonView.RPC("RPC_TurnToPreviousPage", RpcTarget.All);
                else
                    abc.TurnToPreviousPage();
                break;
            case PageDir.Close:
                if (PhotonNetwork.IsConnected)
                    photonView.RPC("RPC_CloseBook", RpcTarget.All);
                else
                    abc.CloseBook();
                break;
        }
    }

    [PunRPC]
    void RPC_TurnToNextPage()
    {
        abc.TurnToNextPage();
    }

    [PunRPC]
    void RPC_TurnToPreviousPage()
    {
        abc.TurnToPreviousPage();
    }

    [PunRPC]
    void RPC_CloseBook()
    {
        abc.CloseBook();
    }
}
