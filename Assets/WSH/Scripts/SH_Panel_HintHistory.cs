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
    public SH_Hint hint;
    public void SetHint(SH_Hint hint)
    {
        this.hint = hint;
        portrait.sprite = hint.portrait;
        hintHistory.text = hint.info[hint.infoIndex];
    }

    public void InfoUpdate(SH_Hint hint)
    {
        Debug.Log(hint.infoIndex);
        hintHistory.text += "\n"+hint.info[hint.infoIndex];
    }
}
