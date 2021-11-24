using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_PlayerManager : MonoBehaviourPun, IPunObservable
{
    public static NSR_PlayerManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // BodyPlayer ���� �ƴ��� ���� �����ϴ� ����
    //public bool bodyControl;

    //OVRCameraRig �θ�
    public Transform OVRCameraRig;


    // ����䰡 �ڱ���� �÷��̾��� �ٵ���Ʈ�� ��Ƶδ� ����
    bool photonViewControl;
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.SendRate = 50;
            PhotonNetwork.SerializationRate = 50;
        }

    }
    void Update()
    {


        //// 1�� �÷��̾�� 2�� �÷��̾� bodyControl �ݴ�� ���ֱ�
        //if (photonView.IsMine)
        //    photonViewControl = PhotonNetwork.IsMasterClient;
        //else
        //    bodyControl = !recivePhotonViewControl;

        // � ��Ʈ�� ������ ���� OVRCameraRig�� �θ� ����
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    OVRCameraRig.parent = NSR_BodyPlayer.instance.transform;
        //    OVRCameraRig.localPosition = new Vector3(0, 1.6f, 0);
        //}
        //else
        //{
        //    OVRCameraRig.parent = NSR_HandPlayer.instance.transform;
        //    OVRCameraRig.localPosition = new Vector3(0, 1.6f, 0);
        //}
    }


    // 1�� �÷��̾�� 2�� �÷��̾� bodyControl �ݴ�� ���ֱ� ����
    //1�� �÷��̾�(PhotonView �� �ڱ���� �÷��̾�)�� bodyControl�� ���� ����(photonViewControl)�� �ְ� �ޱ�
    bool recivePhotonViewControl;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //���࿡ �� �� �ִ� ���¶��
        if (stream.IsWriting)
        {
            print("");
            stream.SendNext(photonViewControl);
        }
        //���࿡ ���� �� �ִ� ���¶��
        if (stream.IsReading)
        {
            recivePhotonViewControl = (bool)stream.ReceiveNext();
        }
    }
}
