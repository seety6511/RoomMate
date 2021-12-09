using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_OnOff : MonoBehaviour
{
    public void OnOff()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
