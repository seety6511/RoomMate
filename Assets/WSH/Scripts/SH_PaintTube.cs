using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PaintColor
{
    Red,
    Green,
    Blue,
    White,
    Black
}
public class SH_PaintTube : MonoBehaviour
{
    [SerializeField]
    PaintColor color;
    [SerializeField]
    bool alreadyOpen;
    [SerializeField]
    GameObject particle;
    public PaintColor GetColor()
    {
        return color;
    }

    public bool GetAlreadyOpen()
    {
        return alreadyOpen;
    }
    public void Open(Transform cup)
    {
        alreadyOpen = true;
        GetComponent<Rigidbody>().isKinematic = false;
        particle.SetActive(true);
        particle.transform.LookAt(cup);
        Destroy(gameObject, 1f);
    }
}
