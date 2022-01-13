using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_Panel_HintHistory : MonoBehaviour
{
    [SerializeField]
    Image portrait;
    [SerializeField]
    Text hintHistory;
    [SerializeField]
    Text hintNum;
    public SH_Hint hint;
    public void SetHint(SH_Hint hint, int num)
    {
        this.hint = hint;
        portrait.sprite = hint.portrait;
        hintHistory.text = hint.info[hint.infoIndex];
        hintNum.text += num;
    }

    public void InfoUpdate(SH_Hint hint)
    {
        Debug.Log(hint.infoIndex);
        hintHistory.text += "\n"+hint.info[hint.infoIndex];
    }
}
