using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_Player : MonoBehaviourPun, IPunObservable
{
    [HideInInspector]
    public bool handPlayer;
    [HideInInspector]
    public bool bodyPlayer;
    //============================= Start =============================
    public GameObject mainCamera;
    void Start()
    {
        if (photonView.IsMine)
        {
            // NSR_GameManager 에 나의 photonViewv저장
            NSR_GameManager.instance.myPhotonView = photonView;

            // 카메라 켜기
            mainCamera.SetActive(true);

            cc = GetComponent<CharacterController>();

            // 로테이션 초기값 설정
            rotY = transform.localEulerAngles.y;
            rotX = Camera.main.transform.localEulerAngles.x;
        }
    }

    //============================= Update =============================
    void Update()
    {
        #region Input Manager
        bool catch_left = Input.GetMouseButtonDown(0);
        bool drop_left = Input.GetMouseButtonUp(0);
        bool catch_right = Input.GetMouseButtonDown(1);
        bool drop_right = Input.GetMouseButtonUp(1);
        #endregion

        if (photonView.IsMine)
        {
            Move();
            Rotate();
            Catch_and_Drop(left_Hand, ref trCatched_Left, catch_left, drop_left, line_left);
            Catch_and_Drop(right_Hand, ref trCatched_Right, catch_right, drop_right, line_right);
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
    #region 물건 집고 놓기
    public Transform left_Hand;
    public Transform right_Hand;
    Transform trCatched_Left = null;
    Transform trCatched_Right = null;
    public LineRenderer line_left;
    public LineRenderer line_right;

    void Catch_and_Drop(Transform hand, ref Transform trCatched, bool catchInput, bool dropInput, LineRenderer line)
    {
        int layer = 1 << LayerMask.NameToLayer("item");
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5, layer))
        {
            if (trCatched == null)
            {
                line.gameObject.SetActive(true);
                line.SetPosition(0, hand.position);
                line.SetPosition(1, hit.point);

                if (catchInput)
                {
                    trCatched = hit.transform;
                    trCatched.parent = hand;
                    trCatched.position = hand.position;
                    trCatched.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
            else if (line.gameObject.activeSelf) line.gameObject.SetActive(false);
            
        }
        else if(line.gameObject.activeSelf)  line.gameObject.SetActive(false);
        

        if (dropInput)
        {
            if (trCatched != null)
            {
                trCatched.GetComponent<Rigidbody>().isKinematic = false;
                trCatched.parent = null;
                trCatched = null;
            }
        }
    }
    #endregion
}
