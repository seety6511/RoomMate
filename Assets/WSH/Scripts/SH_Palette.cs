using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SH_Palette : MonoBehaviourPun
{
    [SerializeField]
    GameObject redPaint;
    [SerializeField]
    GameObject greenPaint;
    [SerializeField]
    GameObject bluePaint;
    [SerializeField]
    GameObject whitePaint;
    [SerializeField]
    GameObject blackPaint;

    private void OnTriggerEnter(Collider other)
    {
        var tube = other.gameObject.GetComponent<SH_PaintTube>();
        if (tube == null)
            return;

        if (tube.GetAlreadyOpen())
            return;

        int i = tube.i;
        photonView.RPC("Rpc_OnTriggerEnter", RpcTarget.Others, i);
        Transform cup = null;
        switch (tube.GetColor())
        {
            case PaintColor.Red:
                cup = redPaint.transform;
                break;
            case PaintColor.Green:
                cup = greenPaint.transform;
                break;
            case PaintColor.Blue:
                cup = bluePaint.transform;
                break;
            case PaintColor.White:
                cup = whitePaint.transform;
                break;
            case PaintColor.Black:
                cup = blackPaint.transform;
                break;
        }
        cup.gameObject.SetActive(true);
        tube.Open(cup);
    }

    [PunRPC]
    void Rpc_OnTriggerEnter(int i)
    {
        var tube = GetComponentInParent<NSR_PaintPuzzle>().caps[i].gameObject.GetComponent< SH_PaintTube>();

        Transform cup = null;
        switch (tube.GetColor())
        {
            case PaintColor.Red:
                cup = redPaint.transform;
                break;
            case PaintColor.Green:
                cup = greenPaint.transform;
                break;
            case PaintColor.Blue:
                cup = bluePaint.transform;
                break;
            case PaintColor.White:
                cup = whitePaint.transform;
                break;
            case PaintColor.Black:
                cup = blackPaint.transform;
                break;
        }
        cup.gameObject.SetActive(true);
        tube.Open(cup);
    }
}