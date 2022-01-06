using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

// ���ӿ�����Ʈ ã�Ƽ� �ֱ�
public class NSR_AutoHandManager : MonoBehaviourPun
{
    public static NSR_AutoHandManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;

        //�ػ� ����
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

            // �����ʹ� HandPlayer �����Ͱ� �ƴϸ� BodyPlayer ����
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
        // ���� �ٲ�� ���� �۵� ����
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

        // ī�޶� �������� ������Ʈ ����
        for (int i = 0; i < cams.Length; i++)
        {
            cams[i].cullingMask = layer;
            tv_camera.GetComponent<Camera>().cullingMask = ~layer;
        }

        //  �̰� ����ϴ� �� ������ ����� HandPlayer ������ ���
        isMaster = handPlayer;

        // �ڵ��� ���
        if (handPlayer)
        {
            SetHandPlayer();

            if (bodyplayer)
                Set_Room_PlayZone();
            else
                Set_Tv_PlayZone();

        }
        //�ڵ尡 �ƴ� ���
        else
        {
            SetBodyPlayer();

            if (bodyplayer)
                Set_Room_PlayZone();
            else
                Set_Tv_PlayZone();
        }
    }

    void SetHandPlayer()
    {
        // �����ڵ� �� �ѱ�
        auto_hand_player.SetActive(true);
        // �����ڵ� �� �ѱ�
        hand_L.SetActive(true);
        hand_R.SetActive(true);

        // �ٵ� �� ����
        body_hand_L.SetActive(false);
        body_hand_R.SetActive(false);
    }

    void SetBodyPlayer()
    {
        //�����ڵ� �� ����
        auto_hand_player.SetActive(false);
        //�����ڵ� �� ����
        hand_L.SetActive(false);
        hand_R.SetActive(false);

        // �ٵ� �� �ѱ�
        body_hand_L.SetActive(true);
        body_hand_R.SetActive(true);

        // Transform �ޱ�(��, ��, ������Ʈ)
        if (NSR_AutoHandPlayer.instance != null)
        {
            // �����ִ� �����ڵ� ��ġ �ޱ�
            autoHandPlayer.transform.position = NSR_AutoHandPlayer.instance.recieve_autoHandPlayer_Pos;
            autoHandPlayer.transform.rotation = NSR_AutoHandPlayer.instance.recieve_autoHandPlayer_Rot;
            // Tracking ��ġ �ޱ�
            trackingContainer.position = NSR_AutoHandPlayer.instance.recieve_trackingContainer_Pos;
            trackingContainer.rotation = NSR_AutoHandPlayer.instance.recieve_trackingContainer_Rot;

            // �� ��ġ �ޱ�
            body_hand_R.transform.position = Vector3.Lerp(body_hand_R.transform.position, NSR_AutoHandPlayer.instance.recieve_hand_R_Pos, speed * Time.deltaTime);
            body_hand_R.transform.rotation = Quaternion.Lerp(body_hand_R.transform.rotation, NSR_AutoHandPlayer.instance.recieve_hand_R_Rot, speed * Time.deltaTime);
            body_hand_L.transform.position = Vector3.Lerp(body_hand_L.transform.position, NSR_AutoHandPlayer.instance.recieve_hand_L_Pos, speed * Time.deltaTime);
            body_hand_L.transform.rotation = Quaternion.Lerp(body_hand_L.transform.rotation, NSR_AutoHandPlayer.instance.recieve_hand_L_Rot, speed * Time.deltaTime);

            //�޼� �հ��� ��ġ �ޱ�
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
        // ī�޶� ������ ���̾� ����
        layer = 1 << 9;

        // handZone(ȭ�� ī�޶�, �ڵ��÷��� ����, ī�޶󷻴��� ��) �ѱ�
        handZone.gameObject.SetActive(true);

        if (NSR_AutoBodyPlayer.instance != null)
        {
            // �ص������ ��ǲ �ޱ�
            if (NSR_AutoBodyPlayer.instance.recieve_lightInput)
            {
                head_light.gameObject.SetActive(true);
            }
            else
            {
                head_light.gameObject.SetActive(false);
            }

            // ȭ�� ī�޶� ��ġ �ޱ�
            tv_camera.position = Vector3.Lerp(tv_camera.position, NSR_AutoBodyPlayer.instance.recieve_tv_camera_pos, 200 * Time.deltaTime);
            tv_camera.rotation = Quaternion.Lerp(tv_camera.rotation, NSR_AutoBodyPlayer.instance.recieve_tv_camera_Rot, 200 * Time.deltaTime); ;


            tv_Canvas.gameObject.SetActive(true);
            loadingText.SetActive(false);
        }
        else
        {
            head_light.gameObject.SetActive(false);

            tv_Canvas.gameObject.SetActive(false);
            loadingText.SetActive(true);
        }
    }

    void Set_Room_PlayZone()
    {
        // ī�޶� ������ ���̾� ����
        layer = ~(1 << 9);

        // ȭ�� ī�޶� ����
        handZone.gameObject.SetActive(false);

        // �ص������ �Ѱ� ����
        if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            head_light.gameObject.SetActive(true);
        }
        else
        {
            head_light.gameObject.SetActive(false);
        }
    }
}

