using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;
using NaughtyAttributes;
using Photon.Pun;

namespace Autohand.Demo{
    public class OVRHandPlayerControllerLink : MonoBehaviourPun, IPunObservable
    {
        public AutoHandPlayer player;
        public OVRInput.Controller moveController;
        public OVRInput.Axis2D moveAxis;

        public OVRInput.Controller turnController;
        public OVRInput.Axis2D turnAxis;

        Vector2 moveInput;
        float turnInput;

        Vector2 recieveMoveInput;
        float recieveTurnInput;
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(moveInput);
                stream.SendNext(turnInput);
            }
            //만약에 읽을 수 있는 상태라면
            if (stream.IsReading)
            {
                recieveMoveInput = (Vector2)stream.ReceiveNext();
                recieveTurnInput = (float)stream.ReceiveNext();
            }
        }

        public void Update() 
        {
            if (PhotonNetwork.IsMasterClient == false)
            {
                moveInput = OVRInput.Get(moveAxis, moveController);
                turnInput = OVRInput.Get(turnAxis, turnController).x;
                
            }
            else
            {
                moveInput = recieveMoveInput;
                turnInput = recieveTurnInput;
            }

            player.Move(moveInput);
            player.Turn(turnInput);
        }
    }
}
