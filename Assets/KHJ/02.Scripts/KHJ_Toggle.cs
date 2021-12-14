using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class KHJ_Toggle : MonoBehaviour
{
    public bool enable;
    public GameObject Target;
    public Image BG;
    public GameObject Warning;
    public GameObject Button;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;
        OnTrigger();
    }

    public void OnTrigger()
    {
        if (enable)
        {
            Target.SetActive(!Target.activeSelf);
            if (Target.activeSelf)
            {
                var c = new Color32(118,255,104,255);
                BG.DOColor(c, 0.4f);
                Button.transform.DOLocalMoveX(3, 0.4f);
            }
            else
            {
                var c = new Color32(255,146,128,255);
                BG.DOColor(c, 0.4f);
                Button.transform.DOLocalMoveX(-3, 0.4f);
            }
        }
        else
        {
            StartCoroutine(warning());
        }
    }
    IEnumerator warning()
    {
        Warning.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        Warning.gameObject.SetActive(false);

    }
}


