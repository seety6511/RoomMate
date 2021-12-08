using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SH_Limit : MonoBehaviour
{
    public enum SH_LimitDir
    {
        X,
        Y,
        Z
    }

    public SH_LimitDir dir;
    float origin;
    float max;
    public float range;


    private void Awake()
    {
        switch (dir)
        {
            case SH_LimitDir.X:
                origin = transform.position.x;  
                break;
            case SH_LimitDir.Y:
                origin = transform.position.y;
                break;
            case SH_LimitDir.Z:
                origin = transform.position.z;
                break;
        }
        max = origin + range;
    }
    private void FixedUpdate()
    {
        var pos = transform.position;

        switch (dir)
        {
            case SH_LimitDir.X:
                if (pos.x < origin)
                    pos.x = origin;
                if(pos.x> max)
                    pos.x = max;
                break;

            case SH_LimitDir.Y:
                if (pos.y < origin)
                    pos.y = origin;
                if (pos.y > max)
                    pos.y = max;
                break;

            case SH_LimitDir.Z:
                if (pos.z < origin)
                    pos.z = origin;
                if (pos.z > max)
                    pos.z = max;
                break;
        }
        transform.position = pos;

    }
}
