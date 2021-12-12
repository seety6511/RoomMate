using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_ControlLock : MonoBehaviour
{
    public List<GameObject> targetObjs;
    public bool unlock;

    private void Awake()
    {
        LockControl(unlock);
    }

    /// <summary>
    /// true = Ȱ��ȭ, false = ��Ȱ��ȭ
    /// </summary>
    /// <param name="value"></param>
    public void LockControl(bool value)
    {
        foreach (var o in targetObjs)
            o.SetActive(value);
    }

}
