using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class NSR_Hand : MonoBehaviour
{
    public GameObject[] targetColl;
    public GameObject indexPivot;

    Grabbable grabObj;
    private void OnTriggerStay(Collider other)
    {
        for (int i = 0; i < targetColl.Length; i++)
        {
            grabObj = GetComponent<Hand>().holdingObj;
            if (other.transform == targetColl[i].transform && grabObj == null && targetColl[i] != null)
            {
                indexPivot.SetActive(true);
            }
            else
            {
                indexPivot.SetActive(false);
            }
        }
    }
}
