using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//anchor가 null 이면 현재 자기 자신의 위치를 originPos로 한다.
public class SH_PosLock : MonoBehaviour
{
    Vector3 originPos;
    public bool posLock;
    public Transform anchor;
    public bool xLock;
    public bool yLock;
    public bool zLock;
    private void Awake()
    {
        originPos = anchor != null ? anchor.position : transform.position;
    }
    public void UnLock()
    {
        posLock = false;
    }
    public void Lock()
    {
        posLock = true;
    }
    private void FixedUpdate()
    {
        if (posLock)
        {
            var pos = originPos;
            if (!xLock)
                pos.x = transform.position.x;
            if (!yLock)
                pos.y = transform.position.y;
            if (!zLock)
                pos.z = transform.position.z;
            transform.position = pos;
        }
    }
}
