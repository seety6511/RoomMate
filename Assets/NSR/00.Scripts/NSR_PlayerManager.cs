using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_PlayerManager : MonoBehaviourPun
{
    public static NSR_PlayerManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // BodyPlayer 인지 HandPlayer 인지 결정하는 변수
    public bool bodyControl;
    //public PhotonView myPhotonView;

    public Transform OVRCameraRig;
    public Transform bodyPlayer;
    public Transform handPlayer;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.SendRate = 50;
            PhotonNetwork.SerializationRate = 50;

            // 마스터가 아니면 마스터의 바디 컨트롤 받아서 반대로 설정
        }
    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("ChangeControl", RpcTarget.AllBuffered);
            // 마스터면 바디 컨트롤 게임 매니저에 보내놓기
        }

        // 어떤 컨트롤 인지에 따른 OVRCameraRig의 부모 결정
        if (bodyControl)
        {
            OVRCameraRig.parent = bodyPlayer;
            OVRCameraRig.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            OVRCameraRig.parent = handPlayer;
            OVRCameraRig.localPosition = new Vector3(0, 0, 0);
        }
    }

    [PunRPC]
    void ChangeControl()
    {
        bodyControl = !bodyControl; 
    }


    
}
