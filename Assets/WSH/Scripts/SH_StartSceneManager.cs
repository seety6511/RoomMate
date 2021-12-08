using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
public enum SH_SceneName
{
    SH_Start,
    SH_Main,
    NSR_ConnetScene,
}
public class SH_StartSceneManager : MonoBehaviour
{
    public GameObject doorPrefab;
    public float doorInterval;

    public void LoadSave()
    {

    }
    public void GameStart()
    {
        Debug.Log("GameStart");
        StartCoroutine(Load(SH_SceneName.NSR_ConnetScene.ToString()));
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
