using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_PushBtn : MonoBehaviour
{
    public Vector3 startPos;
    public bool push;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
        if (push)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 0.1f);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, 0.1f);
        }
    }
}
