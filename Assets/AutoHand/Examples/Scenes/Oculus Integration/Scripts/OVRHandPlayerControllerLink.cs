using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;
using NaughtyAttributes;
using Photon.Pun;

namespace Autohand.Demo{
    public class OVRHandPlayerControllerLink : MonoBehaviourPun
    {
        public AutoHandPlayer player;
        public OVRInput.Controller moveController;
        public OVRInput.Axis2D moveAxis;

        public OVRInput.Controller turnController;
        public OVRInput.Axis2D turnAxis;

        [HideInInspector]
        public Vector2 moveInput;
        float turnInput;

        public bool inGame;
        public bool test;


        public void Update() 
        {
            if (test || (PhotonNetwork.IsConnected && !inGame))
            {
                moveInput = OVRInput.Get(moveAxis, moveController);
                turnInput = OVRInput.Get(turnAxis, turnController).x;
            }

            if (NSR_AutoHandManager.instance != null)
            {
                NSR_AutoHandManager playerManager = NSR_AutoHandManager.instance;
                if (playerManager.isChanging == false) return;

                if (playerManager.bodyplayer == false)
                {
                    if (NSR_AutoBodyPlayer.instance != null)
                    {
                        moveInput = NSR_AutoBodyPlayer.instance.recieve_moveInput;
                        turnInput = NSR_AutoBodyPlayer.instance.recieve_turnInput;
                    }
                }

                if (Mathf.Pow(moveInput.x, 2) > 0 || Mathf.Pow(moveInput.y, 2) > 0)
                {
                    playerManager.FootSound(true);
                }
                else
                {
                    playerManager.FootSound(false);
                }
            }
            player.Move(moveInput);
            player.Turn(turnInput);
        }
    }
}
