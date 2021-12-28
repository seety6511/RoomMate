using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_LampTester : MonoBehaviour
{
    public SH_LampControl[] lcs;

    int index;

    private void Start()
    {
        index = 0;
        lcs = FindObjectsOfType<SH_LampControl>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            lcs[index].OnOff();
            index++;
            if (index >= lcs.Length)
                index = 0;
        }
    }
}
