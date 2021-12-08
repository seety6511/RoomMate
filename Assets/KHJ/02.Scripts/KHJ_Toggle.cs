using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KHJ_Toggle : MonoBehaviour
{

    public GameObject Target;
    public Image BG;
    public GameObject Button;
    public GameObject On;
    public GameObject Off;
    public void OnTrigger()
    {
        Target.SetActive(!Target.activeSelf);
        if (Target.activeSelf)
        {
            var c = new Color32();
            c.r = 118;
            c.g = 255;
            c.b = 104;
            c.a = 255;
            BG.color = c;
            Button.transform.localPosition = new Vector3(3, 0, 0);
        }
        else
        {
            var c = new Color32();
            c.r = 255;
            c.g = 146;
            c.b = 128;
            c.a = 255;
            BG.color = c;
            Button.transform.localPosition = new Vector3(-3, 0, 0);
        }
    }
    void Ddiyong(GameObject a)
    {
        iTween.MoveTo(a, iTween.Hash(
           "x", Screen.width * 0.5,
           "time", 0.5f,
           "easetype", iTween.EaseType.easeInOutBack
           ));
        iTween.ScaleTo(a, iTween.Hash(
            "x", 1,
            "y", 1,
            "z", 1,
            "time", 0.5f,
            "easetype", iTween.EaseType.easeInOutBack
            //"delay", 0.5f
            ));
    }
}
