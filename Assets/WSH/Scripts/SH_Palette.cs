using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Palette : MonoBehaviour
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
        Transform cup = null;
        switch (tube.GetColor())
        {
            case SH_PaintTube.PaintColor.Red:
                cup = redPaint.transform;
                break;
            case SH_PaintTube.PaintColor.Green:
                cup = greenPaint.transform;
                break;
            case SH_PaintTube.PaintColor.Blue:
                cup = bluePaint.transform;
                break;
            case SH_PaintTube.PaintColor.White:
                cup = whitePaint.transform;
                break;
            case SH_PaintTube.PaintColor.Black:
                cup = blackPaint.transform;
                break;
        }
        cup.gameObject.SetActive(true);
        tube.Open(cup);
    }
}
