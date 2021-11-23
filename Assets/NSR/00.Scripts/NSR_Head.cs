using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_Head : MonoBehaviourPun, IPunObservable
{
    public Transform CenterEyeAnchor;
    Vector3 receivePos;
    Quaternion receiveRot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //만약에 쓸 수 있는 상태라면
        if (stream.IsWriting)
        {
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localRotation);
        }
        //만약에 읽을 수 있는 상태라면
        if (stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Update()
    {
        if (!NSR_PlayerManager.instance.bodyControl)
        {

            transform.localPosition = Vector3.Lerp(transform.localPosition, receivePos, 0.2f);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, receiveRot, 0.2f);
        }
        else
        {
            transform.localPosition = CenterEyeAnchor.localPosition;
            transform.localRotation = CenterEyeAnchor.localRotation;
        }
       
    }
}
