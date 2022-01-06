using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class NSR_Hand : MonoBehaviour
{
    public GameObject Pivot;
    public GameObject targetObj;
    public GameObject indexPivot;

    Grabbable grabObj;

    Hand hand;

    public float distance;

    private void Start()
    {
        hand = GetComponent<Hand>();
    }

    private void Update()
    {
        grabObj = hand.holdingObj;

        float dis;
        dis = Vector3.Distance(transform.position, targetObj.transform.position);


        if(grabObj == null && dis < distance)
        {
            indexPivot.SetActive(true);
        }
        else
        {
            indexPivot.SetActive(false);
        }
    }
}
