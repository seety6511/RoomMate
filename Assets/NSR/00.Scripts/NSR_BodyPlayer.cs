using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_BodyPlayer : MonoBehaviour
{
    void Update()
    {
        if (NSR_PlayerManager.instance.bodyControll == false) return;

        Quaternion headRot = head.localRotation;
        headRot.z = 0;
        headRot.x = 0;
        transform.localRotation = headRot;

        Move();
        Rotate();
    }

    #region 이동 및 회전
    public float speed = 5;
    void Move()
    {
        Vector2 hv = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        Vector3 dirH = transform.right * hv.x;
        Vector3 dirV = transform.forward * hv.y;
        Vector3 dir = dirH + dirV;
        dir.y = 0;
        dir.Normalize();

        transform.position += dir * speed * Time.deltaTime;
    }

    public float rotSpeed = 40f;
    float y;
    public Transform head;
    void Rotate()
    {
        
        Vector2 thumb = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);
        float v = thumb.x;

        y += v * rotSpeed * Time.deltaTime;

        transform.localEulerAngles = new Vector3(0, y, 0);
    }

    #endregion

}
