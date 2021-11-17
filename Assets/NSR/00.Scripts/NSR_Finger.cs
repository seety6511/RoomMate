using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_Finger : MonoBehaviour
{
    private void Start()
    {
        print(transform.gameObject.layer);
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.transform.gameObject.layer == 7)
        {

        }
    }
}
