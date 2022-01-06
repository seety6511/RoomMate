using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class NSR_Hand : MonoBehaviour
{
    public GameObject Pivot;
    public GameObject[] targetColl;
    public GameObject indexPivot;

    Grabbable grabObj;

    bool isTargetObj;
    private void Update()
    {
       
    }
    private void OnTriggerStay(Collider other)
    {
        for(int i = 0; i< targetColl.Length; i++)
        {
            if(other.gameObject == targetColl[i].gameObject)
            {
                grabObj = GetComponent<Hand>().holdingObj;

                if (grabObj == null)
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
}
