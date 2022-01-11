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
        if ((PhotonNetwork.IsConnected && NSR_AutoHandManager.instance.handPlayer == false)) return;

        if (activeNodes.Count == nodes.Length)
        {
            if (PasswordCheck())
            {
                drawing = false;
                if (PhotonNetwork.IsConnected)
                    photonView.RPC("Clear", RpcTarget.Others);
                Clear();
            }
            else
            {
                StartCoroutine(Initialize());
            }
        }
        if (!KHJ_SmartPhone.instance.IsTouching)
        {
            Init();
        }
        else
        {
            if (activeNodes.Count != 0)
            {
                charArr = Inputanswer.ToCharArray();
                Vector3[] p = new Vector3[charArr.Length + 1];
                for (int i = 0; i < activeNodes.Count; i++)
                {
                    p[i] = nodes[int.Parse(charArr[i].ToString()) - 1];
                    drawer.SetPosition(i, nodes[int.Parse(charArr[i].ToString()) - 1]);

                }
                
                Vector3 WorldToLocal = KHJ_SmartPhone.instance.tmp.transform.localPosition;
                WorldToLocal.z = -0.0001f;
                //if (PhotonNetwork.IsConnected)
                //{
                //    photonView.RPC("DrawLine", RpcTarget.Others, activeNodes.Count, WorldToLocal);
                //}
                drawer.SetPosition(activeNodes.Count, WorldToLocal);
                p[activeNodes.Count] = WorldToLocal;

                photonView.RPC("Test", RpcTarget.Others, p);
            }
        }
    }

    [PunRPC]
    void Test(Vector3 [] points)
    {
        //    print(points.Length);
        drawer.positionCount = points.Length;
        for(int i = 0; i < points.Length; i++)
        {
            print(points[i]);
            drawer.SetPosition(i, points[i]);
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
    IEnumerator Clear_()
    {
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
    }
    [PunRPC]
    void SetPositionCount(int n)
    {
        Init();
    }

    [PunRPC]
    void PositionCountPlus()
    {
        drawer.positionCount++;
    }
    public void NodeActive(int nodeNum)
    {
        Vector3 node = nodes[nodeNum];
        drawing = true;
        if (activeNodes.Count == 0)
        {
            //if (PhotonNetwork.IsConnected)
            //{
            //    photonView.RPC("SetPositionCount", RpcTarget.Others, 2);
            //}
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
        //if (PhotonNetwork.IsConnected)
        //{
        //    photonView.RPC("PositionCountPlus", RpcTarget.Others);
        //}
        drawer.positionCount++;
        activeNodes.Add(node);
    }
    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(0.4f);
        Init();
    }
    public void Init()
    {
        if (drawer.positionCount == 0) return;

        foreach (Image image in buttons)
        {
            image.color = new Color32(0, 0, 0, 255);
        }
        drawing = false;
        activeNodes.Clear();
        Inputanswer = "";
       
        drawer.positionCount = 0;


        if (PhotonNetwork.IsConnected && NSR_AutoHandManager.instance.handPlayer)
        {
            photonView.RPC("SetPositionCount", RpcTarget.Others, 0);
        }

    }
}
