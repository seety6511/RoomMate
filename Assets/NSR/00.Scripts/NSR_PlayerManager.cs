using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NSR_PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static NSR_PlayerManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // BodyPlayer 인지 아닌지 인지 결정하는 변수
    public bool bodyControl;

    //OVRCameraRig 부모
    public Transform OVRCameraRig;
    public Transform bodyPlayer;
    public Transform handPlayer;

    // 포톤뷰가 자기거인 플레이어의 바디컨트롤 담아두는 변수
    bool photonViewControl;
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.SendRate = 50;
            PhotonNetwork.SerializationRate = 50;
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
    void Update()
    {
        // 스페이스바 누르면 컨트롤 바꾸기
        if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            photonView.RPC("ChangeControl", RpcTarget.All);
        }

        // 1번 플레이어와 2번 플레이어 bodyControl 반대로 해주기
        if (photonView.IsMine)
            photonViewControl = bodyControl;
        else
            bodyControl = !recivePhotonViewControl;

        // 어떤 컨트롤 인지에 따른 OVRCameraRig의 부모 결정
        if (bodyControl)
        {
            OVRCameraRig.parent = bodyPlayer;
            OVRCameraRig.localPosition = new Vector3(0, 1.6f, 0);
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

    // 1번 플레이어와 2번 플레이어 bodyControl 반대로 해주기 위한
    //1번 플레이어(PhotonView 가 자기거인 플레이어)의 bodyControl을 담은 변수(photonViewControl)를 주고 받기
    bool recivePhotonViewControl;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //만약에 쓸 수 있는 상태라면
        if (stream.IsWriting)
        {
            stream.SendNext(photonViewControl);
        }
        //만약에 읽을 수 있는 상태라면
        if (stream.IsReading)
        {
            recivePhotonViewControl = (bool)stream.ReceiveNext();
        }
    }
}
