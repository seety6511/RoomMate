using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_LampRotate : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        float h = 0f;
        float v = 0f;
        if (Input.GetKey(KeyCode.Q))
            h = 1f;
        if (Input.GetKey(KeyCode.W))
            h = -1f;
        if (Input.GetKey(KeyCode.E))
            v = 1f;
        if (Input.GetKey(KeyCode.R))
            v = -1f;
        Rotate(h, v);
    }

    void Rotate(float h, float v)
    {
        transform.Rotate(new Vector3(h, 0,v) * Time.deltaTime * speed);
    }
}
