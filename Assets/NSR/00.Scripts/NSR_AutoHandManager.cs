using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Autohand;
using System.Linq;

// 게임오브젝트 찾아서 넣기
public class NSR_AutoHandManager : MonoBehaviourPun
{
    public static NSR_AutoHandManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;

        //해상도 조정
        Screen.SetResolution(960, 640, false);

        //hand_zone_objects = FindObjectsOfType<SH_SyncObj>().Select(o => o.transform).ToArray();
    }

    public List<Transform> hand_zone_objects;

    public GameObject foodSound;
    public AudioSource audioSource;
    public AudioClip[] turnSounds;

    public GameObject changeText;

    public Camera headCamera;
    public Transform forwardFollow;
    public Transform trackingContainer;
    public Transform autoHandPlayer;

    public Image recoderImageInTV;
    public Image speakerImageInTV;

    float speed = 100;
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

    public Transform tv_camera_pos;

    [HideInInspector]
    public bool isMaster;
    [HideInInspector]
    public bool handPlayer;
    [HideInInspector]
    public bool bodyplayer;

    public Camera[] cams;
    public GameObject loadingText;
    int layer;

    public GameObject tv_Canvas;

    [HideInInspector]
    public bool isChanging;
    [HideInInspector]
    public bool changeEnd;

    public KHJ_ScreenFade fade;
    public Draw draw;

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

        tv_camera.gameObject.SetActive(true);
    }

    float currTime;
    float fadeTime;
    [HideInInspector]
    public bool openEye;
    private void Update()
    {
        // 역할 바뀌는 동안 동기화 막기
        if (isChanging)
        {
            currTime += Time.deltaTime;
            if (currTime > 10)
            {
                isChanging = false;
                currTime = 0;
                fadeTime = 0;
            }
        }

        if (openEye)
        {
            fadeTime += Time.deltaTime;
            if (fadeTime > 0.3f)
            {
                fade.EyeOpen_();
                fadeTime = 0;
                openEye = false;
            }
        }

        if (NSR_AutoHandPlayer.instance)
        {
            if (NSR_AutoHandPlayer.instance.endChange)
            {
                draw.LineSet();
                NSR_AutoHandPlayer.instance.endChange = false;
            }
        }

        if (PhotonNetwork.IsConnected == false) return;

        // 카메라 보여지는 오브젝트 설정
        for (int i = 0; i < cams.Length; i++)
        {
            // 각 플레이어마다 보여지는 레이어
            cams[i].cullingMask = layer | (1 << 10) | (1 << 16);
            tv_camera.GetComponent<Camera>().cullingMask = ~(layer | (1 << 10) | (1 << 16));
        }

        //  이거 사용하는 데 있으면 지우고 HandPlayer 변수로 사용
        isMaster = handPlayer;

        // 핸드인 경우
        if (handPlayer)
        {
            //실제 손 세팅
            SetRealHand();

            if (bodyplayer)
                Set_Room_PlayZone();
            else
                Set_Tv_PlayZone();

        }
        //핸드가 아닌 경우
        else
        {
            //가짜 손 세팅
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

        //h = trackingContainer.position;
        //if (sit)
        //    h.y = -0.5f;
        //else
        //    h.y = 0;
        //trackingContainer.position = Vector3.Lerp(trackingContainer.position, h, 50 * Time.deltaTime);
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

        if (isChanging) return;

        NSR_AutoHandPlayer handPlayer = NSR_AutoHandPlayer.instance;

        // Transform 받기(손, 몸, 오브젝트)
        if (handPlayer != null)
        {
            // 꺼져있는 오토핸드 위치 받기
            autoHandPlayer.transform.position = handPlayer.recieve_autoHandPlayer_Pos;
            autoHandPlayer.transform.rotation = handPlayer.recieve_autoHandPlayer_Rot;
            // Tracking 위치 받기
            trackingContainer.position = handPlayer.recieve_trackingContainer_Pos;
            trackingContainer.rotation = handPlayer.recieve_trackingContainer_Rot;

            // 손 위치 받기
            body_hand_R.transform.position = Vector3.Lerp(body_hand_R.transform.position, handPlayer.recieve_hand_R_Pos, speed * Time.deltaTime);
            body_hand_R.transform.rotation = handPlayer.recieve_hand_R_Rot;
            body_hand_L.transform.position = Vector3.Lerp(body_hand_L.transform.position, handPlayer.recieve_hand_L_Pos, speed * Time.deltaTime);
            body_hand_L.transform.rotation = handPlayer.recieve_hand_L_Rot;

            //왼손 손가락 위치 받기
            for (int i = 0; i < 15; i++)
            {
                body_leftFingers[i].transform.localPosition = handPlayer.recieve_left_finger_Pos[i];
                body_leftFingers[i].transform.localRotation = handPlayer.recieve_left_finger_Rot[i];
                body_rightFingers[i].transform.localPosition = handPlayer.recieve_right_finger_Pos[i];
                body_rightFingers[i].transform.localRotation = handPlayer.recieve_right_finger_Rot[i];
            }



            for (int i = 0; i < hand_zone_objects.Count; i++)
            {
                if (hand_zone_objects[i] != null)
                {
                    hand_zone_objects[i].transform.position = handPlayer.recieve_objects_Pos[i];
                    hand_zone_objects[i].transform.rotation = handPlayer.recieve_objects_Rot[i];
                    //hand_zone_objects[i].transform.localScale = handPlayer.recieve_objects_Scale[i];
                }
            }
        }
    }

    void Set_Tv_PlayZone()
    {
        // 카메라에 보여줄 레이어 설정
        layer = (1 << 9);

        // handZone(화면 카메라, 핸드플레이 공간, 카메라렌더러 등) 켜기
        handZone.gameObject.SetActive(true);

        if (isChanging) return;

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

        //if (OVRInput.GetDown(OVRInput.Button.Three))
        //{
        //    photonView.RPC("setHeight", RpcTarget.Others, true);
        //}
        //if (OVRInput.GetUp(OVRInput.Button.Three))
        //{
        //    photonView.RPC("setHeight", RpcTarget.Others, false);
        //}
    }

    private void OnDisable()
    {
        //if (OVRInput.GetDown(OVRInput.Button.Three))
        //    photonView.RPC("setHeight", RpcTarget.Others, false);

        //if (OVRInput.GetDown(OVRInput.Button.Four))
        //    photonView.RPC("HeadLight", RpcTarget.All, false);
    }

    [PunRPC]
    void HeadLight(bool onOff)
    {
        head_light.gameObject.SetActive(onOff);
    }


    Vector3 h;
    bool sit;

    [PunRPC]
    void setHeight(bool down)
    {
        sit = down;
    }

    public void OnGrab(bool left)
    {
        if (PhotonNetwork.IsConnected)
            photonView.RPC("VibrateController", RpcTarget.Others, left);

        VibrateController(left);
    }

    [PunRPC]
    void VibrateController(bool left)
    {
        if (left)
            StartCoroutine(VibrateController(0.05f, 0.3f, 1, OVRInput.Controller.LTouch));
        else
            StartCoroutine(VibrateController(0.05f, 0.3f, 1, OVRInput.Controller.RTouch));
    }

    protected IEnumerator VibrateController(float waitTime, float frequency, float amplitude, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(waitTime);
        OVRInput.SetControllerVibration(0, 0, controller);
    }

    public void FootSound(bool on)
    {
        if(foodSound.activeSelf != on)
        {
            foodSound.SetActive(on);
            if (PhotonNetwork.IsConnected)
                photonView.RPC("Rpc_FootSound", RpcTarget.Others, on);
        }
    }

    [PunRPC]
    void Rpc_FootSound(bool on)
    {
        foodSound.SetActive(on);
    }

    public void TurnSound()
    {
        StartCoroutine(TurnFootSound());
        if (PhotonNetwork.IsConnected)
            photonView.RPC("Rpc_TurnSound", RpcTarget.Others);
    }

    [PunRPC]
    void Rpc_TurnSound()
    {
        StartCoroutine(TurnFootSound());
    }

    IEnumerator TurnFootSound()
    {
        audioSource.PlayOneShot(turnSounds[(int)Random.Range(0, turnSounds.Length)]);
        yield return new WaitForSeconds(0.3f);
        audioSource.PlayOneShot(turnSounds[(int)Random.Range(0, turnSounds.Length)]);
    }

}

