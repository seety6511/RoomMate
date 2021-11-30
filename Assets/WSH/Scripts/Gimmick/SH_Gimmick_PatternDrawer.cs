using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class SH_Gimmick_PatternDrawer : SH_Gimmick
{
    public SH_Gimmick_PatternNode[] nodes;
    public List<SH_Gimmick_PatternNode> activeNodes = new List<SH_Gimmick_PatternNode>();
    LineRenderer drawer;
    public bool drawing;
    SH_TestHand hand;

    protected override void Awake()
    {
        base.Awake();
        //nodes = GetComponentsInChildren<SH_Gimmick_PatternNode>();
        drawer = GetComponent<LineRenderer>();
        hand = FindObjectOfType<SH_TestHand>();
        drawer.positionCount = 2;
        drawing = false;
    }

    protected override void Update()
    {
        base.Update();
        if (drawing)
        {
            var pos = hand.transform.position;
            pos.z = activeNodes[0].transform.position.z;
            drawer.SetPosition(activeNodes.Count, pos);
        }

        if (activeNodes.Count == nodes.Length)
        {
            if (PasswordCheck())
            {
                Clear();
            }
        }
    }

    protected override void Clear()
    {
        gameObject.SetActive(false);
        base.Clear();
    }
    bool PasswordCheck()
    {
        for(int i = 0; i < activeNodes.Count; ++i)
        {
            var n1 = activeNodes[i];
            var n2 = nodes[i];
            if (n1 != n2)
                return false;
        }
        return true;
    }

    public void NodeActive(SH_Gimmick_PatternNode node)
    {
        drawing = true;
        if (activeNodes.Count == 0)
            drawer.positionCount = 1;

        if (activeNodes.Contains(node))
            return;

        drawer.SetPosition(activeNodes.Count, node.transform.position);
        drawer.positionCount++;
        activeNodes.Add(node);
    }

    protected override void Waiting()
    {
        base.Waiting();
        if (drawing)
        {
            drawing = false;
            activeNodes.Clear();
            drawer.positionCount = 0;
        }
    }

    protected override void Hovering()
    {
        base.Hovering();
        
    }

}
