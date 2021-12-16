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
    bool beforeHand;
    void Update()
    {
        // 스페이스바 누르면 컨트롤 바꾸기
        if (Input.GetKeyDown(KeyCode.Space) /*|| OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.LTouch)*/)
        {
            photonView.RPC("GetControl", RpcTarget.All, true, false);
            photonView.RPC("GetControl", RpcTarget.All, false, false);
        }

        if (Input.GetKeyDown(KeyCode.K))
            photonView.RPC("GetControl", RpcTarget.All, !NSR_AutoHandManager.instance.handPlayer, false);

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (NSR_AutoHandManager.instance.handPlayer == beforeHand)
                photonView.RPC("GetControl", RpcTarget.All, false, true);
            else
                photonView.RPC("GetControl", RpcTarget.All, true, true);
        }
    }

    // 제어건 가져오기
    [PunRPC]
    void GetControl(bool handPlayer, bool re)
    {
        if (re)
        {
            NSR_AutoHandManager.instance.handPlayer = beforeHand;
            NSR_AutoHandManager.instance.bodyplayer = !beforeHand;
        }
        else
        {
            beforeHand = NSR_AutoHandManager.instance.handPlayer;

            if (handPlayer)
                NSR_AutoHandManager.instance.handPlayer = !NSR_AutoHandManager.instance.handPlayer;
            else
                NSR_AutoHandManager.instance.bodyplayer = !NSR_AutoHandManager.instance.bodyplayer;
        }

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

        if (re)
        {
            if (beforeHand)
            {
                NSR_AutoHandPlayer.instance.photonView.TransferOwnership(me);
                NSR_AutoBodyPlayer.instance.photonView.TransferOwnership(you);
            }
            else
            {
                NSR_AutoBodyPlayer.instance.photonView.TransferOwnership(me);
                NSR_AutoHandPlayer.instance.photonView.TransferOwnership(you);
            }
        }
        else
        {
            if (me == NSR_AutoBodyPlayer.instance.photonView.Owner)
            {
                if (handPlayer)
                    NSR_AutoHandPlayer.instance.photonView.TransferOwnership(me);
                else
                    NSR_AutoBodyPlayer.instance.photonView.TransferOwnership(you);
            }
            else
            {
                if (handPlayer)
                    NSR_AutoHandPlayer.instance.photonView.TransferOwnership(you);
                else
                    NSR_AutoBodyPlayer.instance.photonView.TransferOwnership(me);
            }
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
    public Vector3[] recieve_objects_Scale;

    public bool[] receive_input_R;
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
                if (NSR_AutoHandManager.instance.hand_zone_objects[i] != null)
                {
                    stream.SendNext(NSR_AutoHandManager.instance.hand_zone_objects[i].transform.position);
                    stream.SendNext(NSR_AutoHandManager.instance.hand_zone_objects[i].transform.rotation);
                    stream.SendNext(NSR_AutoHandManager.instance.hand_zone_objects[i].transform.localScale);
                }
            }

            stream.SendNext(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch));
            stream.SendNext(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch));
            stream.SendNext(OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch));
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
                if (NSR_AutoHandManager.instance.hand_zone_objects[i] != null)
                {
                    recieve_objects_Pos[i] = (Vector3)stream.ReceiveNext();
                    recieve_objects_Rot[i] = (Quaternion)stream.ReceiveNext();
                    recieve_objects_Scale[i] = (Vector3)stream.ReceiveNext();
                }
            }

            for (int i = 0; i < 3; i++)
            {
                receive_input_R[i] = (bool)stream.ReceiveNext();
            }
        }


    }
}
