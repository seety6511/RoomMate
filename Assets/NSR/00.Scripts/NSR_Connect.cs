using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Autohand.Demo;
public class NSR_Connect : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public string answer;

    public bool openJoinDoor;
    public bool openCreateDoor;

    [HideInInspector]
    public bool enterBtn;
    [HideInInspector]
    public bool createBtn;

    public GameObject joinFailText;
    public GameObject createFailText;
    public GameObject joinSuccessText;

    public GameObject threshold;

    private void Start()
    {
        joinFailText.SetActive(false);
        createFailText.SetActive(false);
        joinSuccessText.SetActive(false);

        if (PhotonNetwork.IsConnected) return;
        Connect();
    }
    void Connect()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            //게임 버전 설정
            PhotonNetwork.GameVersion = "1";
            //접속 시도 (name서버 -> master서버)
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //접속성공시 로비 진입
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    //로비진입 성공 시 방 입장 시도
    RoomOptions roomOptions;
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 진입 성공");


        roomOptions = new RoomOptions();
        //roomOptions.PublishUserId = true;
        //인원수 제한
        roomOptions.MaxPlayers = 2;
    }

    bool isMaster;
    private void Update()
    {
        if (PhotonNetwork.InLobby)
        {
            if (openCreateDoor)
            {
                isMaster = true;
                PhotonNetwork.CreateRoom("1234", roomOptions, TypedLobby.Default);
                openCreateDoor = false;
            }
            else if (enterBtn)
            {
                print(answer);
                PhotonNetwork.JoinRoom(answer);
                enterBtn = false;
            }
        }

        if (PhotonNetwork.InRoom)
        {
            if (openJoinDoor)
            {
                PhotonNetwork.LoadLevel("KHJ_Test");
                openJoinDoor = false;
            }
        }
    }
    //방 입장 성공시
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방입장 완료");

        if (isMaster)
        {
            PhotonNetwork.LoadLevel("KHJ_Test");
            isMaster = false;
        }
        else
        {
            threshold.SetActive(false);
            joinSuccessText.SetActive(true);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        joinFailText.SetActive(true);
        threshold.SetActive(true);

        if (PhotonNetwork.IsConnected == false)
        {
            //게임 버전 설정
            PhotonNetwork.GameVersion = "1";
            //접속 시도 (name서버 -> master서버)
            PhotonNetwork.ConnectUsingSettings();
        }
        else if (PhotonNetwork.InLobby == false)
        {
            PhotonNetwork.JoinLobby();
        }

        if (enterBtn)
        {
            print(answer);
            PhotonNetwork.JoinRoom(answer);
            enterBtn = false;
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        createFailText.SetActive(true);
        // 문 위치 원래대로
    }
}


