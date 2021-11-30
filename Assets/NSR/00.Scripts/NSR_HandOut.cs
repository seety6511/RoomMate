using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_HandOut : MonoBehaviour
{
    public bool isLeft;
    public bool isRight;
    private void OnTriggerEnter(Collider other)
    {
        if (isLeft)
        {
            NSR_HandPlayer.instance.leftHandOut = true;
            NSR_BodyPlayer.instance.leftHandOut = true;
        }
        else if (isRight)
        {
            NSR_HandPlayer.instance.rightHandOut = true;
            NSR_BodyPlayer.instance.rightHandOut = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isLeft)
        {
            NSR_HandPlayer.instance.leftHandOut = false;
            NSR_BodyPlayer.instance.leftHandOut = false;
        }
        else if (isRight)
        {
            NSR_HandPlayer.instance.rightHandOut = false;
            NSR_BodyPlayer.instance.rightHandOut = false;
        }
    }
}
