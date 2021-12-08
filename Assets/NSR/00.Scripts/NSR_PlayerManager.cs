using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Voice.PUN;

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

    // ���̽� ���� �̹���
    public Image recoderImageInTV;
    public Image speakerImageInTV;
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.SendRate = 50;
            PhotonNetwork.SerializationRate = 50;

            // �����ʹ� HandPlayer �����Ͱ� �ƴϸ� BodyPlayer ����
            if (PhotonNetwork.IsMasterClient)
            {
            }
            else
            {
                PhotonNetwork.Instantiate("NSR_Auto Body Player", new Vector3(15f, 1.6f, -2.56f), Quaternion.identity);
            }

            PhotonNetwork.Instantiate("NSR_VoiceView", Vector3.zero, Quaternion.identity);
        }

    }
}
