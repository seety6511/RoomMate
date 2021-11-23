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

    // BodyPlayer ���� HandPlayer ���� �����ϴ� ����
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

            // �����Ͱ� �ƴϸ� �������� �ٵ� ��Ʈ�� �޾Ƽ� �ݴ�� ����
        }
    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("ChangeControl", RpcTarget.AllBuffered);
            // �����͸� �ٵ� ��Ʈ�� ���� �Ŵ����� ��������
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

    [PunRPC]
    void ChangeControl()
    {
        bodyControl = !bodyControl; 
    }


    
}
