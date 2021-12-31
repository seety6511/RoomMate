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
        // 게임종료
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
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 진입 성공");

        RoomOptions roomOptions = new RoomOptions();
        //인원수 제한
        roomOptions.MaxPlayers = 2;

        if(openCreateDoor)
            PhotonNetwork.CreateRoom("1234", roomOptions, TypedLobby.Default);
        else if(openJoinDoor)
            PhotonNetwork.JoinRoom(answer.text);

    }
    //방 입장 성공시
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방입장 완료");

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


