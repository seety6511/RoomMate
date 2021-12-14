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
        // �����̽��� ������ ��Ʈ�� �ٲٱ�
        if (Input.GetKeyDown(KeyCode.Space) /*|| OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.LTouch)*/)
        {
            photonView.RPC("ChangeControl", RpcTarget.All);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if(NSR_AutoHandManager.instance.bodyPlaeyr)
                photonView.RPC("TakeHandControl", RpcTarget.All);
            if(NSR_AutoHandManager.instance.handPlayer)
                photonView.RPC("TakeBodyControl", RpcTarget.All);
        }
    }

    //����� ��ȯ

    [PunRPC]
    void ChangeControl(PhotonMessageInfo info)
    {
        //PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
        NSR_AutoHandManager.instance.handPlayer = !NSR_AutoHandManager.instance.handPlayer;
        NSR_AutoHandManager.instance.bodyPlaeyr = !NSR_AutoHandManager.instance.bodyPlaeyr;

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
            // ���� �ٵ� -> �ڵ�
            NSR_AutoBodyPlayer.instance.photonView.TransferOwnership(you);
            NSR_AutoHandPlayer.instance.photonView.TransferOwnership(me);
        }
        else
        {
            // ���� �ڵ� -> �ٵ�
            NSR_AutoBodyPlayer.instance.photonView.TransferOwnership(me);
            NSR_AutoHandPlayer.instance.photonView.TransferOwnership(you);
        }
    }

    [PunRPC]
    void TakeBodyControl()
    {
        NSR_AutoHandManager.instance.bodyPlaeyr = !NSR_AutoHandManager.instance.bodyPlaeyr;

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
            // ���� �ٵ� -> �ڵ�
            NSR_AutoBodyPlayer.instance.photonView.TransferOwnership(you);
        }
        else
        {
            // ���� �ڵ� -> �ٵ�
            NSR_AutoBodyPlayer.instance.photonView.TransferOwnership(me);
        }
    }

    [PunRPC]
    void TakeHandControl()
    {
        NSR_AutoHandManager.instance.bodyPlaeyr = !NSR_AutoHandManager.instance.bodyPlaeyr;

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
            // ���� �ٵ� -> �ڵ�
            NSR_AutoBodyPlayer.instance.photonView.TransferOwnership(me);
        }
        else
        {
            // ���� �ڵ� -> �ٵ�
            NSR_AutoBodyPlayer.instance.photonView.TransferOwnership(you);

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
            // Tracking ��ġ ������
            stream.SendNext(NSR_AutoHandManager.instance.trackingContainer.transform.position);
            stream.SendNext(NSR_AutoHandManager.instance.trackingContainer.transform.rotation);
            // AutoHandPlayer ��ġ ������
            stream.SendNext(NSR_AutoHandManager.instance.autoHandPlayer.position);
            stream.SendNext(NSR_AutoHandManager.instance.autoHandPlayer.rotation);

            // �� ��ġ ������
            stream.SendNext(NSR_AutoHandManager.instance.hand_L.transform.position);
            stream.SendNext(NSR_AutoHandManager.instance.hand_L.transform.rotation);
            stream.SendNext(NSR_AutoHandManager.instance.hand_R.transform.position);
            stream.SendNext(NSR_AutoHandManager.instance.hand_R.transform.rotation);
            // �հ��� ��ġ ������
            for (int i = 0; i < 15; i++)
            {
                stream.SendNext(NSR_AutoHandManager.instance.leftFingers[i].transform.position);
                stream.SendNext(NSR_AutoHandManager.instance.leftFingers[i].transform.rotation);
                stream.SendNext(NSR_AutoHandManager.instance.rightFingers[i].transform.position);
                stream.SendNext(NSR_AutoHandManager.instance.rightFingers[i].transform.rotation);
            }

            // ������Ʈ ��ġ ������
            for (int i = 0; i < NSR_AutoHandManager.instance.hand_zone_objects.Length; i++)
            {
                if(NSR_AutoHandManager.instance.hand_zone_objects[i] != null)
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
            // ���� Tracking ��ġ
            recieve_trackingContainer_Pos = (Vector3)stream.ReceiveNext();
            recieve_trackingContainer_Rot = (Quaternion)stream.ReceiveNext();
            // ���� AutoHandPlayer ��ġ
            recieve_autoHandPlayer_Pos = (Vector3)stream.ReceiveNext();
            recieve_autoHandPlayer_Rot = (Quaternion)stream.ReceiveNext();

            //���� �� ��ġ
            recieve_hand_L_Pos = (Vector3)stream.ReceiveNext();
            recieve_hand_L_Rot = (Quaternion)stream.ReceiveNext();
            recieve_hand_R_Pos = (Vector3)stream.ReceiveNext();
            recieve_hand_R_Rot = (Quaternion)stream.ReceiveNext();
            //���� �հ��� ��ġ
            for (int i = 0; i < 15; i++)
            {
                recieve_left_finger_Pos[i] = (Vector3)stream.ReceiveNext();
                recieve_left_finger_Rot[i] = (Quaternion)stream.ReceiveNext();
                recieve_right_finger_Pos[i] = (Vector3)stream.ReceiveNext();
                recieve_right_finger_Rot[i] = (Quaternion)stream.ReceiveNext();
            }

            //���� ������Ʈ ��ġ
            for (int i = 0; i < NSR_AutoHandManager.instance.hand_zone_objects.Length; i++)
            {
                if(NSR_AutoHandManager.instance.hand_zone_objects[i] != null)
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
