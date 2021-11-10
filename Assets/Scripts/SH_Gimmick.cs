using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��� �θ�Ŭ����
//��� ����� �� ����� ��� �޾ƾ� �Ѵ�.
//Active()�� �ܹ߼� ȿ���� ���� �޼ҵ�.
//Activating() �� ���Ӽ� ȿ���� ���� �޼ҵ�.
[RequireComponent(typeof(Collider))]
public class SH_Gimmick : MonoBehaviour
{
    public float activeCoolTime;    //�ѹ� �۵���Ų�� �ٽ� �۵���Ű�� ���� �ʿ��� �ð�
    public float activatingOffTime; //�ѹ� �۵���Ų�� �ٽ� �����·� ���ư��� ���� �ʿ��� �ð�
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
