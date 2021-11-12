using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_VRPlayer : MonoBehaviour
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
        Vector2 hv = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        Vector3 dirH = transform.right * hv.x;
        Vector3 dirV = transform.forward * hv.y;
        Vector3 dir = dirH + dirV;
        dir.Normalize();

        cc.Move(dir * speed * Time.deltaTime);
    }
    #endregion
}
