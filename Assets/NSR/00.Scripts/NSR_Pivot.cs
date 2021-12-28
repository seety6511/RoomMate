using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_Pivot : MonoBehaviour
{
    public GameObject mainPivot;

    private void OnEnable()
    {
        mainPivot.SetActive(false);
    }

    private void OnDisable()
    {
        mainPivot.SetActive(true);
    }
}
