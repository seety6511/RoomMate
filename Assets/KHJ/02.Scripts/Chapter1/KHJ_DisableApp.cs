using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHJ_DisableApp : MonoBehaviour
{
    public bool enable;
    public GameObject Warning;
    private void Update()
    {
        if(enable == false)
        {
            StartCoroutine(warning());
            enable = true;
        }
    }
    IEnumerator warning()
    {
        yield return new WaitForSeconds(0.4f);
        Warning.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        enable = false;
        Warning.gameObject.SetActive(false);
    }
}
