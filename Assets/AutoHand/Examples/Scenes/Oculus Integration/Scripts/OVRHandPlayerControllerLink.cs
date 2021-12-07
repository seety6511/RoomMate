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

        Vector2 moveInput;
        float turnInput;
        public void Update() 
        {
            if (PhotonNetwork.IsMasterClient == false)
            {
                moveInput = OVRInput.Get(moveAxis, moveController);
                turnInput = OVRInput.Get(turnAxis, turnController).x;
            }
            else
            {
                moveInput = NSR_AutoBodyPlayer.instance.recieve_moveInput;
                turnInput = NSR_AutoBodyPlayer.instance.recieve_turnInput;
            }

            player.Move(moveInput);
            player.Turn(turnInput);
        }
    }
}
