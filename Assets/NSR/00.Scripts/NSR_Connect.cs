using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// 1. 서버에 접속
// 2. 로비 진입
// 3. 방 생성 or 방 입장
public class NSR_Connect : MonoBehaviourPunCallbacks
{
    private void Start()
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

        PhotonNetwork.JoinRoom("게임장");
    }

    public GameObject gameManager;
    //방 입장 성공시
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방입장 완료");

        //게임메니저 켜기
        gameManager.SetActive(true);
    }

    //방 입장 실패시
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("방입장 실패 : " + returnCode + ", " + message);

        RoomOptions roomOptions = new RoomOptions();
        //인원수 제한
        roomOptions.MaxPlayers = 2;

        //방의 이름으로 방을 만들기
        PhotonNetwork.CreateRoom("게임장", roomOptions, TypedLobby.Default);
    }

    //방 생성 성공시
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방생성 성공");

        PhotonNetwork.JoinRoom("게임장");
    }

    //방 생성 실패시
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("방생성 실패 : " + returnCode + ", " + message);
    }
}
