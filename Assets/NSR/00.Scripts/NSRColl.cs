using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSRColl : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print(gameObject.name + "°ú ºÎµúÈù ¹°°Ç : " + collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(gameObject.name + "°ú ºÎµúÈù ¹°°Ç : " + other.gameObject.name);
    }
}
