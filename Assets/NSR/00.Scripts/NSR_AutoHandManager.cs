using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Autohand.Demo;

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

    public Transform autoHandPlayer;

    // 보이스 관련 이미지
    public Image recoderImageInTV;
    public Image speakerImageInTV;

    public GameObject hand_L;
    public GameObject hand_R;
    public GameObject auto_hand_player;

    public Transform followHandL;
    public Transform followHandR;

    public Transform[] leftFingers;
    public Transform[] rightFingers;

    public GameObject hand_zone;
    public GameObject body_zone;

    public Transform[] hand_zone_objects;
    public Transform[] body_zone_objects;

    public GameObject autoHandPlayerContainer;

    public Transform tv_camera;
    public GameObject head_light;
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

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (tv_camera.gameObject.activeSelf == false)
            {
                tv_camera.gameObject.SetActive(true);
            }

            if (NSR_AutoBodyPlayer.instance != null)
            {
                if (NSR_AutoBodyPlayer.instance.recieve_lightInput)
                {
                    head_light.gameObject.SetActive(true);
                }
                else
                {
                    head_light.gameObject.SetActive(false);
                }
            }
            else
            {
                head_light.gameObject.SetActive(false);
            }
        }
        else
        {
            if (tv_camera.gameObject.activeSelf == true)
                tv_camera.gameObject.SetActive(false);

            if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                head_light.gameObject.SetActive(true);
            }
            else
            {
                head_light.gameObject.SetActive(false);
            }

            if (NSR_AutoHandPlayer.instance != null)
            {
                autoHandPlayer.transform.position = NSR_AutoHandPlayer.instance.recieve_autoHandPlayer_Pos;
                autoHandPlayer.transform.rotation = NSR_AutoHandPlayer.instance.recieve_autoHandPlayer_Rot;
            }
        }
    }

}
