using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Player : MonoBehaviour
{
    public SH_Gimmick gim;
    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, LayerMask.NameToLayer("Gimmick")))
        {
            gim = hit.collider.GetComponent<SH_Gimmick>();

            if (Input.GetMouseButtonDown(0))
                hit.collider.GetComponent<SH_Gimmick>().Active();
            else
                gim.StateChange(SH_GimmickState.Hovering);
        }
        else
        {
            if (gim != null)
            {
                gim.StateChange(SH_GimmickState.Waiting);
                gim = null;
            }
        }
    }
}
