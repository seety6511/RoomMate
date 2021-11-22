using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SH_GimmickState
{
    None,
    Active,     //�ܹ߼�
    Activating, //���Ӽ�
    Waiting,    //���Ӽ�
    Hovering,   //���Ӽ�
    Clear,      //�ܹ߼�
    Disable,    //�ܹ߼�
}

//��� �θ�Ŭ����
//��� ����� �� ����� ��� �޾ƾ� �Ѵ�.
//Active()�� �ܹ߼� ȿ���� ���� �޼ҵ�.
//Activating() �� ���Ӽ� ȿ���� ���� �޼ҵ�.
[RequireComponent(typeof(SH_Gimmick_ModelStateMachine))]
[RequireComponent(typeof(SH_Gimmick_EffectController))]
[RequireComponent(typeof(SH_Gimmick_SoundController))]
public class SH_Gimmick : MonoBehaviour
{
    public SH_GimmickState gimmickState;

    public float activeCoolTime;    //�ѹ� �۵���Ų�� �ٽ� �۵���Ű�� ���� �ʿ��� �ð�
    public float activatingOffTime; //�ѹ� �۵���Ų�� �ٽ� �����·� ���ư��� ���� �ʿ��� �ð�
    public float reloadTime;        //������ �ߴܵǾ�����, �����·� ���ư������ �ʿ��� �ð�

    SH_Gimmick_SoundController soundController;
    SH_Gimmick_EffectController effectController;
    SH_Gimmick_ModelStateMachine modelController;

    //����� �⺻ �ʱ�ȭ
    protected virtual void Awake()
    {
        activeCoolTimeCheck = true;
        soundController = GetComponent<SH_Gimmick_SoundController>();
        effectController = GetComponent<SH_Gimmick_EffectController>();
        modelController = GetComponent<SH_Gimmick_ModelStateMachine>();
        soundController.Init();
        effectController.Init();
        modelController.Init();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    public void StateChange(SH_GimmickState state, bool force = false)
    {
        if (force)
        {
            gimmickState = state;
            modelController.StateUpdate();
            soundController.StateUpdate();
            effectController.StateUpdate();
            return;
        }

        switch (state)
        {
            case SH_GimmickState.Active:
                break;

            case SH_GimmickState.Activating:
                break;

            case SH_GimmickState.Waiting:
                switch (gimmickState)
                {
                    case SH_GimmickState.Active:
                    case SH_GimmickState.Activating:
                    case SH_GimmickState.Clear:
                    case SH_GimmickState.Disable:
                        return;
                }
                break;

            case SH_GimmickState.Disable:
                break;

            case SH_GimmickState.Hovering:
                if (gimmickState != SH_GimmickState.Waiting)
                    return;
                break;

            case SH_GimmickState.Clear:
                break;

            case SH_GimmickState.None:
                break;
        }

        gimmickState = state;
        modelController.StateUpdate();
        soundController.StateUpdate();
        effectController.StateUpdate();
    }

    bool activeCoolTimeCheck;
    public virtual void Active()
    {
        if (!activeCoolTimeCheck)
            return;
            
        StartCoroutine(ActiveCoolTimeCheck());
    }

    public virtual IEnumerator SpecialEffect()
    {
        yield return null;
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
        StateChange(SH_GimmickState.Active);
        StartCoroutine(SpecialEffect());
        Debug.Log("Active");
        check1 = false;
    }

    public virtual void Activating()
    {
    }

    public virtual void Disable()
    {

    }

    public virtual void Clear()
    {
    }

    public virtual void Waiting()
    {
    }

    public virtual void Hovering()
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
        Disable();
    }

    protected virtual void OnCollisionEnter()
    {
    }

    protected virtual void OnCollisionStay()
    {
    }

    protected virtual void OnCollisionExit()
    {
        Disable();
    }
}