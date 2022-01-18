using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;


// viewId �� �浹��ü �Ѱ��ֱ�
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
