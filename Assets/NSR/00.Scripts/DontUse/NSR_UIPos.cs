using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_UIPos : MonoBehaviour
{
    public Transform pos;

    // Update is called once per frame
    void Update()
    {
        //1. 카메라위치, 카메라앞방향으로 나가는 Ray 를 만든다
        Ray ray = new Ray(pos.position, pos.forward);
        //2. 만약에 Ray 발사해서 어딘가에 부딪혔다면
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            print(hit.transform.name);
            //3. 부딪힌 위치에 Crosshair (나) 를 놓는다.
            transform.position = hit.point;
            //4. Crosshair 의 크기를 카메라 - 부딪힌위치의 거리만큼 곱한다.
            transform.localScale = new Vector3(1, 1, 1) * hit.distance;
        }
    }
}
