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
    Reload,     //���Ӽ�
}

//��� �θ�Ŭ����
//��� ����� �� ����� ��� �޾ƾ� �Ѵ�.
//Active()�� �ܹ߼� ȿ���� ���� �޼ҵ�.
//Activating() �� ���Ӽ� ȿ���� ���� �޼ҵ�.
[RequireComponent(typeof(SH_Gimmick_SoundController))]
[RequireComponent(typeof(SH_Gimmick_EffectController))]
[RequireComponent(typeof(SH_Gimmick_ModelStateMachine))]
public class SH_Gimmick : MonoBehaviour
{
    public SH_GimmickState gimmickState;

    public float activeCoolTime;    //�ѹ� �۵���Ų�� �ٽ� �۵���Ű�� ���� �ʿ��� �ð�
    public float activatingOffTime; //�ѹ� �۵���Ų�� �ٽ� �����·� ���ư��� ���� �ʿ��� �ð�
    public float reloadTime;        //������ �ߴܵǾ�����, �����·� ���ư������ �ʿ��� �ð�
    protected float reloadTimer;

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
        modelController.Init();
        effectController.Init();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    protected virtual void Reloading()
    {
        reloadTimer += Time.deltaTime;
        if (reloadTimer >= reloadTime)
        {
            reloadTimer = 0f;
            StartCoroutine(ReloadEvent());
        }
    }

    //�������� �߻��ϴ� �̺�Ʈ
    protected virtual IEnumerator ReloadEvent()
    {
        StateChange(SH_GimmickState.Waiting, true);
        yield return null;
    }

    /// <summary>
    /// ���� ����� ���¸� �����Ҷ� ����Ѵ�. �ݵ�� �̰��� ���ؼ� ���¸� ������Ѿ� �Ѵ�.<br/>
    /// ���¸� ������ ��ȯ��ų��� force == true
    /// </summary>
    /// <param name="state"></param>
    /// <param name="force"></param>
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