using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// 1. 서버에 접속
// 2. 로비 진입
// 3. 방 생성
// 4. 방 입장

public class NSR_Connect : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Connect();
    }

    // 네트워크 접속
    public void Connect()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            //게임 버전 설정
            PhotonNetwork.GameVersion = "1";
            //접속 시도 (name서버 -> master서버)
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //접속성공시
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        //로비 진입 시도
        PhotonNetwork.JoinLobby();
    }

    //로비진입 성공시
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 진입 성공");

        RoomOptions roomOptions = new RoomOptions();
        //인원수 제한
        roomOptions.MaxPlayers = 2;

        //방의이름으로 방을 만들고 입장 시도
        PhotonNetwork.CreateRoom("게임장", roomOptions, TypedLobby.Default);
    }

    //방 생성 성공시
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방생성 성공");
    }

    //방 생성 실패시
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("방생성 실패 : " + returnCode + ", " + message);
    }

    //방 입장
    public void JoinRoom()
    {
        //방 입장 시도
        PhotonNetwork.JoinRoom("게임장");
    }

    //방 입장 성공시
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방입장 완료");
        //PhotonNetwork.LoadLevel("NSR_Scene");
    }

    //방 입장 실패시
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("방입장 실패 : " + returnCode + ", " + message);
    }
}
