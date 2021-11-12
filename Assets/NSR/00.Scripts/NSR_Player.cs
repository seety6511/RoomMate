using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_Player : MonoBehaviour
{
    public bool handPlayer;
    public bool bodyPlayer;

    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
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
