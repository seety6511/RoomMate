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

            //���� ���� 4�ڸ�
            for(int i = 0; i < 4; i++)
            {
                Text.Text += ((int)Random.Range(0, 9)).ToString();
            }
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

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            print("�κ� ���� ����");
        }

        bool isMaster;
        private void Update()
        {

            if (openCreateDoor)
            {
                isMaster = true;

                RoomOptions roomOptions = new RoomOptions();
                //roomOptions.PublishUserId = true;
                //�ο��� ����
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
        //�� ���� ������
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print("������ �Ϸ�");
            PhotonNetwork.LoadLevel("KHJ_Test");

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

            createFailText.SetActive(true);
            // �� ��ġ �������
        }

        List<RoomInfo> rooms = new List<RoomInfo>();
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            rooms = roomList;
        }
    }
}