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

    //�������� �߻��ϴ� �̺�Ʈ
    protected virtual IEnumerator ReloadEvent()
    {
        StateChange(SH_GimmickState.Waiting);
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
    /// ��ȣ�ۿ� ������ ���¸� true, �ƴϸ� false
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