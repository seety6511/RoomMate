using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// �ѹ��̶� ��µǾ����� ��Ʈ���� ������ UI
/// </summary>
public class SH_HintPanel : MonoBehaviour
{
    [SerializeField]
    List<SH_Hint> hintList = new List<SH_Hint>();
    [SerializeField]
    SH_Panel_HintHistory hintHistoryPrefab;
    [SerializeField]
    Transform hintHistory;

    List<SH_Panel_HintHistory> hintHistoryList = new List<SH_Panel_HintHistory>();
    public void Add(SH_Hint hint)
    {
        for(int i = 0; i < hintHistoryList.Count; ++i)
        {
            if(hintHistoryList[i].hint == hint)
            {
                hintHistoryList[i].InfoUpdate(hint);
                return;
            }
        }

        hintList.Add(hint);

        var his = Instantiate(hintHistoryPrefab, hintHistory);
        his.SetHint(hint);
        hintHistoryList.Add(his);
    }

    public void Switch()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
