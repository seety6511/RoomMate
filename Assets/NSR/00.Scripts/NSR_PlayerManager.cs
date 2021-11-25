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

        //�ػ� ����
        Screen.SetResolution(960, 640, false);
    }

    //OVRCameraRig
    public Transform OVRCameraRig;

    public Transform CenterEyeAnchor;

    public Transform LeftHandAnchor;
    public Transform RightHandAnchor;

    // �÷��̾� ���Դ��� Ȯ���ϴ� ����
    public bool BodyIn;
    public bool HandIn;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.SendRate = 50;
            PhotonNetwork.SerializationRate = 50;

            // �����ʹ� BodyPlayer �����Ͱ� �ƴϸ� HandPlayer ����
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate("NSR_BodyPlayer", Vector3.zero, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate("NSR_HandPlayer", new Vector3(15f, 1.6f, -2.56f), Quaternion.identity);
            }
        }
    }
}
