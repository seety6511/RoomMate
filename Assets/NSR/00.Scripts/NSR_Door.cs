using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand.Demo;

namespace MText
{
    public class NSR_Door : MonoBehaviour
    {
        public NSR_Connect connect;
        public enum DOOR
        {
            create,
            join,
            exit
        }

        public DOOR door;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name != "DoorBackWall") return;

            switch (door)
            {
                case DOOR.create:
                    connect.openCreateDoor = true;
                    break;

                case DOOR.join:
                    connect.openJoinDoor = true;
                    break;

                case DOOR.exit:
                    // 게임종료
                    Application.Quit();
                    break;
            }
        }

        public void SetChapterNum(int chNum)
        {
            connect.chNum = chNum;
        }
    }
}