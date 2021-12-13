using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KHJ_Pattern : MonoBehaviour
{
    public string answer1 = "135798642";
    public string answer2 = "137986542";
    public string Inputanswer;
    public GameObject[] nodes;
    public Image[] buttons;
    public List<GameObject> activeNodes = new List<GameObject>();
    LineRenderer drawer;
    public bool drawing;
    private void Awake()
    {
        drawer = GetComponent<LineRenderer>();
        drawer.positionCount = 0;
        drawing = false;
    }
    private void Update()
    {
        if (activeNodes.Count == nodes.Length)
        {
            if (PasswordCheck())
            {
                drawing = false;
                Clear();
            }
            else
            {
                Init();
            }
        }
    }
    bool PasswordCheck()
    {
        if (Inputanswer == answer1)
            return true;
        if (Inputanswer == answer2)
            return true;
        return false;
    }
    void Clear()
    {
        StartCoroutine(Clear_());
    }
    IEnumerator Clear_()
    {
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
        KHJ_SceneManager_1.instance.disappearWall();
    }
    public void NodeActive(int nodeNum)
    {
        GameObject node = nodes[nodeNum];
        drawing = true;
        if (activeNodes.Count == 0)
        {
            drawer.positionCount = 1;
            drawer.SetPosition(0, node.transform.position);
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
        drawer.SetPosition(activeNodes.Count, node.transform.position);
        activeNodes.Add(node);
    }
    
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
