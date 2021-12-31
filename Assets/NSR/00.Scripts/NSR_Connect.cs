using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class NSR_Connect : MonoBehaviourPunCallbacks
{
    public Text answer;

    bool openCreateDoor;
    bool openJoinDoor;

    public GameObject joinFailText;
    public GameObject createFailText;

    public void OpenCreate()
    {
        openCreateDoor = true;
    }

    public void OpenJoin()
    {
        openJoinDoor = true;
    }

    public void OpenExit()
    {
        // ��������
        Application.Quit();
    }
    private void Start()
    {
        if(joinFailText != null)
        joinFailText.SetActive(false);
        if (createFailText != null)
        createFailText.SetActive(false);

        Connect();
    }
    void Connect()
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

        RoomOptions roomOptions = new RoomOptions();
        //�ο��� ����
        roomOptions.MaxPlayers = 2;

        if(openCreateDoor)
            PhotonNetwork.CreateRoom("1234", roomOptions, TypedLobby.Default);
        else if(openJoinDoor)
            PhotonNetwork.JoinRoom(answer.text);

    }
    //�� ���� ������
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("������ �Ϸ�");

        PhotonNetwork.LoadLevel("NSR_main");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        joinFailText.SetActive(true);
        openJoinDoor = false;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        createFailText.SetActive(true);
        openCreateDoor = false;
    }
}


