using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class KHJ_Menu : MonoBehaviourPun
{
    public GameObject Player;
    public GameObject MenuUI;
    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) || OVRInput.GetDown(OVRInput.Button.Four, OVRInput.Controller.LTouch))
        {
            SetMenuUI();
            photonView.RPC("SetMenuUI", RpcTarget.Others);
            //print("Menu");
            //MenuUI.SetActive(!MenuUI.activeSelf);
            //MenuUI.transform.position = Player.transform.position;
            //MenuUI.transform.rotation = Player.transform.rotation;
        }
    }


    [PunRPC]
    void SetMenuUI()
    {
        print("Menu");
        MenuUI.SetActive(!MenuUI.activeSelf);
        MenuUI.transform.position = Player.transform.position;
        MenuUI.transform.rotation = Player.transform.rotation;
    }

    public void GoToScene(string name)
    {
        Rpc_GoToScene(name);
        photonView.RPC("Rpc_GoToScene", RpcTarget.Others, name);
        //if (name == "Quit")
        //{
        //    Application.Quit();
        //    return;
        //}
        //StartCoroutine(LoadScene(name));
    }

    [PunRPC]
    void Rpc_GoToScene(string name)
    {
        if (name == "Quit")
        {
            Application.Quit();
            return;
        }
        StartCoroutine(LoadScene(name));
    }

    IEnumerator LoadScene(string name)
    {
        var ao = SceneManager.LoadSceneAsync(name);
        while (!ao.isDone)
        {
            yield return null;
        }
    }
}
