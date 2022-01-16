using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//팔레트에 묻어있는 페인트
public class SH_Paint : MonoBehaviour
{
    [SerializeField]
    PaintColor color;

    public PaintColor GetColor => color;
}
