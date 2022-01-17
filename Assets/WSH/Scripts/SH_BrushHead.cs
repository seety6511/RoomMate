using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SH_BrushHead : MonoBehaviourPun
{
    public int i;

    [SerializeField]
    PaintColor buriedColor;

    public PaintColor GetColor => buriedColor;

    Material fur;

    private void Awake()
    {
        fur = GetComponent<MeshRenderer>().material;
        buriedColor = PaintColor.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        var paint = other.GetComponent<SH_Paint>();
        if (paint == null)
            return;
        //int i = paint.i;
        //photonView.RPC("Rpc_OnTriggerEnter", RpcTarget.Others, i);

        switch (paint.GetColor)
        {
            case PaintColor.Red:
                fur.SetColor("_BaseColor",Color.red);
                buriedColor = PaintColor.Red;
                break;
            case PaintColor.Green:
                fur.SetColor("_BaseColor", Color.green);
                buriedColor = PaintColor.Green;
                break;
            case PaintColor.Blue:
                fur.SetColor("_BaseColor", Color.blue);
                buriedColor = PaintColor.Blue;
                break;
            case PaintColor.Black:
                fur.SetColor("_BaseColor", Color.black);
                buriedColor = PaintColor.Black;
                break;
            case PaintColor.White:
                fur.SetColor("_BaseColor", Color.white);
                buriedColor = PaintColor.White;
                break;
        }
    }

    [PunRPC]
    void Rpc_OnTriggerEnter(int i)
    {
        Collider other = GetComponentInParent<NSR_PaintPuzzle>().paints[i];
        var paint = other.GetComponent<SH_Paint>();
        switch (paint.GetColor)
        {
            case PaintColor.Red:
                fur.SetColor("_BaseColor", Color.red);
                buriedColor = PaintColor.Red;
                break;
            case PaintColor.Green:
                fur.SetColor("_BaseColor", Color.green);
                buriedColor = PaintColor.Green;
                break;
            case PaintColor.Blue:
                fur.SetColor("_BaseColor", Color.blue);
                buriedColor = PaintColor.Blue;
                break;
            case PaintColor.Black:
                fur.SetColor("_BaseColor", Color.black);
                buriedColor = PaintColor.Black;
                break;
            case PaintColor.White:
                fur.SetColor("_BaseColor", Color.white);
                buriedColor = PaintColor.White;
                break;
        }
    }
}
