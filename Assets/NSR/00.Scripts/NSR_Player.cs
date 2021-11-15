using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_Player : MonoBehaviourPun, IPunObservable
{
    public bool handPlayer;
    public bool bodyPlayer;
    //============================= Start =============================
    public GameObject mainCamera;
    void Start()
    {
        if (photonView.IsMine)
        {
            // NSR_GameManager �� ���� photonViewv����
            NSR_GameManager.instance.myPhotonView = photonView;

            // ī�޶� �ѱ�
            mainCamera.SetActive(true);

            cc = GetComponent<CharacterController>();

            // �����̼� �ʱⰪ ����
            rotY = transform.localEulerAngles.y;
            rotX = Camera.main.transform.localEulerAngles.x;
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
    #region �̵� �� ȸ��
    CharacterController cc;
    public float speed = 5;
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * v;
        Vector3 dir = dirH + dirV;
        dir.Normalize();

        cc.Move(dir * speed * Time.deltaTime);
    }

    public float rotSpeed = 40f;
    float rotY;
    float rotX;
    void Rotate()
    {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            rotX += my * rotSpeed * Time.deltaTime;
            rotY += mx * rotSpeed * Time.deltaTime;
            rotX = Mathf.Clamp(rotX, -60, 60);

            transform.localEulerAngles = new Vector3(0, rotY, 0);
            Camera.main.transform.localEulerAngles = new Vector3(-rotX, 0, 0);
        
    }

    Vector3 receivePos;
    Quaternion receiveRot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //���࿡ �� �� �ִ� ���¶��
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        //���࿡ ���� �� �ִ� ���¶��
        if (stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
    #endregion
}
