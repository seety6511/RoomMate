using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_Finger : MonoBehaviour
{
    public float speed = 5;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            NSR_PushBtn pushBtn = other.transform.GetComponent<NSR_PushBtn>();
            pushBtn.push = !pushBtn.push;
            if (pushBtn.push)
            {
                print("버튼 누름");
            }
        }
    }
}
