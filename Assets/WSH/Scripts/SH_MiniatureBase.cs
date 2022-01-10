using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_MiniatureBase : MonoBehaviour
{
    public GameObject miniatureGridTilePrefab;

    public int width;
    public int height;
    private void Awake()
    {
        var scaleX = miniatureGridTilePrefab.transform.localScale.x;
        var scaleZ = miniatureGridTilePrefab.transform.localScale.z;
        for(int i = 0; i < width; ++i)
        {
            for(int j = 0; j < height; ++j)
            {
                var t = Instantiate(miniatureGridTilePrefab,transform);
                t.transform.localPosition = new Vector3(scaleX*i*4, 0, scaleZ*j*4);
            }
        }
    }
}
