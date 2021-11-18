using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_TestHand : MonoBehaviour
{
    public float speed;
    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        float y = 0;

        if (Input.GetKey(KeyCode.Q))
            y = 1;
        else if (Input.GetKey(KeyCode.E))
            y = -1;

        var dir = new Vector3(h, y, v);
        transform.position += dir * Time.deltaTime * speed;
    }
}
