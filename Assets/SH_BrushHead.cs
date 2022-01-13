using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_BrushHead : MonoBehaviour
{
    [SerializeField]
    PaintColor buriedColor;

    Material fur;

    private void Awake()
    {
        fur = GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        var paint = other.GetComponent<SH_Paint>();
        if (paint == null)
            return;

        switch (paint.GetColor)
        {
            case PaintColor.Red:
                fur.SetColor("Color",Color.red);
                break;
            case PaintColor.Green:
                fur.SetColor("Color",Color.green);
                break;
            case PaintColor.Blue:
                fur.SetColor("Color",Color.blue);
                break;
            case PaintColor.Black:
                fur.SetColor("Color",Color.black);
                break;
            case PaintColor.White:
                fur.SetColor("Color",Color.white);
                break;
        }
    }
}
