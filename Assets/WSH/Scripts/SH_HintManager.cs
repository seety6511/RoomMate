using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Febucci.UI;

/// <summary>
/// 1. 힌트 발동
/// 2. 텍스트 출력
/// 3. 발동된 힌트 저장
/// </summary>
public class SH_HintManager : MonoBehaviour
{
    [SerializeField]
    SH_Hint[] hints;
    [SerializeField]
    SH_HintPanel hintPanel;

    [SerializeField]
    TextAnimatorPlayer infoText;
    [SerializeField]
    TextAnimatorPlayer infoText_back;
    [SerializeField]
    float textFadeTime;
    [SerializeField]
    float textShowTime;
    private void Awake()
    {
        hints = FindObjectsOfType<SH_Hint>();
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
    public bool alreadyInfo;
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
        infoText.gameObject.SetActive(true);
        infoText_back.gameObject.SetActive(true);
        infoText.ShowText(hint.info[hint.infoIndex]);
        infoText_back.ShowText(hint.info[hint.infoIndex]);
        yield return new WaitForSeconds(textShowTime);
        infoText.StartDisappearingText();
        infoText_back.StartDisappearingText();
        yield return new WaitForSeconds(textFadeTime);
        infoText.gameObject.SetActive(false);
        infoText_back.gameObject.SetActive(false);
        hintPanel.Add(hint);

        hint.infoIndex++;
        alreadyInfo = false;
    }
}
