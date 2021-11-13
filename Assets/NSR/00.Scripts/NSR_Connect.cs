using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// 1. ������ ����
// 2. �κ� ����
// 3. �� ����
// 4. �� ����

public class NSR_Connect : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Connect();
    }

    // ��Ʈ��ũ ����
    public void Connect()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            //���� ���� ����
            PhotonNetwork.GameVersion = "1";
            //���� �õ� (name���� -> master����)
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //���Ӽ�����
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        //�κ� ���� �õ�
        PhotonNetwork.JoinLobby();
    }

    //�κ����� ������
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("�κ� ���� ����");

        RoomOptions roomOptions = new RoomOptions();
        //�ο��� ����
        roomOptions.MaxPlayers = 2;

        //�����̸����� ���� ����� ���� �õ�
        PhotonNetwork.CreateRoom("������", roomOptions, TypedLobby.Default);
    }

    //�� ���� ������
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("����� ����");
    }

    //�� ���� ���н�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("����� ���� : " + returnCode + ", " + message);
    }

    //�� ����
    public void JoinRoom()
    {
        //�� ���� �õ�
        PhotonNetwork.JoinRoom("������");
    }

    //�� ���� ������
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("������ �Ϸ�");
        //PhotonNetwork.LoadLevel("NSR_Scene");
    }

    //�� ���� ���н�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("������ ���� : " + returnCode + ", " + message);
    }
}
