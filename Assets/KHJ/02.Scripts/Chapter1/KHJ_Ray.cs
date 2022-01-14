using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHJ_Ray : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DrawRay();
    }

    // Update is called once per frame
    void Update()
    {
        DrawRay();
    }

    public Vector3 tmp;
    void DrawRay()
    {
        for(int i = 0; i < 6; i++)
        {
            tmp = new Vector3(2 * Mathf.Cos((60 * i) * (Mathf.PI / 180)),
                0, 2 * Mathf.Sin((60 * i) * (Mathf.PI / 180)));

            Vector3 dir = transform.localPosition - tmp;
            Debug.DrawRay(transform.position, dir * 15, Color.red);
        }        
    }
}
