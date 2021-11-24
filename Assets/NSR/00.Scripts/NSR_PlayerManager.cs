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

    // BodyPlayer ���� �ƴ��� ���� �����ϴ� ����
    public bool bodyControl;

    //OVRCameraRig �θ�
    public Transform OVRCameraRig;
    public Transform bodyPlayer;
    public Transform handPlayer;

    // ����䰡 �ڱ���� �÷��̾��� �ٵ���Ʈ�� ��Ƶδ� ����
    bool photonViewControl;
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.SendRate = 50;
            PhotonNetwork.SerializationRate = 50;
        }

        // � ��Ʈ�� ������ ���� OVRCameraRig�� �θ� ����
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
        // �����̽��� ������ ��Ʈ�� �ٲٱ�
        if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            photonView.RPC("ChangeControl", RpcTarget.All);
        }

        // 1�� �÷��̾�� 2�� �÷��̾� bodyControl �ݴ�� ���ֱ�
        if (photonView.IsMine)
            photonViewControl = bodyControl;
        else
            bodyControl = !recivePhotonViewControl;

        // � ��Ʈ�� ������ ���� OVRCameraRig�� �θ� ����
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

    // 1�� �÷��̾�� 2�� �÷��̾� bodyControl �ݴ�� ���ֱ� ����
    //1�� �÷��̾�(PhotonView �� �ڱ���� �÷��̾�)�� bodyControl�� ���� ����(photonViewControl)�� �ְ� �ޱ�
    bool recivePhotonViewControl;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //���࿡ �� �� �ִ� ���¶��
        if (stream.IsWriting)
        {
            stream.SendNext(photonViewControl);
        }
        //���࿡ ���� �� �ִ� ���¶��
        if (stream.IsReading)
        {
            recivePhotonViewControl = (bool)stream.ReceiveNext();
        }
    }
}
