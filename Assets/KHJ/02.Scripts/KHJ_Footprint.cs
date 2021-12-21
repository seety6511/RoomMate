using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHJ_Footprint : MonoBehaviour
{
    public KHJ_TriggerNameCheck[] names;
    public bool hand_R;
    public bool hand_L;
    public bool foot;

    public bool isSolved = false;
    void Start()
    {
        names = GetComponentsInChildren<KHJ_TriggerNameCheck>();
    }
    public void AnswerCheck()
    {
        if (isSolved)
            return;
        if (InputCheck())
        {
            SolveEffect();
        }
    }
    bool InputCheck()
    {
        hand_R = names[0].OnTouch;
        hand_L = names[1].OnTouch;
        foot = names[2].OnTouch;
        if (hand_R && hand_L)
        {
            return true;
        }
        else
            return false;
    }


    void SolveEffect()
    {
        Destroy(gameObject);
    }
}
