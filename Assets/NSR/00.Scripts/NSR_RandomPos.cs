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

            transform.position = new Vector3(Random.Range(0.92f, 0.99f), Random.Range(-0.022f, 0.0239f), Random.Range(-1.1355f, -1.1318f));
            currTime = 0;
        }

        //print("x : " + transform.position.x);
        //print("y : " + transform.position.y);
        //print("z : " + transform.position.z);
    }
}
