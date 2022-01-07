using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class NSR_Hand : MonoBehaviour
{
    public GameObject[] targetColl;
    public GameObject indexPivot;

    Grabbable grabObj;

    bool isTarget;
    private void OnTriggerStay(Collider other)
    {
        isTarget = false;
        grabObj = GetComponent<Hand>().holdingObj;

        for (int i = 0; i < targetColl.Length; i++)
        {
            if (other.transform == targetColl[i].transform && grabObj == null && targetColl[i] != null)
            {
                isTarget = true;
                break;
            }
        }

        if (isTarget)
            indexPivot.SetActive(true);
        else
            indexPivot.SetActive(false);
    }
}
