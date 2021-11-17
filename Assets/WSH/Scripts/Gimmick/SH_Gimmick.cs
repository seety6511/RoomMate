using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SH_GimmickState
{
    None,
    Active,     //단발성
    Activating, //연속성
    Waiting,    //연속성
    Hovering,   //연속성
    Clear,      //단발성
    Disable,    //단발성
    Reload,     //연속성
}

//기믹 부모클래스
//모든 기믹은 이 기믹을 상속 받아야 한다.
//Active()는 단발성 효과를 위한 메소드.
//Activating() 은 연속성 효과를 위한 메소드.
[RequireComponent(typeof(SH_Gimmick_SoundController))]
[RequireComponent(typeof(SH_Gimmick_EffectController))]
[RequireComponent(typeof(SH_Gimmick_ModelStateMachine))]
public class SH_Gimmick : MonoBehaviour
{
    public SH_GimmickState gimmickState;
    public List<SH_Gimmick> password = new List<SH_Gimmick>();    //이 리스트의 모든 기믹이 clear 상태여야 이 기믹을 조작가능하다.
    public bool hasActive;          //현재 활성화 상태인가?
    public bool keepState;          //활성화 상태를 유지 할것인가?
    public float activeCoolTime;    //한번 작동시킨후 다시 작동시키기 위해 필요한 시간
    public float activatingOffTime; //한번 작동시킨후 다시 원상태로 돌아가기 위해 필요한 시간
    public float reloadTime;        //조작이 중단되었을때, 원상태로 돌아가기까지 필요한 시간
    public float reloadTimer;

    SH_Gimmick_SoundController soundController;
    SH_Gimmick_EffectController effectController;
    SH_Gimmick_ModelStateMachine modelController;

    #region Protected Interfaces
    //기믹의 기본 초기화
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

    //재장전시 발생하는 이벤트
    protected virtual IEnumerator ReloadEvent()
    {
        StateChange(SH_GimmickState.Waiting, true);
        yield return null;
    }
    #endregion

    #region Public Interfaces
    /// <summary>
    /// 현재 기믹의 상태를 변경할때 사용한다. 반드시 이것을 통해서 상태를 변경시켜야 한다.<br/>
    /// 상태를 강제로 변환시킬경우 force == true
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