using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSRColl : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print(gameObject.name + "�� �ε��� ���� : " + collision.gameObject.name);
    }
}
