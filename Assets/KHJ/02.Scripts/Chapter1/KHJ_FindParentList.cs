using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHJ_FindParentList : MonoBehaviour
{
    private void Start()
    {
        FindParent(gameObject);
    }

    public List<string> parents;
    void FindParent(GameObject obj)
    {
        if (obj.name == "RobotHand (R) (OVR)")
            return;
        if (obj.transform.parent == null)
        {
            return;
        }
        else
        {
            parents.Add(obj.transform.parent.name);
            FindParent(obj.transform.parent.gameObject);
        }
    }
}
