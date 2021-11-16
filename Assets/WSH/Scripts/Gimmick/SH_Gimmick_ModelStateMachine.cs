using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Gimmick_ModelStateMachine : MonoBehaviour
{
    SH_Gimmick parent;

    SH_GimmickTag currentState;
    public SH_GimmickTag_Active activeState;
    public SH_GimmickTag_Activating activatingState;
    public SH_GimmickTag_Waiting waitingState;
    public SH_GimmickTag_Disable disableState;
    public SH_GimmickTag_Clear clearState;
    public SH_GimmickTag_Hovering hoveringState;
    public SH_GimmickTag_Reload reloadState;

    public void Init()
    {
        parent = GetComponent<SH_Gimmick>();

        clearState = parent.gameObject.GetComponentInChildren<SH_GimmickTag_Clear>();
        activeState = parent.gameObject.GetComponentInChildren<SH_GimmickTag_Active>();
        reloadState = parent.gameObject.GetComponentInChildren<SH_GimmickTag_Reload>();
        waitingState = parent.gameObject.GetComponentInChildren<SH_GimmickTag_Waiting>();
        disableState = parent.gameObject.GetComponentInChildren<SH_GimmickTag_Disable>();
        hoveringState = parent.gameObject.GetComponentInChildren<SH_GimmickTag_Hovering>();
        activatingState = parent.gameObject.GetComponentInChildren<SH_GimmickTag_Activating>();

        if (clearState != null)
            clearState.gameObject.SetActive(false);

        if (activeState != null)
            activeState.gameObject.SetActive(false);

        if (waitingState != null)
            waitingState.gameObject.SetActive(false);

        if (disableState != null)
            disableState.gameObject.SetActive(false);
       
        if (hoveringState != null)
            hoveringState.gameObject.SetActive(false);

        if (activatingState != null)
            activatingState.gameObject.SetActive(false);

        if (reloadState != null)
            reloadState.gameObject.SetActive(false);

        Change(waitingState);
    }

    void Change(SH_GimmickTag change)
    {
        if (currentState != null)
            currentState.gameObject.SetActive(false);

        if (change == null)
            return;

        currentState = change;
        change.gameObject.SetActive(true);
    }

    public void StateUpdate()
    {
        switch (parent.gimmickState)
        {
            case SH_GimmickState.Active:
                Change(activeState);
                break;

            case SH_GimmickState.Activating:
                Change(activatingState);
                break;

            case SH_GimmickState.Waiting:
                Change(waitingState);
                break;

            case SH_GimmickState.Disable:
                Change(disableState);
                break;

            case SH_GimmickState.Clear:
                Change(clearState);
                break;

            case SH_GimmickState.Hovering:
                Change(hoveringState);
                break;

            case SH_GimmickState.Reload:
                Change(reloadState);
                break;

            case SH_GimmickState.None:
                break;

            default:
                break;
        }
    }
}
