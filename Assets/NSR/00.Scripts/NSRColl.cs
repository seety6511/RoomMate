using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSRColl : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print("ºÎµúÈù ¹°°Ç : " + collision.gameObject.name);
    }
}
