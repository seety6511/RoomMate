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
    public List<SH_Gimmick> password = new List<SH_Gimmick>();    //�� ����Ʈ�� ��� ����� clear ���¿��� �� ����� ���۰����ϴ�.
    public bool hasActive;          //���� Ȱ��ȭ �����ΰ�?
    public bool keepState;          //Ȱ��ȭ ���¸� ���� �Ұ��ΰ�?
    public float activeCoolTime;    //�ѹ� �۵���Ų�� �ٽ� �۵���Ű�� ���� �ʿ��� �ð�
    public float activatingOffTime; //�ѹ� �۵���Ų�� �ٽ� �����·� ���ư��� ���� �ʿ��� �ð�
    public float reloadTime;        //������ �ߴܵǾ�����, �����·� ���ư������ �ʿ��� �ð�
    public float reloadTimer;

    SH_Gimmick_SoundController soundController;
    SH_Gimmick_EffectController effectController;
    SH_Gimmick_ModelStateMachine modelController;

    #region Protected Interfaces
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
        hasActive = false;
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        switch (gimmickState)
        {
            case SH_GimmickState.Hovering:
                Hovering();
                break;
            case SH_GimmickState.Waiting:
                Waiting();
                break;
        }

        if (keepState)
            return;

        if (!hasActive)
            return;

        Reloading();
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
    #endregion

    #region Public Interfaces
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
            StateUpdate();
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
        StateUpdate();
    }

    public virtual void Active()
    {
        if (!activeCoolTimeCheck)
            return;

        StartCoroutine(ActiveCheck());
    }

    public virtual IEnumerator ActiveEffect()
    {
        yield return null;
    }

    public virtual void Activating()
    {
        StateChange(SH_GimmickState.Activating);
        StateUpdate();
        StartCoroutine(ActivatingEffect());
    }

    protected virtual IEnumerator ActivatingEffect()
    {
        yield return null;
    }

    public virtual void Disable()
    {
        StateUpdate();
    }

    public virtual void Clear()
    {
        StateUpdate();

    }

    public virtual void Waiting()
    {
        StateUpdate();
    }

    public virtual void Hovering()
    {
        StateUpdate();
    }
    #endregion

    #region Physhics
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
    #endregion

    #region System

    bool activeCoolTimeCheck;
    float activeCoolTimer;
    IEnumerator ActiveCoolTimeCheck()
    {
        if (!activeCoolTimeCheck)
            yield break;

        activeCoolTimeCheck = false;

        while (activeCoolTime > activeCoolTimer)
        {
            activeCoolTimer += Time.deltaTime;
            yield return null;
        }

        activeCoolTimeCheck = true;
    }

    bool PasswordCheck()
    {
        foreach (var p in password)
        {
            if (p.gimmickState != SH_GimmickState.Clear)
                return false;
        }
        return true;
    }

    IEnumerator ActiveCheck()
    {
        if (!PasswordCheck())
            yield break;

        if (!activeCoolTimeCheck)
            yield break;

        activeCoolTimer = 0f;
        StateChange(SH_GimmickState.Active);
        StartCoroutine(ActiveEffect());
        StartCoroutine(ActiveCoolTimeCheck());
    }

    void StateUpdate()
    {
        modelController.StateUpdate();
        soundController.StateUpdate();
        effectController.StateUpdate();
    }
    #endregion
}