using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NSR_Grabbable : MonoBehaviour
{
    public Transform leftPos;
    public Transform rightPos;

    public bool isLeft;
    public bool isRight;

    public bool isKey;
    void Update()
    {
        if (isLeft)
        {
            print("¿Ãµø");
            transform.position = Vector3.Lerp(transform.position, leftPos.position, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, leftPos.rotation, 0.2f);
        }
        else if (isRight)
        {
            transform.position = Vector3.Lerp(transform.position, rightPos.position, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rightPos.rotation, 0.2f);
        }
        else
        {
            if (isKey)
                transform.localPosition = new Vector3(0, 0.223f, 0);
            else
                transform.localPosition = Vector3.zero;

            transform.localRotation = Quaternion.identity;
        }

    }
}

