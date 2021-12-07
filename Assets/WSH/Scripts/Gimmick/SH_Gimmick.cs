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
    Waiting,    //���Ӽ�
    Hovering,   //���Ӽ�
    Active,     //�ܹ߼�
    Activating, //���Ӽ�
    Clear,      //�ܹ߼�
    Disable,    //�ܹ߼�. �� ���¸� �ƹ� ��ȣ�ۿ��� ���� �ʴ´�.
    Reload,     //���Ӽ�
    ActiveKeep,       //Active���� �ٷ� �Ѿ�� ����.
    ActivatingKeep,   //Activating ���� �ٷ� �Ѿ�� ����.
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
    public SH_Layer interactiveLayer;  //�� ���̾��� �ݶ��̴��θ� ���� �����ϴ�. �⺻�� : LayerMask.NameToLayer("Interacter")
    public SH_GimmickState gimmickState;
    public SH_GimmickState prevState;
    public List<SH_Gimmick> password = new List<SH_Gimmick>();    //�� ����Ʈ�� ��� ����� clear ���¿��� �� ����� ���۰����ϴ�.
    public bool isActive;           //���� Ȱ��ȭ �����ΰ�?
    public bool keepState;          //Ȱ��ȭ ���¸� ���� �Ұ��ΰ�?
    public float activeCoolTime;    //�ѹ� �۵���Ų�� �ٽ� �۵���Ű�� ���� �ʿ��� �ð�
    public float reloadTime;        //������ �ߴܵǾ�����, �����·� ���ư������ �ʿ��� �ð�
    protected float reloadTimer;

    public OVRInput.Button vrKey;   //�� ��ư���� �۵��Ѵ�.(vr)
    public KeyCode pcKey;           //�� ��ư���� �۵��Ѵ�(pc)

    SH_Gimmick_SoundController soundController;
    SH_Gimmick_EffectController effectController;
    SH_Gimmick_ModelStateMachine modelController;

    #region Protected Interfaces
    //����� �⺻ �ʱ�ȭ
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
    /// ���� ����� ���¸� �����Ҷ� ����Ѵ�. �ݵ�� �̰��� ���ؼ� ���¸� ������Ѿ� �Ѵ�.<br/>
    /// ���¸� ������ ��ȯ��ų��� force == true
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

            case SH_GimmickState.Disable:   //��Ȱ��ȭ ������ ��� ���������� �Ұ����ϴ�. ������ GimmickEnable()�� Ȱ��ȭ����
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
    /// ��ȣ�ۿ� ������ ���¸� true, �ƴϸ� false
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

