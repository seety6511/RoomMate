using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public struct Line
{
    public GameObject lineObj;
    public List<Transform> BoxVectors;
}

public class Draw : MonoBehaviourPun
{
    //프리팹
    public GameObject Pencil_Line;
    public GameObject Bcollider;

    //리스트
    public List<Line> Lines;
    public Color PenColor;
    public bool isDraw = true;
    GameObject now;
    Vector3 pos;

    [HideInInspector]
    public Transform trRight;
    public Transform trRight_hand;
    public Transform trRight_body;
    public LineRenderer line;

    private void Start()
    {
        SetMarketWhite();
        LineSet();
    }
    private void OnEnable()
    {
        resetPos();
    }
    void Update()
    {
        Ray ray = new Ray(trRight.position, trRight.forward);
        RaycastHit hitInfo;
        int layer = 1 << LayerMask.NameToLayer("Board");
        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
        {
            line.SetPosition(0, trRight.position);
            line.SetPosition(1, hitInfo.point);
        }
        else
        {
            //그림판 밖으로 나가면
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            photonView.RPC("CreateLine", RpcTarget.Others);
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
            {
                //그리는 중
                if (isDraw)
                {
                    GameObject dot = Instantiate(Pencil_Line);
                    dot.transform.position = hitInfo.point + hitInfo.normal * 0.01f;
                    dot.transform.forward = -hitInfo.normal;
                    dot.transform.parent = hitInfo.transform;
                    dot.GetComponent<LineRenderer>().startColor = PenColor;
                    dot.GetComponent<LineRenderer>().endColor = PenColor;
                    dot.GetComponent<LineRenderer>().SetPosition(0, hitInfo.point + hitInfo.normal * 0.01f);

                    Line line = new Line();
                    line.lineObj = dot;
                    line.BoxVectors = new List<Transform>();
                    Lines.Add(line);

                    now = dot;
                }
                //지우는 중
                if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
                {
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Pencil"))
                    {
                        if (!isDraw)
                        {
                            Destroy(hitInfo.transform.gameObject);
                        }
                    }
                }
            }
        }
        if ((OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && NSR_AutoHandManager.instance.handPlayer)|| (NSR_AutoHandPlayer.instance != null && NSR_AutoHandPlayer.instance.receive_input_R[0] && NSR_AutoHandManager.instance.handPlayer == false))
        {
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
            {
                //그리는 중
                if (isDraw)
                {
                    if (now == null)
                    {
                        GameObject dot = Instantiate(Pencil_Line);
                        dot.transform.position = hitInfo.point + hitInfo.normal * 0.01f;
                        dot.transform.forward = -hitInfo.normal;
                        dot.transform.parent = hitInfo.transform;
                        dot.GetComponent<LineRenderer>().startColor = PenColor;
                        dot.GetComponent<LineRenderer>().endColor = PenColor;
                        dot.GetComponent<LineRenderer>().SetPosition(0, hitInfo.point + hitInfo.normal * 0.01f);

                        Line line = new Line();
                        line.lineObj = dot;
                        line.BoxVectors = new List<Transform>();
                        Lines.Add(line);

                        now = dot;
                    }
                    if (Vector3.Distance(pos, hitInfo.point) > 0.0001f)
                    {
                        Vector3 position = hitInfo.point + hitInfo.normal * 0.01f;

                        DrawLine(position);

                    }
                    pos = hitInfo.point;
                }

            }
            else
            {
                //그림판 밖으로 나가면
                LineEnd();
            }

            //지우는 중
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Pencil"))
                {
                    if (!isDraw)
                    {
                        Destroy(hitInfo.transform.parent.gameObject);
                    }

                }
            }
        }
    }
    public void LineSet()
    {
        if (NSR_AutoHandManager.instance.handPlayer)
        {
            trRight = trRight_hand;
        }
        else
        {
            trRight = trRight_body;
        }
    }

    [PunRPC]

    private void Line_SetPosition(Vector3 pos, Vector3 point)
    {
        line.SetPosition(0, pos);
        line.SetPosition(1, point);
    }

    [PunRPC]
    void CreateLine()
    {
        Ray ray = new Ray(trRight.position, trRight.forward);
        RaycastHit hitInfo;
        int layer = 1 << LayerMask.NameToLayer("Board");

        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
        {
            //그리는 중
            if (isDraw)
            {
                GameObject dot = Instantiate(Pencil_Line);
                dot.transform.position = hitInfo.point + hitInfo.normal * 0.01f;
                dot.transform.forward = -hitInfo.normal;
                dot.transform.parent = hitInfo.transform;
                dot.GetComponent<LineRenderer>().startColor = PenColor;
                dot.GetComponent<LineRenderer>().endColor = PenColor;
                dot.GetComponent<LineRenderer>().SetPosition(0, hitInfo.point + hitInfo.normal * 0.01f);

                Line line = new Line();
                line.lineObj = dot;
                line.BoxVectors = new List<Transform>();
                Lines.Add(line);

                now = dot;
            }
            //지우는 중
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Pencil"))
                {
                    if (!isDraw)
                    {
                        Destroy(hitInfo.transform.gameObject);
                    }
                }
            }
        }
    }

    [PunRPC]
    void DrawLine(Vector3 position)
    {
        //라인 그리기
        LineRenderer tmp = now.GetComponent<LineRenderer>();
        tmp.positionCount++;
        tmp.SetPosition(tmp.positionCount - 1, position);

        //박스 콜라이더 추가하기
        GameObject box_collider = Instantiate(Bcollider);
        box_collider.transform.position = tmp.GetPosition(tmp.positionCount - 1);
        box_collider.transform.forward = gameObject.transform.forward;
        box_collider.transform.parent = now.transform;

        Lines[Lines.Count - 1].BoxVectors.Add(box_collider.transform);
    }

    [PunRPC]
    void LineEnd()
    {
        now = null;
    }

    [PunRPC]
    void DestroyLine(GameObject obj)
    {
        Destroy(obj);
    }



    public void resetPos()
    {
        photonView.RPC("Rpc_resetPos", RpcTarget.Others);
        Rpc_resetPos();
        ////이동하고 나서 다시 그려주는 함수
        //foreach (Line a in Lines)
        //{
        //    if (a.lineObj != null)
        //    {
        //        LineRenderer line = a.lineObj.GetComponent<LineRenderer>();

        //        line.SetPosition(0, a.lineObj.transform.position);
        //        for (int i = 1; i < line.positionCount; i++)
        //        {
        //            line.SetPosition(i, a.BoxVectors[i - 1].position);
        //        }
        //    }
        //}
    }

    [PunRPC]
    void Rpc_resetPos()
    {
        //이동하고 나서 다시 그려주는 함수
        foreach (Line a in Lines)
        {
            if (a.lineObj != null)
            {
                LineRenderer line = a.lineObj.GetComponent<LineRenderer>();

                line.SetPosition(0, a.lineObj.transform.position);
                for (int i = 1; i < line.positionCount; i++)
                {
                    line.SetPosition(i, a.BoxVectors[i - 1].position);
                }
            }
        }
    }

    void SetMarkerColour(Color new_color)
    {
        PenColor = new_color;
    }
    public void SetMarkerRed()
    {
        photonView.RPC("Rpc_SetMarkerRed", RpcTarget.Others);

        Color c = Color.red;
        SetMarkerColour(c);
    }
    public void SetMarkerGreen()
    {
        photonView.RPC("Rpc_SetMarkerGreen", RpcTarget.Others);
        Color c = Color.green;
        SetMarkerColour(c);
    }
    public void SetMarkerBlue()
    {
        photonView.RPC("Rpc_SetMarkerBlue", RpcTarget.Others);
        Color c = Color.blue;
        SetMarkerColour(c);
    }
    public void SetMarketWhite()
    {
        photonView.RPC("Rpc_SetMarketWhite", RpcTarget.Others);
        Color c = Color.white;
        SetMarkerColour(c);
    }

    [PunRPC]
    void Rpc_SetMarkerColour(Color new_color)
    {
        PenColor = new_color;
    }
    [PunRPC]
    void Rpc_SetMarkerRed()
    {
        Color c = Color.red;
        SetMarkerColour(c);
    }
    [PunRPC]
    void Rpc_SetMarkerGreen()
    {
        Color c = Color.green;
        SetMarkerColour(c);
    }
    [PunRPC]
    void Rpc_SetMarkerBlue()
    {
        Color c = Color.blue;
        SetMarkerColour(c);
    }
    [PunRPC]
    void Rpc_SetMarketWhite()
    {
        Color c = Color.white;
        SetMarkerColour(c);
    }

    public void EaraseAll()
    {
        Rpc_EaraseAll();
        photonView.RPC("Rpc_EaraseAll", RpcTarget.Others);

    }

    [PunRPC]
    void Rpc_EaraseAll()
    {
        foreach (Line lines in Lines)
        {
            Destroy(lines.lineObj);
        }
        Lines.Clear();
    }

    public void isDrawing()
    {
        isDraw = true;
        photonView.RPC("Rpc_isDrawing", RpcTarget.Others);
    }

    [PunRPC]
    void Rpc_isDrawing()
    {
        isDraw = true;
    }
    public void isErasing()
    {
        photonView.RPC("Rpc_isErasing", RpcTarget.Others);
        isDraw = false;
    }

    [PunRPC]
    void Rpc_isErasing()
    {
        isDraw = false;
    }
}


