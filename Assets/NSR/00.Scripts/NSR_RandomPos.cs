using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] 
public class PontArray
{
    public List<Sprite> ponts;
}
public class NSR_RandomPos : MonoBehaviour
{
    float currTime;
    public float posChangeTime = 3;
    Image image;
    public List<PontArray> changeTexts;

    int idx = 0;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        SetText();
        currTime = 0;
    }
    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime > posChangeTime)
        {
            SetText();
            currTime = 0;
        }

        //print("x : " + transform.localPosition.x);
        //print("y : " + transform.localPosition.y);
        //print("z : " + transform.localPosition.z);
    }

    void SetText()
    {
        image.sprite = changeTexts[idx].ponts[Random.Range(0, changeTexts[idx].ponts.Count)];
        idx++;
        transform.localPosition = new Vector3(Random.Range(-120, 120), Random.Range(-140, 140), Random.Range(320, 370));

        image.material.color = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        if (idx >= changeTexts.Count)
            idx = 0;
    }
}
