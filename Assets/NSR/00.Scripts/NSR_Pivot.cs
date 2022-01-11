using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_Pivot : MonoBehaviour
{
    //public GameObject mainPivot;
    [HideInInspector]
    public bool active;

    public GameObject[] Pivots;

    //public bool isMainPivot;
    private void OnEnable()
    {
        active = true;
    }

    private void OnDisable()
    {
        active = true;
    }
}
