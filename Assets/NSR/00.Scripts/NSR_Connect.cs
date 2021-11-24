using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// 1. ������ ����
// 2. �κ� ����
// 3. �� ���� or �� ����
public class NSR_Connect : MonoBehaviourPunCallbacks
{

    void Awake()
    {
        Screen.SetResolution(960, 640, false);
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            //���� ���� ����
            PhotonNetwork.GameVersion = "1";
            //���� �õ� (name���� -> master����)
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //���Ӽ����� �κ� ����
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    //�κ����� ���� �� �� ���� �õ�
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("�κ� ���� ����");

        PhotonNetwork.JoinRoom("������");
    }
    //�� ���� ������
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("������ �Ϸ�");

        PhotonNetwork.LoadLevel("NSR_Scene");
    }

    //�� ���� ���н�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("������ ���� : " + returnCode + ", " + message);

        RoomOptions roomOptions = new RoomOptions();
        //�ο��� ����
        roomOptions.MaxPlayers = 2;

        //���� �̸����� ���� �����
        PhotonNetwork.CreateRoom("������", roomOptions, TypedLobby.Default);
    }

    //�� ���� ������
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("����� ����");

        PhotonNetwork.JoinRoom("������");
    }

    //�� ���� ���н�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("����� ���� : " + returnCode + ", " + message);
    }
}
