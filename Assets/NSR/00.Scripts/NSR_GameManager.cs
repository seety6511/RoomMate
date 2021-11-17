using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NSR_GameManager : MonoBehaviourPunCallbacks
{
    static public NSR_GameManager instance;

    public bool useVR;

    //���� ����View
    public PhotonView myPhotonView;
    public PhotonView otherPhotonView;

    //public bool bodyPlayer;
    public int countPlayer = 0;
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

    //public bool changeBody;
    public void ClickChangeBody()
    {
        myPhotonView.RPC("ChangeBody", RpcTarget.AllBuffered);
        //otherPhotonView.RPC("ChangeBody", RpcTarget.AllBuffered);
    }
}
