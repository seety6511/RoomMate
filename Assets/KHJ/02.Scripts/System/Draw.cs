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
    //������
    public GameObject Pencil_Line;
    public GameObject Bcollider;

    //����Ʈ
    public List<Line> Lines;
    public Color PenColor;
    public bool isDraw = true;
    GameObject now;
    Vector3 pos;

    public Transform trRight;
    public LineRenderer line;

    private void Start()
    {
        SetMarketWhite();
    }
    private void OnEnable()
    {
        resetPos();
    }
    void Update()
    {
        if (NSR_AutoHandManager.instance.handPlayer == false) return;

        Ray ray = new Ray(trRight.position, trRight.forward);
        RaycastHit hitInfo;
        int layer = 1 << LayerMask.NameToLayer("Board");
        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
        {
            photonView.RPC("Line_SetPosition", RpcTarget.Others, trRight.position, hitInfo.point);
            line.SetPosition(0, trRight.position);
            line.SetPosition(1, hitInfo.point);
        }
        else
        {
            //�׸��� ������ ������
            photonView.RPC("Line_SetPosition", RpcTarget.Others, Vector3.zero, Vector3.zero);
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
            {
                //�׸��� ��
                if (isDraw)
                {
                    Vector3 position = hitInfo.point + hitInfo.normal * 0.01f;
                    Vector3 forward = -hitInfo.normal;
                    Transform parent = hitInfo.transform;

                    CreateLine(position, forward, parent);
                    photonView.RPC("CreateLine", RpcTarget.Others, position, forward, parent);
                }
                //����� ��
                if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
                {
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Pencil"))
                    {
                        if (!isDraw)
                        {
                            Destroy(hitInfo.transform.gameObject);
                            photonView.RPC("DestroyLine", RpcTarget.Others, hitInfo.transform.gameObject);
                        }
                    }
                }
            }
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
            {
                //�׸��� ��
                if (isDraw)
                {

                    if (now == null)
                    {
                        Vector3 position = hitInfo.point + hitInfo.normal * 0.01f;
                        Vector3 forward = -hitInfo.normal;
                        Transform parent = hitInfo.transform;

                        CreateLine(position, forward, parent);
                        photonView.RPC("CreateLine", RpcTarget.Others, position, forward, parent);
                    }
                    if (Vector3.Distance(pos, hitInfo.point) > 0.0001f)
                    {
                        Vector3 position = hitInfo.point + hitInfo.normal * 0.01f;

                        DrawLine(position);
                        photonView.RPC("DrawLine", RpcTarget.Others, position);

                    }
                    pos = hitInfo.point;
                }

            }
            else
            {
                //�׸��� ������ ������
                LineEnd();
                photonView.RPC("LineEnd", RpcTarget.Others);
            }

            //����� ��
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Pencil"))
                {
                    if (!isDraw)
                    {
                        Destroy(hitInfo.transform.parent.gameObject);
                        photonView.RPC("DestroyLine", RpcTarget.Others, hitInfo.transform.parent.gameObject);
                    }

                }
            }
        }
    }

    [PunRPC]

    private void Line_SetPosition(Vector3 pos, Vector3 point)
    {
        line.SetPosition(0, pos);
        line.SetPosition(1, point);
    }

    [PunRPC]
    void CreateLine(Vector3 position, Vector3 forward, Transform parent)
    {
        GameObject dot = Instantiate(Pencil_Line);
        dot.transform.position = position;
        dot.transform.forward = forward;
        dot.transform.parent = parent;
        dot.GetComponent<LineRenderer>().startColor = PenColor;
        dot.GetComponent<LineRenderer>().endColor = PenColor;
        dot.GetComponent<LineRenderer>().SetPosition(0, position);

        Line line = new Line();
        line.lineObj = dot;
        line.BoxVectors = new List<Transform>();
        Lines.Add(line);

        now = dot;
    }

    [PunRPC]
    void DrawLine(Vector3 position)
    {
        //���� �׸���
        LineRenderer tmp = now.GetComponent<LineRenderer>();
        tmp.positionCount++;
        tmp.SetPosition(tmp.positionCount - 1, position);

        //�ڽ� �ݶ��̴� �߰��ϱ�
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
        Rpc_resetPos();
        photonView.RPC("Rpc_resetPos", RpcTarget.Others);
        ////�̵��ϰ� ���� �ٽ� �׷��ִ� �Լ�
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
        //�̵��ϰ� ���� �ٽ� �׷��ִ� �Լ�
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

    public void SetMarkerColour(Color new_color)
    {
        photonView.RPC("Rpc_SetMarkerColour", RpcTarget.Others, new_color);
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
        //foreach (Line lines in Lines)
        //{
        //    Destroy(lines.lineObj);
        //}
        //Lines.Clear();
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
        photonView.RPC("Rpc_isDrawing", RpcTarget.Others);
        isDraw = true;
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


