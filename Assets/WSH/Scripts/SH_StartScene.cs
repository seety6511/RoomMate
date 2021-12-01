using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Cinemachine;

public enum SH_SceneName
{
    SH_Start,
    SH_Main,
}
public class SH_StartScene : MonoBehaviour
{
    public void GameStart()
    {
        StartCoroutine(Load(SH_SceneName.SH_Main.ToString()));
    }

    IEnumerator Load(string name)
    {
        var ao = SceneManager.LoadSceneAsync(name);
        while (!ao.isDone)
        {
            yield return null;
        }
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        EditorApplication.Exit(0);
#elif UNITY_STANDALONE
        Application.Quit();
#endif
    }
}
