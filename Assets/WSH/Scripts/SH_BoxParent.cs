using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_BoxParent : MonoBehaviour
{
    [SerializeField]
    GameObject box;

    public void On()
    {
        box.SetActive(true);
    }
}
