using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
using Photon.Pun;


// viewId 로 충돌물체 넘겨주기
public class SH_BoardChanger : MonoBehaviourPun
{
    public Transform easel;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EmptyBoard" || other.tag == "EquipBoard")
        {
            SH_SketchBoard board = other.GetComponent<SH_SketchBoard>();
            int i = board.i;
            board.onEasel = true;
            photonView.RPC("Rpc_OnEasel", RpcTarget.Others, i, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "EmptyBoard")
        {
            other.GetComponent<SH_SketchBoard>().onEasel = false;
            photonView.RPC("Rpc_OnEasel", RpcTarget.Others, other, false);
        }
    }


    [PunRPC]
    void Rpc_OnEasel(int i, bool onEasel)
    {
        Collider other =  GetComponentInParent<NSR_PaintPuzzle>().papers[i];
        other.GetComponent<SH_SketchBoard>().onEasel = onEasel;
    }
}
