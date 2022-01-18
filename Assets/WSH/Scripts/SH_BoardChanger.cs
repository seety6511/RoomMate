using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;


// viewId 로 충돌물체 넘겨주기
public class SH_BoardChanger : MonoBehaviour
{
    public Transform easel;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EmptyBoard" || other.tag == "EquipBoard")
        {
            SH_SketchBoard board = other.GetComponent<SH_SketchBoard>();
            int i = board.i;
            board.onEasel = true;
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
