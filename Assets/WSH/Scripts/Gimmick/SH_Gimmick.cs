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
    ActiveKeep,       //Active이후 바로 넘어가는 상태.
    ActivatingKeep,   //Activating 이후 바로 넘어가는 상태.
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
    public SH_GimmickState prevState;
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
        //Debug.Log("1");
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
        //Debug.Log("2");
    }

    protected virtual void Update()
    {
        if (!PasswordCheck())
            return;

        switch (gimmickState)
        {
            case SH_GimmickState.Waiting:
                Waiting();
                break;

            case SH_GimmickState.Hovering:
                Hovering();
                break;

            case SH_GimmickState.Active:
                Active();
                break;

            case SH_GimmickState.ActiveKeep:
                KeepActive();
                break;

            case SH_GimmickState.Activating:
                Activating();
                break;

            case SH_GimmickState.ActivatingKeep:
                KeepActivating();
                break;

            case SH_GimmickState.Disable:
                Disable();
                break;

            case SH_GimmickState.Clear:
                Clear();
                break;

            case SH_GimmickState.Reload:
                Reload();
                break;
        }
    }

    void KeepActivating()
    {
        //Debug.Log("Activating");

        StartCoroutine(InputWaiting());
        if (keepState)
            return;
        else
        {
            if (alreadyInput)
                return;
            else
                StateChange(SH_GimmickState.Reload);
        }
    }

    void KeepActive()
    {
        //Debug.Log("KeepActive");
        StartCoroutine(InputWaiting());

        if (keepState)
            return;
        else
            StateChange(SH_GimmickState.Reload);
    }

    #endregion

    #region Public Interfaces
    /// <summary>
    /// 현재 기믹의 상태를 변경할때 사용한다. 반드시 이것을 통해서 상태를 변경시켜야 한다.<br/>
    /// 상태를 강제로 변환시킬경우 force == true
    /// </summary>
    /// <param name="state"></param>
    /// <param name="force"></param>
    protected void StateChange(SH_GimmickState state, Collider col)
    {
        if (!InteractibleCheck(col))
            return;

        StateChange(state);
    }
    protected void StateChange(SH_GimmickState state, Collision col)
    {
        if (!InteractibleCheck(col))
            return;

        StateChange(state);
    }
    bool KeepStateCheck()
    {
        if (keepState)
        {
            if (gimmickState == SH_GimmickState.ActiveKeep)
                return true;
            if (gimmickState == SH_GimmickState.ActivatingKeep)
                return true;
        }
        return false;
    }
    protected void StateChange(SH_GimmickState state)
    {
        switch (state)
        {
            case SH_GimmickState.Waiting:
                if (KeepStateCheck())
                    return;
                break;

            case SH_GimmickState.Hovering:
                if (KeepStateCheck())
                    return;
                break;

            case SH_GimmickState.Active:
            case SH_GimmickState.ActiveKeep:
            case SH_GimmickState.Activating:
            case SH_GimmickState.ActivatingKeep:
                if (gimmickState == state)
                    return;
                break;

            case SH_GimmickState.Clear:
                break;

            case SH_GimmickState.Disable:   //비활성화 상태인 경우 상태접근이 불가능하다. 오로지 GimmickEnable()로 활성화가능
                return;

            case SH_GimmickState.Reload:
                break;
        }

        gimmickState = state;
        StateUpdate();
    }

    public void GimmickEnable()
    {
        gimmickState = SH_GimmickState.Waiting;
        StateUpdate();
    }

    protected virtual void Waiting()
    {
        //Debug.Log("Waiting : 6");
    }
    protected virtual void Hovering()
    {
        //Debug.Log("Hovering : 7");
        InputWaiting();
    }

    protected virtual void Active()
    {
        //Debug.Log("Active : 8");
        isActive = true;
        StateChange(SH_GimmickState.Active);
        StartCoroutine(ActiveEvent());
    }

    protected virtual IEnumerator ActiveEvent()
    {
        yield return null;
        //Debug.Log("9");
        StateChange(SH_GimmickState.ActiveKeep);
    }
    protected virtual void Activating()
    {
        //Debug.Log("Activating Start : 10");
        StartCoroutine(ActivatingEvent());
    }
    protected virtual IEnumerator ActivatingEvent()
    {
        yield return null;
        //Debug.Log("11");
        StateChange(SH_GimmickState.ActivatingKeep);
    }
    protected virtual void Disable()
    {
    }
    protected virtual void Clear()
    {
    }
    protected virtual void Reload()
    {
        StartCoroutine(ReloadEvent());
    }
    protected virtual IEnumerator ReloadEvent()
    {
        yield return null;
    }
    float inputTimer = 0f;
    float inputTime;
    bool alreadyInput;
    bool inputWait;
    IEnumerator InputWaiting()
    {
        inputTime = 1f;
        if (inputWait)
            yield break;
        inputWait = true;

        while(gimmickState != SH_GimmickState.Disable && inputWait)
        {
            if (Input.GetKeyDown(pcKey))
                StateChange(SH_GimmickState.Active);
            else if (Input.GetKey(pcKey))
                inputTimer += Time.deltaTime;
            else if (Input.GetKey(pcKey))
                inputWait = false;

            if (OVRInput.GetDown(vrKey))
                StateChange(SH_GimmickState.Active);
            else if (OVRInput.Get(vrKey))
                inputTimer += Time.deltaTime;
            else if (OVRInput.GetUp(vrKey))
                inputWait = false;

            if (inputTime <= inputTimer)
            {
                alreadyInput = true;
                StateChange(SH_GimmickState.Activating);

                if (Input.GetKeyUp(pcKey))
                {
                    StateChange(SH_GimmickState.Waiting);
                    inputTimer = 0f;
                    alreadyInput = false;
                    inputWait = false;
                }

                if (OVRInput.GetUp(vrKey))
                {
                    StateChange(SH_GimmickState.Waiting);
                    inputTimer = 0f;
                    alreadyInput = false;
                    inputWait = false;
                }
            }
            yield return null;
        }
        
    }

    #endregion

    #region Physhics
    /// <summary>
    /// 상호작용 가능한 상태면 true, 아니면 false
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    protected bool InteractibleCheck(Collider col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer(interactiveLayer.ToString()))
            return false;

        return true;
    }

    protected bool InteractibleCheck(Collision col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer(interactiveLayer.ToString()))
            return false;

        return true;
    }
    protected virtual void OnTriggerEnter(Collider col)
    {
        StateChange(SH_GimmickState.Hovering, col);
    }
    protected virtual void OnTriggerStay(Collider col)
    {
        StateChange(SH_GimmickState.Hovering, col);
    }
    protected virtual void OnTriggerExit(Collider col)
    {
        StateChange(SH_GimmickState.Waiting, col);
    }
    protected virtual void OnCollisionEnter(Collision col)
    {
        StateChange(SH_GimmickState.Hovering, col);
    }
    protected virtual void OnCollisionStay(Collision col)
    {
        StateChange(SH_GimmickState.Hovering, col);
    }
    protected virtual void OnCollisionExit(Collision col)
    {
        StateChange(SH_GimmickState.Waiting, col);
    }
    #endregion

    #region System

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

    void StateUpdate()
    {
        modelController.StateUpdate();
        soundController.StateUpdate();
        effectController.StateUpdate();
    }
    #endregion
}

