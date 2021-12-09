using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PosLock : MonoBehaviour
{
    Vector3 originPos;
    public bool posLock;
    public bool xLock;
    public bool yLock;
    public bool zLock;
    private void Awake()
    {
        originPos = transform.position;
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
