using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHJ_Hint : SH_Hint
{
    void Start()
    {
        hintManager = FindObjectOfType<SH_HintManager>();
        infoIndex = 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        return;
    }
}
