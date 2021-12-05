using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_AutoBodyPlayer : MonoBehaviourPun, IPunObservable
{
    public static NSR_AutoBodyPlayer instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public GameObject hand_R;
    public GameObject hand_L;

    public Vector3 RplayerPos;
    public Quaternion RPlayerRot;

    public Transform[] leftFingers;
    public Transform[] rightFingers;

    public Transform[] objects;
    void Update()
    {
        // 마스터가 아니라면 = bodyPlayer
        if (PhotonNetwork.IsMasterClient == false)
        {

            hand_L.SetActive(true);
            hand_R.SetActive(true);


            if (NSR_AutoHandPlayer.instance != null)
            {
                // 손 위치 받기
                hand_R.transform.position = NSR_AutoHandPlayer.instance.recieve_hand_R_Pos;
                hand_R.transform.rotation = NSR_AutoHandPlayer.instance.recieve_hand_R_Rot;
                hand_L.transform.position = NSR_AutoHandPlayer.instance.recieve_hand_L_Pos;
                hand_L.transform.rotation = NSR_AutoHandPlayer.instance.recieve_hand_L_Rot;

                //손가락 위치 받기
                for (int i = 0; i < leftFingers.Length; i++)
                {
                    leftFingers[i].transform.position = NSR_AutoHandPlayer.instance.recieve_left_finger_Pos[i];
                    leftFingers[i].transform.rotation = NSR_AutoHandPlayer.instance.recieve_left_finger_Rot[i];
                    rightFingers[i].transform.position = NSR_AutoHandPlayer.instance.recieve_right_finger_Pos[i];
                    rightFingers[i].transform.rotation = NSR_AutoHandPlayer.instance.recieve_right_finger_Rot[i];
                }
                //오브젝트 위치 받기
                for (int i = 0; i < objects.Length; i++)
                {
                    leftFingers[i].transform.position = NSR_AutoHandPlayer.instance.recieve_objects_Pos[i];
                    leftFingers[i].transform.rotation = NSR_AutoHandPlayer.instance.recieve_objects_Rot[i];
                }
            }
        }
        else
        {
            hand_L.SetActive(false);
            hand_R.SetActive(false);
        }
    }

    [HideInInspector]
    public Vector3 recieve_bodyPos;
    [HideInInspector]
    public Quaternion recieve_bodyRot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(NSR_AutoHandManager.instance.trackingSpace.transform.position);
            stream.SendNext(NSR_AutoHandManager.instance.trackingSpace.transform.rotation);
        }
        if (stream.IsReading)
        {
            recieve_bodyPos = (Vector3)stream.ReceiveNext();
            recieve_bodyRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
