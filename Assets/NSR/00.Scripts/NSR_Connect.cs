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

    public GameObject gameManager;
    //�� ���� ������
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("������ �Ϸ�");

        //���Ӹ޴��� �ѱ�
        gameManager.SetActive(true);
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
