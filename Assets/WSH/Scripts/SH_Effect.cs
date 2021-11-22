using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Effect : MonoBehaviour
{
    public float lifeTime;
    float timer = 0f;

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (lifeTime < timer)
            Destroy(gameObject);
    }
}
