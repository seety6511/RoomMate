using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NSR_GameManager : MonoBehaviourPunCallbacks
{
    static public NSR_GameManager instance;

    public bool useVR;

    //³ªÀÇ Æ÷ÅæView
    public PhotonView myPhotonView;

    public bool bodyPlayer;

    private void Awake()
    {
        if (instance == null)   instance = this;
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (useVR)
                PhotonNetwork.Instantiate("VRPlayer", Vector3.zero, Quaternion.identity);
            else
                PhotonNetwork.Instantiate("TestPlayer", Vector3.zero, Quaternion.identity);
        }
    }

    public bool changeBody;
    public void ClickBody()
    {
        bodyPlayer = true;
        changeBody = true;
    }

    public void ClickHand()
    {
        bodyPlayer = false;
        changeBody = true;
    }

}
