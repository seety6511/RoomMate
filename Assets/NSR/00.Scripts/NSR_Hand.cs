using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class NSR_Hand : MonoBehaviour
{
    public Collider[] targetColl;
    public GameObject indexPivot;

    Grabbable grabObj;

    bool isTarget;
    private void OnTriggerStay(Collider other)
    {
        isTarget = false;
        grabObj = GetComponent<Hand>().holdingObj;

        for (int i = 0; i < targetColl.Length; i++)
        {
            if (other == targetColl[i])
            {
                isTarget = true;
                break;
            }
        }

        if (isTarget && grabObj == null)
            indexPivot.SetActive(true);
        else
            indexPivot.SetActive(false);
    }
}
