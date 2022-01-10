using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_Pivot : MonoBehaviour
{
    public GameObject mainPivot;
    public bool active;

    public GameObject[] Pivots;
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

    private void Update()
    {
        for(int i = 0; i < Pivots.Length; i++)
        {
            if (Pivots[i].activeSelf)
            {
                gameObject.SetActive(false);
                break;
            }
        }
    }

}
