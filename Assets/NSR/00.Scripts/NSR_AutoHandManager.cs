using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class NSR_AutoHandManager : MonoBehaviourPun
{
    public static NSR_AutoHandManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;

        //해상도 조정
        Screen.SetResolution(960, 640, false);
    }

    public Camera headCamera;
    public Transform forwardFollow;
    public Transform trackingContainer;

    public Transform trackingSpace;

    // 보이스 관련 이미지
    public Image recoderImageInTV;
    public Image speakerImageInTV;

    public GameObject hand_L;
    public GameObject hand_R;
    public GameObject body;

    public Transform followHandL;
    public Transform followHandR;

    public Transform[] leftFingers;
    public Transform[] rightFingers;

    public Transform[] objects;

    public GameObject hand_zone;
    public GameObject body_zone;
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.SendRate = 50;
            PhotonNetwork.SerializationRate = 50;

            // 마스터는 HandPlayer 마스터가 아니면 BodyPlayer 생성
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate("NSR_Auto_Hand_Player", Vector3.zero, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate("NSR_Auto_Body_Player", Vector3.zero, Quaternion.identity);
            }

            PhotonNetwork.Instantiate("NSR_VoiceView", Vector3.zero, Quaternion.identity);
        }
    }

}
