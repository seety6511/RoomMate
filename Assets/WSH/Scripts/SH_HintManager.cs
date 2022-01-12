using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Febucci.UI;

/// <summary>
/// 1. ��Ʈ �ߵ�
/// 2. �ؽ�Ʈ ���
/// 3. �ߵ��� ��Ʈ ����
/// </summary>
public class SH_HintManager : MonoBehaviour
{
    [SerializeField]
    SH_Hint[] hints;
    [SerializeField]
    SH_HintPanel hintPanel;

    [SerializeField]
    TMP_Text infoText;
    [SerializeField]
    TMP_Text infoText_back;
    [SerializeField]
    float textFadeTime;
    public TextAnimatorPlayer textAnimatorPlayer;
    private void Awake()
    {
        hints = FindObjectsOfType<SH_Hint>();
        //textAnimatorPlayer = 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hints[0].Hint();
        }
        if (Input.GetKeyDown(KeyCode.Q))
            hintPanel.Switch();
    }

    [SerializeField]
    bool alreadyInfo;
    public void EnableHint(SH_Hint hint)
    {
        if (hint.infoIndex == hint.info.Count)
            return;

        StartCoroutine("InfoFade", hint);
    }

    IEnumerator InfoFade(SH_Hint hint)
    {
        if (alreadyInfo)
            yield break;

        alreadyInfo = true;
        infoText.text =hint.info[hint.infoIndex].ToString();
        //infoText.text = "<fade>" + hint.info[hint.infoIndex] + "</>";
        //infoText_back.text = "<fade><shake>" + hint.info[hint.infoIndex] + "</>";
        infoText_back.text = hint.info[hint.infoIndex].ToString();
        yield return new WaitForEndOfFrame();
        infoText.gameObject.SetActive(true);
        infoText_back.gameObject.SetActive(true);
        yield return new WaitForSeconds(textFadeTime);
        infoText.gameObject.SetActive(false);
        infoText_back.gameObject.SetActive(false);
        hintPanel.Add(hint);

        hint.infoIndex++;
        alreadyInfo = false;
    }
}
