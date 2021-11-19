using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MText;
public class SH_InputField : MonoBehaviour
{
    public Modular3DText inputField;
    private void Awake()
    {
        inputField = GetComponent<Modular3DText>();
    }
}
