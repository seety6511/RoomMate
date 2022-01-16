using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class SH_BoardChanger : MonoBehaviour
{
    public Transform easel;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EmptyBoard" || other.tag == "EquipBoard")
        {
            other.GetComponent<SH_SketchBoard>().onEasel = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "EmptyBoard")
        {
            other.GetComponent<SH_SketchBoard>().onEasel = false;
        }
    }
}
