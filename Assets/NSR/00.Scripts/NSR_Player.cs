using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_Player : MonoBehaviourPun
{
    public bool handPlayer;
    public bool bodyPlayer;

    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();

        if (photonView.IsMine)
        {
            NSR_GameManager.instance.myPhotonView = photonView;
        }
    }

    void Update()
    {
        if (handPlayer)
        {

        }
        else if (bodyPlayer)
        {
            Move();
        }
    }

    #region ¿Ãµø
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
    #endregion
}
