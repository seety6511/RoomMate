using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_RandomPos : MonoBehaviour
{
    float currTime;
    public float posChangeTime;

    private void OnDisable()
    {
        currTime = 0;
    }
    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime > posChangeTime)
        {

            transform.localPosition = new Vector3(Random.Range(-147, 147), Random.Range(-154, 154), Random.Range(280, 320));
            currTime = 0;
        }

        //print("x : " + transform.localPosition.x);
        //print("y : " + transform.localPosition.y);
        //print("z : " + transform.localPosition.z);
    }
}
