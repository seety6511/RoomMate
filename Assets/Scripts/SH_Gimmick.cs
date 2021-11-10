using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//기믹 부모클래스
//모든 기믹은 이 기믹을 상속 받아야 한다.
//Active()는 단발성 효과를 위한 메소드.
//Activating() 은 연속성 효과를 위한 메소드.
[RequireComponent(typeof(Collider))]
public class SH_Gimmick : MonoBehaviour
{
    public float activeCoolTime;    //한번 작동시킨후 다시 작동시키기 위해 필요한 시간
    public float activatingOffTime; //한번 작동시킨후 다시 원상태로 돌아가기 위해 필요한 시간
    public GameObject activeEffect;

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
    }

    bool activeCoolTimeCheck;
    public virtual void Active()
    {
        if (!activeCoolTimeCheck)
            return;

        StartCoroutine(ActiveCoolTimeCheck());

        activeEffect.SetActive(true);
    }

    bool check1;
    IEnumerator ActiveCoolTimeCheck()
    {
        if (check1)
            yield break;

        check1 = true;

        float timer = 0f;
        activeCoolTimeCheck = false;
        while (activeCoolTime > timer)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        activeCoolTimeCheck = true;

        check1 = false;
    }

    public virtual void Activating()
    {
    }

    protected virtual void OnTriggerEnter()
    {
    }

    protected virtual void OnTriggerStay()
    {
    }

    protected virtual void OnTriggerExit()
    {
    }

    protected virtual void OnCollisionEnter()
    {
    }

    protected virtual void OnCollisionStay()
    {
    }

    protected virtual void OnCollisionExit()
    {
    }
}
