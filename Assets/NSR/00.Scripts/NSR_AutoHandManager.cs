using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Autohand;

public class NSR_AutoHandManager : MonoBehaviourPun
{
    public static NSR_AutoHandManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;

        //해상도 조정
        Screen.SetResolution(960, 640, false);
    }

    #region 
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

    public Transform tv_camera;
    public GameObject head_light;

    public GameObject body_hand_R;
    public GameObject body_hand_L;

    public Transform[] body_leftFingers;
    public Transform[] body_rightFingers;

    public Transform[] hand_zone_objects;
    public Transform tv_camera_pos;

    public bool isMaster;
    #endregion
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
        // 마스터라면 = handPlayer
        if (PhotonNetwork.IsMasterClient)
        {
            isMaster = true;
            // 화면 카메라 켜기
            if (tv_camera.gameObject.activeSelf == false)
            {
                tv_camera.gameObject.SetActive(true);
            }

            if(auto_hand_player.activeSelf == false)
            {
                auto_hand_player.SetActive(true);
            }

            // 손이 켜기
            if (hand_L.activeSelf == false)
            {
                hand_L.SetActive(true);
                hand_R.SetActive(true);
            }

            // 바디 손 끄기
            if (body_hand_L.activeSelf)
            {
                body_hand_L.SetActive(false);
                body_hand_R.SetActive(false);
            }

            for(int i = 0; i < hand_zone_objects.Length; i++)
            {
                Grabbable grabbable = hand_zone_objects[i].GetComponent<Grabbable>();
                if (grabbable != null)
                {
                    grabbable.enabled = true;
                }
            }

            if (NSR_AutoBodyPlayer.instance != null)
            {
                // 해드라이팅 인풋 받기
                if (NSR_AutoBodyPlayer.instance.recieve_lightInput)
                {
                    head_light.gameObject.SetActive(true);
                }
                else
                {
                    head_light.gameObject.SetActive(false);
                }

                // 화면 카메라 위치 받기
                tv_camera.position = NSR_AutoBodyPlayer.instance.recieve_tv_camera_pos;
                tv_camera.rotation = NSR_AutoBodyPlayer.instance.recieve_tv_camera_Rot;
            }
            else
            {
                head_light.gameObject.SetActive(false);
            }

        }
        // bodyPlayer
        else
        {
            isMaster = false;

            // 화면 카메라 끄기
            if (tv_camera.gameObject.activeSelf == true)
                tv_camera.gameObject.SetActive(false);

            // 손 이랑 오토핸드 켜져있으면 끄기
            if (hand_L.activeSelf)
            {
                hand_L.SetActive(false);
                hand_R.SetActive(false);
            }

            if (auto_hand_player.activeSelf)
            {
                auto_hand_player.SetActive(false);
            }

            // 바디 손 켜기
            body_hand_L.SetActive(true);
            body_hand_R.SetActive(true);

            for (int i = 0; i < hand_zone_objects.Length; i++)
            {
                Grabbable grabbable = hand_zone_objects[i].GetComponent<Grabbable>();
                if (grabbable != null)
                {
                    grabbable.enabled = false;
                }
            }

            // 해드라이팅 켜고 끄기
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
                // 꺼져있는 오토핸드 위치 받기
                autoHandPlayer.transform.position = NSR_AutoHandPlayer.instance.recieve_autoHandPlayer_Pos;
                autoHandPlayer.transform.rotation = NSR_AutoHandPlayer.instance.recieve_autoHandPlayer_Rot;
                // Tracking 위치 받기
                trackingContainer.position = NSR_AutoHandPlayer.instance.recieve_trackingContainer_Pos;
                trackingContainer.rotation = NSR_AutoHandPlayer.instance.recieve_trackingContainer_Rot;
                // 손 위치 받기
                body_hand_R.transform.position = NSR_AutoHandPlayer.instance.recieve_hand_R_Pos;
                body_hand_R.transform.rotation = NSR_AutoHandPlayer.instance.recieve_hand_R_Rot;
                body_hand_L.transform.position = NSR_AutoHandPlayer.instance.recieve_hand_L_Pos;
                body_hand_L.transform.rotation = NSR_AutoHandPlayer.instance.recieve_hand_L_Rot;
                //손가락 위치 받기
                for (int i = 0; i < 15; i++)
                {
                    body_leftFingers[i].transform.position = NSR_AutoHandPlayer.instance.recieve_left_finger_Pos[i];
                    body_leftFingers[i].transform.rotation = NSR_AutoHandPlayer.instance.recieve_left_finger_Rot[i];
                    body_rightFingers[i].transform.position = NSR_AutoHandPlayer.instance.recieve_right_finger_Pos[i];
                    body_rightFingers[i].transform.rotation = NSR_AutoHandPlayer.instance.recieve_right_finger_Rot[i];
                }
                //오브젝트 위치 받기
                for (int i = 0; i < hand_zone_objects.Length; i++)
                {
                    if(hand_zone_objects[i] != null)
                    {
                        hand_zone_objects[i].transform.position = NSR_AutoHandPlayer.instance.recieve_objects_Pos[i];
                        hand_zone_objects[i].transform.rotation = NSR_AutoHandPlayer.instance.recieve_objects_Rot[i];
                        hand_zone_objects[i].transform.localScale = NSR_AutoHandPlayer.instance.recieve_objects_Scale[i];
                    }
                }
            }
        }
    }
}
