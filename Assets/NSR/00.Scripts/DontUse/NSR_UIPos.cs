using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_UIPos : MonoBehaviour
{
    public Transform pos;

    // Update is called once per frame
    void Update()
    {
        //1. ī�޶���ġ, ī�޶�չ������� ������ Ray �� �����
        Ray ray = new Ray(pos.position, pos.forward);
        //2. ���࿡ Ray �߻��ؼ� ��򰡿� �ε����ٸ�
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            print(hit.transform.name);
            //3. �ε��� ��ġ�� Crosshair (��) �� ���´�.
            transform.position = hit.point;
            //4. Crosshair �� ũ�⸦ ī�޶� - �ε�����ġ�� �Ÿ���ŭ ���Ѵ�.
            transform.localScale = new Vector3(1, 1, 1) * hit.distance;
        }
    }
}
