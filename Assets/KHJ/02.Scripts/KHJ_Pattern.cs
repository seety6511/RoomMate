using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class KHJ_Pattern : MonoBehaviourPun
{
    public string answer1 = "135798642";
    public string answer2 = "137986542";
    public string Inputanswer;
    public Vector3[] nodes;
    public Image[] buttons;
    public List<Vector3> activeNodes = new List<Vector3>();
    LineRenderer drawer;
    public bool drawing;
    private void Awake()
    {
        drawer = GetComponent<LineRenderer>();
        drawer.positionCount = 0;
        drawing = false;
    }
    public char[] charArr;
    void Update()
    {
        if (!NSR_AutoHandManager.instance.handPlayer) return;

        if (activeNodes.Count == nodes.Length)
        {
            if (PasswordCheck())
            {
                drawing = false;
                photonView.RPC("Clear", RpcTarget.All);
                //Clear();
            }
            else
            {
                StartCoroutine(Initialize());
            }
        }
        if (!KHJ_SmartPhone.instance.IsTouching)
        {
            photonView.RPC("Init", RpcTarget.All);
            //Init();
        }
        else
        {
            if (activeNodes.Count != 0)
            {
                charArr = Inputanswer.ToCharArray();
              
                for (int i = 0; i < activeNodes.Count; i++)
                {
                    //실시간으로 라인 그려주기
                    photonView.RPC("DrawLine", RpcTarget.All, i, nodes[int.Parse(charArr[i].ToString()) - 1]);
                }
                Vector3 WorldToLocal = KHJ_SmartPhone.instance.tmp.transform.localPosition;
                WorldToLocal.z = -0.0001f;
                photonView.RPC("DrawLine", RpcTarget.All, activeNodes.Count, WorldToLocal);
            }
        }
    }

    [PunRPC]
    void DrawLine(int i, Vector3 pos)
    {
        drawer.SetPosition(i, pos);
    }

    
    bool PasswordCheck()
    {
        if (Inputanswer == answer1)
            return true;
        if (Inputanswer == answer2)
            return true;
        return false;
    }

    [PunRPC]
    void Clear()
    {
        StartCoroutine(Clear_());
    }
    public GameObject FootPuzzle__1;
    IEnumerator Clear_()
    {
        FootPuzzle__1.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
    }
    public void NodeActive(int nodeNum)
    {
        if (!KHJ_SmartPhone.instance.IsAlarmEnd)
            return;
        Vector3 node = nodes[nodeNum];
        drawing = true;
        if (activeNodes.Count == 0)
        {
            drawer.positionCount = 2;
            Inputanswer += (nodeNum + 1);
            activeNodes.Add(node);
            buttons[nodeNum].color = new Color32(0, 0, 0, 0);
            return;
        }

        if (activeNodes.Contains(node))
            return;
        Inputanswer += (nodeNum + 1);
        buttons[nodeNum].color = new Color32(0, 0, 0, 0);
        drawer.positionCount++;
        activeNodes.Add(node);
    }
    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(0.4f);
        photonView.RPC("Init", RpcTarget.All);

        //Init();
    }

    [PunRPC]
    public void Init()
    {
        foreach(Image image in buttons)
        {
            image.color = new Color32(0, 0, 0, 255);
        }
        drawing = false;
        activeNodes.Clear();
        Inputanswer = "";
        drawer.positionCount = 0;
    }
}
