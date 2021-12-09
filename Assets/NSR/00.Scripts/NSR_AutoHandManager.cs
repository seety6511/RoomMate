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

        //�ػ� ����
        Screen.SetResolution(960, 640, false);
    }

    public Camera headCamera;
    public Transform OVRCameraRig;

    public Transform trackingSpace;

    // ���̽� ���� �̹���
    public Image recoderImageInTV;
    public Image speakerImageInTV;

    // HandPlayer
    public GameObject hand_L;
    public GameObject hand_R;
    public Transform autoHandPlayer;

    public Transform[] leftFingers;
    public Transform[] rightFingers;

    // handAnchor
    public Transform followHandL;
    public Transform followHandR;

    public GameObject hand_zone;
    public GameObject body_zone;
    public Transform[] hand_zone_objects;
    public Transform[] body_zone_objects;

    public GameObject autoHandPlayerContainer;

    public Transform tv_camera;
    public GameObject head_light;

    //bodyPlayer
    public GameObject body_hand_R;
    public GameObject body_hand_L;

    public Transform[] body_leftFingers;
    public Transform[] body_rightFingers;
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.SendRate = 50;
            PhotonNetwork.SerializationRate = 50;

            // �����ʹ� HandPlayer �����Ͱ� �ƴϸ� BodyPlayer ����
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
        //�����Ͷ�� (HandPlayer)
        if (PhotonNetwork.IsMasterClient)
        {
            // handPlayer �� �Ѱ� BodyPlayer �� ����
            hand_L.SetActive(true);
            hand_R.SetActive(true);
            autoHandPlayer.gameObject.SetActive(true);
            hand_zone.SetActive(true);

            body_hand_R.SetActive(false);
            body_hand_L.SetActive(false);
            body_zone.SetActive(false);

            // AutoHandPlayer �θ� ���ֱ�
            if (autoHandPlayerContainer.transform.childCount > 0)
            {
                autoHandPlayer.parent = null;
            }

            if (NSR_AutoBodyPlayer.instance != null)
            {

                // �ص����Ʈ
                if (NSR_AutoBodyPlayer.instance.recieve_lightInput)
                    head_light.gameObject.SetActive(true);
                else
                    head_light.gameObject.SetActive(false);
                // tv ī�޶� �ѱ�
                if (tv_camera.gameObject.activeSelf == false)
                {
                    tv_camera.gameObject.SetActive(true);
                }
                tv_camera.position = NSR_AutoBodyPlayer.instance.recieve_headCamera_Pos;
                tv_camera.rotation = NSR_AutoBodyPlayer.instance.recieve_headCamera_Rot;
            }
            else
            {
                head_light.gameObject.SetActive(false);
            }


        }
        // �����Ͱ� �ƴ϶�� (BodyPlayer)
        else
        {
            // HandPlayer �� ���� BodyPlayer �� �ѱ�
            hand_L.SetActive(false);
            hand_R.SetActive(false);
            autoHandPlayer.gameObject.SetActive(false);
            hand_zone.SetActive(false);
            tv_camera.gameObject.SetActive(false);

            body_hand_R.SetActive(true);
            body_hand_L.SetActive(true);
            body_zone.SetActive(true);

            if (NSR_AutoHandPlayer.instance != null)
            {
                // OVRCameraRig ��ġ �ޱ�
                OVRCameraRig.transform.position = NSR_AutoHandPlayer.instance.recieve_trackingContainer_Pos;
                OVRCameraRig.transform.rotation = NSR_AutoHandPlayer.instance.recieve_trackingContainer_Rot;

                //������Ʈ ��ġ �ޱ�
                for (int i = 0; i < NSR_AutoHandPlayer.instance.recieve_objects_Pos.Length; i++)
                {
                    body_zone_objects[i].transform.position = NSR_AutoHandPlayer.instance.recieve_objects_Pos[i];
                    body_zone_objects[i].transform.rotation = NSR_AutoHandPlayer.instance.recieve_objects_Rot[i];
                }
                // �� ��ġ �ޱ�
                body_hand_R.transform.position = NSR_AutoHandPlayer.instance.recieve_hand_R_Pos;
                body_hand_R.transform.rotation = NSR_AutoHandPlayer.instance.recieve_hand_R_Rot;
                body_hand_L.transform.position = NSR_AutoHandPlayer.instance.recieve_hand_L_Pos;
                body_hand_L.transform.rotation = NSR_AutoHandPlayer.instance.recieve_hand_L_Rot;

                //�հ��� ��ġ �ޱ�
                for (int i = 0; i < 15; i++)
                {
                    body_leftFingers[i].transform.position = NSR_AutoHandPlayer.instance.recieve_left_finger_Pos[i];
                    body_leftFingers[i].transform.rotation = NSR_AutoHandPlayer.instance.recieve_left_finger_Rot[i];
                    body_rightFingers[i].transform.position = NSR_AutoHandPlayer.instance.recieve_right_finger_Pos[i];
                    body_rightFingers[i].transform.rotation = NSR_AutoHandPlayer.instance.recieve_right_finger_Rot[i];
                }

                // AutoHandPlayerContainer �θ�� �ϰ� ��ġ �ޱ�
                if (autoHandPlayer.parent == null)
                {
                    autoHandPlayer.parent = autoHandPlayerContainer.transform;
                    autoHandPlayer.localPosition = Vector3.zero;
                    autoHandPlayer.localRotation = Quaternion.identity;
                }
                else
                {
                    autoHandPlayerContainer.transform.position = NSR_AutoHandPlayer.instance.recieve_autoHandPlayer_Pos;
                    autoHandPlayerContainer.transform.rotation = NSR_AutoHandPlayer.instance.recieve_autoHandPlayer_Rot;
                }


            }
            else
            {

            }
        }
    }

}
