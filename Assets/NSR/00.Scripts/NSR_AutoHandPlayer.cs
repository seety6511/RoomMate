using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NSR_AutoHandPlayer : MonoBehaviourPun, IPunObservable
{
    public static NSR_AutoHandPlayer instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public GameObject body;
    void Update()
    {
        // 마스터라면 = handPlayer
        if (PhotonNetwork.IsMasterClient)
        {
            if(NSR_AutoHandManager.instance.hand_L.activeSelf == false)
            {
                NSR_AutoHandManager.instance.hand_L.SetActive(true);
                NSR_AutoHandManager.instance.hand_R.SetActive(true);
                body.SetActive(true);
            }

            if (NSR_AutoBodyPlayer.instance != null)
            {
                body.transform.position = NSR_AutoBodyPlayer.instance.recieve_bodyPos;
                body.transform.rotation = NSR_AutoBodyPlayer.instance.recieve_bodyRot;

                NSR_AutoHandManager.instance.OVRCameraRig.position = body.transform.position;
                NSR_AutoHandManager.instance.OVRCameraRig.rotation = body.transform.rotation;
            }
        }
        else
        {
            if (NSR_AutoHandManager.instance.hand_L.activeSelf)
            {
                NSR_AutoHandManager.instance.hand_L.SetActive(false);
                NSR_AutoHandManager.instance.hand_R.SetActive(false);
                body.SetActive(false);
            }
        }

        // 스페이스바 누르면 컨트롤 바꾸기
        if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            photonView.RPC("ChangeControl", RpcTarget.All);
        }
    }


    [PunRPC]
    void ChangeControl(PhotonMessageInfo info)
    {
        PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());

        Player you = null;
        Player me = null;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                me = PhotonNetwork.PlayerList[i];
            }
            else
            {
                you = PhotonNetwork.PlayerList[i];
            }
        }

        if (me == null || you == null)
            return;

        if (me == NSR_AutoBodyPlayer.instance.photonView.Owner)
        {
            // 내가 바디 -> 핸드
            NSR_AutoBodyPlayer.instance.photonView.TransferOwnership(you);
            NSR_AutoHandPlayer.instance.photonView.TransferOwnership(me);
        }
        else
        {
            // 내가 핸드 -> 바디
            NSR_AutoBodyPlayer.instance.photonView.TransferOwnership(me);
            NSR_AutoHandPlayer.instance.photonView.TransferOwnership(you);
        }
    }

    public Vector3 recieve_hand_L_Pos;
    public Quaternion recieve_hand_L_Rot;
    public Vector3 recieve_hand_R_Pos;
    public Quaternion recieve_hand_R_Rot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(NSR_AutoHandManager.instance.hand_L.transform.position);
            stream.SendNext(NSR_AutoHandManager.instance.hand_L.transform.rotation); 
            stream.SendNext(NSR_AutoHandManager.instance.hand_L.transform.position);
            stream.SendNext(NSR_AutoHandManager.instance.hand_L.transform.rotation);
        }
        if (stream.IsReading)
        {
            recieve_hand_L_Pos = (Vector3)stream.ReceiveNext();
            recieve_hand_L_Rot = (Quaternion)stream.ReceiveNext();
            recieve_hand_R_Pos = (Vector3)stream.ReceiveNext();
            recieve_hand_R_Rot = (Quaternion)stream.ReceiveNext();
        }
    }
}
