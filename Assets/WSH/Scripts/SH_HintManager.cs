using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Text infoText;
    [SerializeField]
    float textFadeTime;

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
        infoText.text = hint.info[hint.infoIndex];

        infoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(textFadeTime);
        infoText.gameObject.SetActive(false);
        hintPanel.Add(hint);

        hint.infoIndex++;
        alreadyInfo = false;
    }
}
