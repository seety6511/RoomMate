using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_Door : MonoBehaviour
{
     public bool open;
    Vector3 startAngle;

    private void Start()
    {
        startAngle = transform.localEulerAngles;   
    }

    void Update()
    {
        if (open)
        {
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, Vector3.zero, 0.1f);
        }
        else
        {
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, startAngle, 0.1f);
        }
    }
}
