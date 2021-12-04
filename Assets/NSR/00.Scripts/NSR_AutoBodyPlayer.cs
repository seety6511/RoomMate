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
    void Update()
    {
        // 마스터가 아니라면 = bodyPlayer
        if (PhotonNetwork.IsMasterClient == false)
        {
            if (NSR_AutoHandManager.instance.body.activeSelf == false)
            {
                hand_L.SetActive(true);
                hand_R.SetActive(true);
                NSR_AutoHandManager.instance.body.SetActive(true);
            }

            if (NSR_AutoHandPlayer.instance != null)
            {
                hand_R.transform.position = NSR_AutoHandPlayer.instance.recieve_hand_R_Pos;
                hand_R.transform.rotation = NSR_AutoHandPlayer.instance.recieve_hand_R_Rot;
                hand_L.transform.position = NSR_AutoHandPlayer.instance.recieve_hand_L_Pos;
                hand_L.transform.rotation = NSR_AutoHandPlayer.instance.recieve_hand_L_Rot;
            }
        }
        else
        {
            if (NSR_AutoHandManager.instance.body.activeSelf)
            {
                hand_L.SetActive(false);
                hand_R.SetActive(false);
                NSR_AutoHandManager.instance.body.SetActive(false);
            }
        }
    }

    public Vector3 recieve_bodyPos;
    public Quaternion recieve_bodyRot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(NSR_AutoHandManager.instance.body.transform.position);
            stream.SendNext(NSR_AutoHandManager.instance.body.transform.rotation);
        }
        if (stream.IsReading)
        {
            recieve_bodyPos = (Vector3)stream.ReceiveNext();
            recieve_bodyRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
