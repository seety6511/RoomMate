using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSRColl : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print(gameObject.name + "°ú ºÎµúÈù ¹°°Ç : " + collision.gameObject.name);
    }
}
