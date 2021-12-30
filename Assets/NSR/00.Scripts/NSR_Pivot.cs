using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_Pivot : MonoBehaviour
{
    public GameObject mainPivot;
    public bool active;
    private void OnEnable()
    {
        active = true;
        mainPivot.SetActive(false);
    }

    private void OnDisable()
    {
        active = true;
        mainPivot.SetActive(true);
    }

}
