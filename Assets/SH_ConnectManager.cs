using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using MText;

public enum Scene
{
    Chapter1,
    Chapter2,
}

public class SH_ConnectManager : MonoBehaviourPunCallbacks
{
    public SH_ChapterDoor[] doors;
    public int numberLength;

    public GameObject threshold;

    bool connected;
    private void Awake()
    {
        numberLength = 4;
        if (Connect())
        {
            doors = FindObjectsOfType<SH_ChapterDoor>();
            CreateRandomRoomNumber();
            Debug.Log("Connect Server Success");
        }
        else
        {
            ConnectFailed();
            Debug.Log("Connect Server Fail");
        }
    }

    void ConnectFailed()
    {
        for(int i = 0; i < doors.Length; ++i)
        {
            doors[i].roomNumber.UpdateText("Connection Fail!");
        }
    }

    void CreateRandomRoomNumber()
    {
        for(int i = 0; i < doors.Length; ++i)
        {
            string num = "";
            for(int q = 0; q < numberLength; ++q)
            {
                num += Random.Range(0, 10).ToString();
            }
            doors[i].roomNumber.UpdateText(num);
        }

    }

    bool Connect()
    {
        //게임 버전 설정
        PhotonNetwork.GameVersion = "1";
        //접속 시도 (name서버 -> master서버)
        connected = PhotonNetwork.ConnectUsingSettings();
        return connected;
    }

    //접속성공시 로비 진입
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 진입 성공");
    }

    Scene scene;
    //방 입장 성공시
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(scene.ToString());
        print("방입장 완료");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("JoinRoomFailed");
        //threshold.SetActive(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

    }

    List<RoomInfo> rooms = new List<RoomInfo>();
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        rooms = roomList;
    }

    public void Check(string num)
    {
        foreach (var t in doors)
        {
            if(t.roomNumber.Text == num)
            {
                Debug.Log("Room Found : "+t.scene.ToString());
                JoinRoom(t.scene);
                return;
            }
        }

        Debug.Log("Room Found Fail");
    }

    public void CreateRoom(string text)
    {
        RoomOptions roomOptions = new RoomOptions();
        //roomOptions.PublishUserId = true;
        //인원수 제한
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(text, roomOptions, TypedLobby.Default);
        print("CreateRoom");
    }

    public void JoinRoom(Scene scene)
    {
        this.scene = scene;
        PhotonNetwork.JoinRoom(scene.ToString());
    }
}
