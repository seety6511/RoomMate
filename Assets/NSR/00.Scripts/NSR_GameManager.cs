using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NSR_GameManager : MonoBehaviourPunCallbacks
{
    static public NSR_GameManager instance;

    public bool bodyPlayer;
    public bool handPlayer;

    // 마스터의 바디컨트롤 담아두는 변수
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        if (NSR_GameManager.instance.bodyPlayer)
        {
            photonView.RPC("BodyPlayerHere", RpcTarget.AllBuffered, false);
        }
        else if (NSR_GameManager.instance.handPlayer)
        {
            photonView.RPC("HandPlayerHere", RpcTarget.AllBuffered, false);
        }
    }

    [PunRPC]
    void BodyPlayerHere(bool here)
    {
        NSR_GameManager.instance.bodyPlayer = here;
    }

    [PunRPC]
    void HandPlayerHere(bool here)
    {
        NSR_GameManager.instance.handPlayer = here;
    }
}
