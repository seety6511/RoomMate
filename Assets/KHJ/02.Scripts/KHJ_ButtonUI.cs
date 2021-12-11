using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KHJ_ButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        GetComponent<Button>().onClick.Invoke();
    }
}
