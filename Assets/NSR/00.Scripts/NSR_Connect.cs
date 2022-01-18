using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Autohand.Demo;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.Serialization;
using UnityEditor;

namespace MText
{

    public class NSR_Connect : MonoBehaviourPunCallbacks
    {
        public Modular3DText Text;
        public Modular3DText Text2;
        
        [HideInInspector]
        public string answer;

        public bool openJoinDoor;
        public bool openCreateDoor;

        [HideInInspector]
        public bool enterBtn;
        [HideInInspector]
        public bool createBtn;

        public GameObject joinFailText;
        public GameObject[] createFailText;
        public GameObject joinSuccessText;

        public GameObject threshold;

        public string[] sceneName;
        [HideInInspector]
        public int chNum;
        private void Start()
        {
            joinFailText.SetActive(false);
            for (int i = 0; i < createFailText.Length; i++)
            {
                createFailText[i].SetActive(false);
            }
            joinSuccessText.SetActive(false);

            //랜덤 숫자 4자리
            for(int i = 0; i < 4; i++)
            {
                Text.Text += ((int)Random.Range(0, 9)).ToString();
            }

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

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            print("로비 진입 성공");
        }

        bool isMaster;
        private void Update()
        {

            if (openCreateDoor)
            {
                isMaster = true;

                RoomOptions roomOptions = new RoomOptions();
                //roomOptions.PublishUserId = true;
                //인원수 제한
                roomOptions.MaxPlayers = 2;
                PhotonNetwork.CreateRoom(Text.Text, roomOptions, TypedLobby.Default);
                openCreateDoor = false;
            }
            else if (enterBtn)
            {
                print(answer);
                enterBtn = false;

                for (int i = 0; i < rooms.Count; i++)
                {
                    if (rooms[i].Name == answer)
                    {
                        threshold.SetActive(false);
                        joinSuccessText.SetActive(true);
                        break;
                    }
                }
            }

            if (openJoinDoor)
            {
                PhotonNetwork.JoinRoom(answer);
                openJoinDoor = false;
            }

            //if (PhotonNetwork.InRoom)
            //{
            //    if (openJoinDoor)
            //    {
            //        PhotonNetwork.LoadLevel("KHJ_Test");
            //        openJoinDoor = false;
            //    }
            //}
        }
        //방 입장 성공시
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print("방입장 완료");
            PhotonNetwork.LoadLevel(sceneName[chNum]);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);

            joinFailText.SetActive(true);
            threshold.SetActive(true);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);

            for (int i = 0; i < createFailText.Length; i++)
            {
                createFailText[i].SetActive(true);
            }
            // 문 위치 원래대로
        }

        List<RoomInfo> rooms = new List<RoomInfo>();
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            rooms = roomList;
        }
    }
}