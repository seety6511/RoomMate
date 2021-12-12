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

    void Update()
    {
        //// 마스터라면 = handPlayer
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    // 손이 꺼져있으면 켜기
        //    if (NSR_AutoHandManager.instance.hand_L.activeSelf == false)
        //    {
        //        NSR_AutoHandManager.instance.hand_L.SetActive(true);
        //        NSR_AutoHandManager.instance.hand_R.SetActive(true);
        //        NSR_AutoHandManager.instance.auto_hand_player.SetActive(true);
        //    }
        //    // 맵 켜고 끄기
        //    if (NSR_AutoHandManager.instance.body_zone.activeSelf)
        //    {
        //        NSR_AutoHandManager.instance.body_zone.SetActive(false);
        //    }
        //    if (NSR_AutoHandManager.instance.hand_zone.activeSelf == false)
        //    {
        //        NSR_AutoHandManager.instance.hand_zone.SetActive(true);
        //    }
        //}
        //// bodyPlayer
        //else
        //{
        //    // 손 켜져있으면 끄기
        //    if (NSR_AutoHandManager.instance.hand_L.activeSelf)
        //    {
        //        NSR_AutoHandManager.instance.hand_L.SetActive(false);
        //        NSR_AutoHandManager.instance.hand_R.SetActive(false);
        //        NSR_AutoHandManager.instance.auto_hand_player.SetActive(false);
        //    }
        //    // 맵 켜고 끄기
        //    if (NSR_AutoHandManager.instance.body_zone.activeSelf == false)
        //    {
        //        NSR_AutoHandManager.instance.body_zone.SetActive(true);
        //    }
        //    if (NSR_AutoHandManager.instance.hand_zone.activeSelf)
        //    {
        //        NSR_AutoHandManager.instance.hand_zone.SetActive(false);
        //    }

        //}

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

    [HideInInspector]
    public Vector3 recieve_trackingContainer_Pos;
    [HideInInspector]
    public Quaternion recieve_trackingContainer_Rot;

    [HideInInspector]
    public Vector3 recieve_autoHandPlayer_Pos;
    [HideInInspector]
    public Quaternion recieve_autoHandPlayer_Rot;

    [HideInInspector]
    public Vector3 recieve_hand_L_Pos;
    [HideInInspector]
    public Quaternion recieve_hand_L_Rot;
    [HideInInspector]
    public Vector3 recieve_hand_R_Pos;
    [HideInInspector]
    public Quaternion recieve_hand_R_Rot;

    public Vector3[] recieve_left_finger_Pos;
    public Quaternion[] recieve_left_finger_Rot;
    public Vector3[] recieve_right_finger_Pos;
    public Quaternion[] recieve_right_finger_Rot;

    public Vector3[] recieve_objects_Pos;
    public Quaternion[] recieve_objects_Rot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Tracking 위치 보내기
            stream.SendNext(NSR_AutoHandManager.instance.trackingContainer.transform.position);
            stream.SendNext(NSR_AutoHandManager.instance.trackingContainer.transform.rotation);
            // AutoHandPlayer 위치 보내기
            stream.SendNext(NSR_AutoHandManager.instance.autoHandPlayer.position);
            stream.SendNext(NSR_AutoHandManager.instance.autoHandPlayer.rotation);

            // 손 위치 보내기
            stream.SendNext(NSR_AutoHandManager.instance.hand_L.transform.position);
            stream.SendNext(NSR_AutoHandManager.instance.hand_L.transform.rotation); 
            stream.SendNext(NSR_AutoHandManager.instance.hand_R.transform.position);
            stream.SendNext(NSR_AutoHandManager.instance.hand_R.transform.rotation);
            // 손가락 위치 보내기
            for (int i = 0; i < 15; i++)
            {
                stream.SendNext(NSR_AutoHandManager.instance.leftFingers[i].transform.position);
                stream.SendNext(NSR_AutoHandManager.instance.leftFingers[i].transform.rotation);
                stream.SendNext(NSR_AutoHandManager.instance.rightFingers[i].transform.position);
                stream.SendNext(NSR_AutoHandManager.instance.rightFingers[i].transform.rotation);
            }

            // 오브젝트 위치 보내기
            for (int i = 0; i < NSR_AutoHandManager.instance.hand_zone_objects.Length; i++)
            {
                stream.SendNext(NSR_AutoHandManager.instance.hand_zone_objects[i].transform.position);
                stream.SendNext(NSR_AutoHandManager.instance.hand_zone_objects[i].transform.rotation);
            }
        }
        if (stream.IsReading)
        {
            // 받은 Tracking 위치
            recieve_trackingContainer_Pos = (Vector3)stream.ReceiveNext();
            recieve_trackingContainer_Rot = (Quaternion)stream.ReceiveNext();
            // 받은 AutoHandPlayer 위치
            recieve_autoHandPlayer_Pos = (Vector3)stream.ReceiveNext();
            recieve_autoHandPlayer_Rot = (Quaternion)stream.ReceiveNext();

            //받은 손 위치
            recieve_hand_L_Pos = (Vector3)stream.ReceiveNext();
            recieve_hand_L_Rot = (Quaternion)stream.ReceiveNext();
            recieve_hand_R_Pos = (Vector3)stream.ReceiveNext();
            recieve_hand_R_Rot = (Quaternion)stream.ReceiveNext();
            //받은 손가락 위치
            for (int i = 0; i < 15; i++)
            {
                recieve_left_finger_Pos[i] = (Vector3)stream.ReceiveNext();
                recieve_left_finger_Rot[i] = (Quaternion)stream.ReceiveNext();
                recieve_right_finger_Pos[i] = (Vector3)stream.ReceiveNext();
                recieve_right_finger_Rot[i] = (Quaternion)stream.ReceiveNext();
            }

            //받은 오브젝트 위치
            for (int i = 0; i < NSR_AutoHandManager.instance.hand_zone_objects.Length; i++)
            {
                recieve_objects_Pos[i] = (Vector3)stream.ReceiveNext();
                recieve_objects_Rot[i] = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
