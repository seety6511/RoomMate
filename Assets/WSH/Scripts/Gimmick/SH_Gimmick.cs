using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SH_Layer
{
    Gimmick,
    Destroyer,
    Interacter,
    Finger
}

public enum SH_GimmickState
{
    Waiting,    //연속성
    Hovering,   //연속성
    Active,     //단발성
    Activating, //연속성
    Clear,      //단발성
    Disable,    //단발성. 이 상태면 아무 상호작용을 하지 않는다.
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
    public SH_Layer interactiveLayer;  //이 레이어의 콜라이더로만 조작 가능하다. 기본값 : LayerMask.NameToLayer("Interacter")
    public SH_GimmickState gimmickState;
    public List<SH_Gimmick> password = new List<SH_Gimmick>();    //이 리스트의 모든 기믹이 clear 상태여야 이 기믹을 조작가능하다.
    public bool isActive;           //현재 활성화 상태인가?
    public bool keepState;          //활성화 상태를 유지 할것인가?
    public float activeCoolTime;    //한번 작동시킨후 다시 작동시키기 위해 필요한 시간
    public float reloadTime;        //조작이 중단되었을때, 원상태로 돌아가기까지 필요한 시간
    protected float reloadTimer;

    public OVRInput.Button vrKey;   //이 버튼에만 작동한다.(vr)
    public KeyCode pcKey;           //이 버튼에만 작동한다(pc)

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
        isActive = false;
        pcKey = KeyCode.Mouse0;
        StateUpdate();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        PasswordCheck();

        switch (gimmickState)
        {
            case SH_GimmickState.Waiting:
                Waiting();
                break;

            case SH_GimmickState.Disable:
                Disable();
                return;
        }

        Reload();
    }

    void Reload()
    {
        if (gimmickState == SH_GimmickState.Waiting || gimmickState == SH_GimmickState.Hovering)
            return;

        if (keepState)
            return;

        if (!isActive)
            return;

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
        StateChange(SH_GimmickState.Waiting);
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
    protected void StateChange(SH_GimmickState state, bool force = false)
    {
        if (gimmickState == SH_GimmickState.Disable)
            return;

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
                if (triggerStay)
                {
                    StateChange(SH_GimmickState.Hovering);
                    return;
                }

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
                //if (gimmickState != SH_GimmickState.Waiting)
                //    return;
                break;

            case SH_GimmickState.Clear:
                break;

        }
        gimmickState = state;
        StateUpdate();
    }

    protected virtual void Active()
    {
        if (!activeCoolTimeCheck)
            return;

        StartCoroutine(ActiveCheck());
    }

    protected virtual IEnumerator ActiveEffect()
    {
        yield return null;
    }

    protected virtual void Activating()
    {
        Debug.Log("A");
        StateChange(SH_GimmickState.Activating);
        StateUpdate();
        StartCoroutine(ActivatingEffect());
    }

    protected virtual IEnumerator ActivatingEffect()
    {
        yield return null;
    }

    protected virtual void Disable()
    {
        StateChange(SH_GimmickState.Disable);
    }

    protected virtual void Clear()
    {
        StateChange(SH_GimmickState.Clear);
    }

    protected virtual void Waiting()
    {
        StateChange(SH_GimmickState.Waiting);
    }

    protected virtual void Hovering()
    {
        StateChange(SH_GimmickState.Hovering);
    }

    bool triggerStay;
    float inputTime;
    bool alreadyInputWaiting;
    IEnumerator InputWaiting()
    {
        if (gimmickState == SH_GimmickState.Disable)
            yield break;

        if (!triggerStay)
            yield break;

        if (alreadyInputWaiting)
            yield break;

        alreadyInputWaiting = true;
        inputTime = 0.5f;
        float inputTimer = 0f;
        while (triggerStay)
        {
            if(Input.GetKeyDown(pcKey))
                Active();
            else if(Input.GetKey(pcKey))
                inputTimer += Time.deltaTime;

            if (OVRInput.GetDown(vrKey))
                Active();
            else if (OVRInput.Get(vrKey))
                inputTimer += Time.deltaTime;

            if (inputTime <= inputTimer)
            {
                Activating();

                if (Input.GetKeyUp(pcKey))
                    break;

                if (OVRInput.GetUp(vrKey))
                    break;
            }
            yield return null;
        }
        alreadyInputWaiting = false;
    }

    #endregion

    #region Physhics
    /// <summary>
    /// 상호작용 가능한 상태면 true, 아니면 false
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    bool InteractibleCheck(Collider col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer(interactiveLayer.ToString()))
            return false;

        if (gimmickState == SH_GimmickState.Disable)
            return false;

        return true;
    }

    bool InteractibleCheck(Collision col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer(interactiveLayer.ToString()))
            return false;

        if (gimmickState == SH_GimmickState.Disable)
            return false;

        return true;
    }
    protected virtual void OnTriggerEnter(Collider col)
    {
        if (!InteractibleCheck(col))
            return;
        else
            triggerStay = true;

        if (gimmickState == SH_GimmickState.Waiting)
            Hovering();
    }

    protected virtual void OnTriggerStay(Collider col)
    {
        if (!InteractibleCheck(col))
            return;

        triggerStay = true;
        StartCoroutine(InputWaiting());
    }

    protected virtual void OnTriggerExit(Collider col)
    {
        if (!InteractibleCheck(col))
            return;
        else
            triggerStay = false;

        if (InteractibleCheck(col))
        {
            if (!keepState || gimmickState == SH_GimmickState.Hovering)
                Waiting();
        }
    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (!InteractibleCheck(col))
            return;
        else
            triggerStay = true;

        if (gimmickState == SH_GimmickState.Waiting)
            Hovering();
    }

    protected virtual void OnCollisionStay(Collision col)
    {
        if (!InteractibleCheck(col))
            return;
    }

    protected virtual void OnCollisionExit(Collision col)
    {
        if (!InteractibleCheck(col))
            return;

        triggerStay = false;
        if (!keepState || gimmickState == SH_GimmickState.Hovering)
            Waiting();
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
            {
                StateChange(SH_GimmickState.Disable);
                return false;
            }
        }

        if (gimmickState == SH_GimmickState.Disable)
            StateChange(SH_GimmickState.Waiting);

        return true;
    }

    IEnumerator ActiveCheck()
    {
        if (!activeCoolTimeCheck)
            yield break;

        activeCoolTimer = 0f;
        //Debug.Log("Active");
        isActive = true;
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