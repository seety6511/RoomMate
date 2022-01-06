using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Autohand;

// 게임오브젝트 찾아서 넣기
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
    float speed = 100;

    public Camera headCamera;
    public Transform forwardFollow;
    public Transform trackingContainer;
    public Transform trackingSpace;
    public Transform autoHandPlayer;
    public Image recoderImageInTV;
    public Image speakerImageInTV;
    public GameObject hand_L;
    public GameObject hand_R;
    public GameObject auto_hand_player;

    public Transform[] leftFingers;
    public Transform[] rightFingers;

    public Transform tv_camera;
    public GameObject handZone;
    public GameObject head_light;

    public GameObject body_hand_R;
    public GameObject body_hand_L;

    public Transform[] body_leftFingers;
    public Transform[] body_rightFingers;

    public Transform[] hand_zone_objects;
    public Transform tv_camera_pos;

    public bool isMaster;
    public bool handPlayer;
    public bool bodyplayer;

    public Camera[] cams;
    public GameObject loadingText;
    int layer;

    public bool leftCatched;
    public bool rightCatched;

    public GameObject tv_Canvas;

    public bool isChanging;
    #endregion

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.SendRate = 200;
            PhotonNetwork.SerializationRate = 200;

            // 마스터는 HandPlayer 마스터가 아니면 BodyPlayer 생성
            if (PhotonNetwork.IsMasterClient)
            {
                handPlayer = true;
                bodyplayer = false;
                PhotonNetwork.Instantiate("NSR_Auto_Hand_Player", Vector3.zero, Quaternion.identity);
            }
            else
            {
                handPlayer = false;
                bodyplayer = true;
                PhotonNetwork.Instantiate("NSR_Auto_Body_Player", Vector3.zero, Quaternion.identity);
            }

            PhotonNetwork.Instantiate("NSR_VoiceView", Vector3.zero, Quaternion.identity);
        }
    }

    float currTime;
    private void Update()
    {
        // 역할 바뀌는 동안 작동 막기
        if (isChanging)
        {
            currTime += Time.deltaTime;
            if(currTime > 1f)
            {
                isChanging = false;
                //NSR_AutoHandPlayer.instance.canChange = true;
                currTime = 0;
            }
        }
        if (PhotonNetwork.IsConnected == false || isChanging) return;

        // 카메라 보여지는 오브젝트 설정
        for (int i = 0; i < cams.Length; i++)
        {
            cams[i].cullingMask = layer;
            tv_camera.GetComponent<Camera>().cullingMask = ~layer;
        }

        //  이거 사용하는 데 있으면 지우고 HandPlayer 변수로 사용
        isMaster = handPlayer;

        // 핸드인 경우
        if (handPlayer)
        {
            SetRealHand();

            if (bodyplayer)
                Set_Room_PlayZone();
            else
                Set_Tv_PlayZone();

        }
        //핸드가 아닌 경우
        else
        {
            setFakeHand();

            if (bodyplayer)
                Set_Room_PlayZone();
            else
                Set_Tv_PlayZone();
        }
    }

    void SetRealHand()
    {
        // 오토핸드 몸 켜기
        auto_hand_player.SetActive(true);
        // 오토핸드 손 켜기
        hand_L.SetActive(true);
        hand_R.SetActive(true);

        // 바디 손 끄기
        body_hand_L.SetActive(false);
        body_hand_R.SetActive(false);
    }

    void setFakeHand()
    {
        //오토핸드 몸 끄기
        auto_hand_player.SetActive(false);
        //오토핸드 손 끄기
        hand_L.SetActive(false);
        hand_R.SetActive(false);

        // 바디 손 켜기
        body_hand_L.SetActive(true);
        body_hand_R.SetActive(true);

        // Transform 받기(손, 몸, 오브젝트)
        if (NSR_AutoHandPlayer.instance != null)
        {
            // 꺼져있는 오토핸드 위치 받기
            autoHandPlayer.transform.position = NSR_AutoHandPlayer.instance.recieve_autoHandPlayer_Pos;
            autoHandPlayer.transform.rotation = NSR_AutoHandPlayer.instance.recieve_autoHandPlayer_Rot;
            // Tracking 위치 받기
            trackingContainer.position = NSR_AutoHandPlayer.instance.recieve_trackingContainer_Pos;
            trackingContainer.rotation = NSR_AutoHandPlayer.instance.recieve_trackingContainer_Rot;

            // 손 위치 받기
            body_hand_R.transform.position = Vector3.Lerp(body_hand_R.transform.position, NSR_AutoHandPlayer.instance.recieve_hand_R_Pos, speed * Time.deltaTime);
            body_hand_R.transform.rotation = Quaternion.Lerp(body_hand_R.transform.rotation, NSR_AutoHandPlayer.instance.recieve_hand_R_Rot, speed * Time.deltaTime);
            body_hand_L.transform.position = Vector3.Lerp(body_hand_L.transform.position, NSR_AutoHandPlayer.instance.recieve_hand_L_Pos, speed * Time.deltaTime);
            body_hand_L.transform.rotation = Quaternion.Lerp(body_hand_L.transform.rotation, NSR_AutoHandPlayer.instance.recieve_hand_L_Rot, speed * Time.deltaTime);

            //왼손 손가락 위치 받기
            for (int i = 0; i < 15; i++)
            {
                body_leftFingers[i].transform.localPosition = NSR_AutoHandPlayer.instance.recieve_left_finger_Pos[i];
                body_leftFingers[i].transform.localRotation = NSR_AutoHandPlayer.instance.recieve_left_finger_Rot[i];
                body_rightFingers[i].transform.localPosition = NSR_AutoHandPlayer.instance.recieve_right_finger_Pos[i];
                body_rightFingers[i].transform.localRotation = NSR_AutoHandPlayer.instance.recieve_right_finger_Rot[i];
            }

           
         
            for (int i = 0; i < hand_zone_objects.Length; i++)
            {
                if (hand_zone_objects[i] != null)
                {
                    hand_zone_objects[i].transform.position = NSR_AutoHandPlayer.instance.recieve_objects_Pos[i];
                    hand_zone_objects[i].transform.rotation = NSR_AutoHandPlayer.instance.recieve_objects_Rot[i];
                    //hand_zone_objects[i].transform.localScale = NSR_AutoHandPlayer.instance.recieve_objects_Scale[i];
                }
            }
        }
    }

    void Set_Tv_PlayZone()
    {
        // 카메라에 보여줄 레이어 설정
        layer = 1 << 9;

        // handZone(화면 카메라, 핸드플레이 공간, 카메라렌더러 등) 켜기
        handZone.gameObject.SetActive(true);

        if (NSR_AutoBodyPlayer.instance != null)
        {
          // 화면 카메라 위치 받기
            tv_camera.position = Vector3.Lerp(tv_camera.position, NSR_AutoBodyPlayer.instance.recieve_tv_camera_pos, 200 * Time.deltaTime);
            tv_camera.rotation = Quaternion.Lerp(tv_camera.rotation, NSR_AutoBodyPlayer.instance.recieve_tv_camera_Rot, 200 * Time.deltaTime);

            tv_Canvas.gameObject.SetActive(true);
            loadingText.SetActive(false);
        }
        else
        {
            tv_Canvas.gameObject.SetActive(false);
            loadingText.SetActive(true);
        }
    }

    void Set_Room_PlayZone()
    {
        // 카메라에 보여줄 레이어 설정
        layer = ~(1 << 9);

        // 화면 카메라 끄기
        handZone.gameObject.SetActive(false);

        tv_camera.position = Vector3.Lerp(tv_camera.position, tv_camera_pos.position, 200 * Time.deltaTime);
        tv_camera.rotation = Quaternion.Lerp(tv_camera.rotation, tv_camera_pos.rotation, 200 * Time.deltaTime);

        // 해드라이팅 켜고 끄기
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            photonView.RPC("HeadLight", RpcTarget.All, true);
            //head_light.gameObject.SetActive(true);
        }
        if (OVRInput.GetUp(OVRInput.Button.Four))
        {
            photonView.RPC("HeadLight", RpcTarget.All, false);
            //head_light.gameObject.SetActive(false);
        }

        if(OVRInput.GetDown(OVRInput.Button.Three))
        {
            photonView.RPC("setHeight", RpcTarget.Others, true);
        }
        if(OVRInput.GetUp(OVRInput.Button.Three))
        {
            photonView.RPC("setHeight", RpcTarget.Others, false);
        }
    }

    private void OnDisable()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
            photonView.RPC("setHeight", RpcTarget.Others, false);

        if (OVRInput.GetDown(OVRInput.Button.Four))
            photonView.RPC("HeadLight", RpcTarget.All, false);
    }

    [PunRPC]
    void HeadLight(bool onOff)
    {
        head_light.gameObject.SetActive(onOff);
    }


    Vector3 h;

    [PunRPC]
    void setHeight(bool sit)
    {
        h = trackingContainer.position;
        if (sit)
            h.y = -0.5f;
        else
            h.y = 0;
        trackingContainer.position = h;
    }
}

