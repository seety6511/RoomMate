using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_VRPlayer : MonoBehaviourPun, IPunObservable
{
    public bool handPlayer;
    public bool bodyPlayer;
    //============================= Start =============================
    public GameObject OVRCameraRig;
    void Start()
    {
        if (photonView.IsMine)
        {
            // NSR_GameManager 에 나의 photonViewv저장
            NSR_GameManager.instance.myPhotonView = photonView;

            // OVRCameraRig 켜기
            OVRCameraRig.SetActive(true);

            cc = GetComponent<CharacterController>();
        }
    }
    //============================= Update =============================
    void Update()
    {
        if (photonView.IsMine)
        {
            if (handPlayer)
            {
                Rotate();
            }
            else if (bodyPlayer)
            {
                Move();
                Rotate();
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, 0.2f);
        }
    }
    //====================================================================
    #region 이동 및 회전
    CharacterController cc;
    public float speed = 5;
    void Move()
    {
        Vector2 hv = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        Vector3 dirH = Camera.main.transform.right * hv.x;
        Vector3 dirV = Camera.main.transform.forward * hv.y;
        Vector3 dir = dirH + dirV;
        dir.y = 0;
        dir.Normalize();

        cc.Move(dir * speed * Time.deltaTime);
    }

    public float rotSpeed = 40f;
    float y;
    void Rotate()
    {
        Vector2 thumb = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);
        float v = thumb.x;

        y += v * rotSpeed * Time.deltaTime;

        transform.localEulerAngles = new Vector3(0, y, 0);
    }

    Vector3 receivePos;
    Quaternion receiveRot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //만약에 쓸 수 있는 상태라면
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        //만약에 읽을 수 있는 상태라면
        if (stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
    #endregion
}
