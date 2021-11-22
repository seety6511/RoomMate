using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Line
{
    public GameObject lineObj;
    public List<Transform> BoxVectors;
}

public class Draw : MonoBehaviour
{
    //������
    public GameObject Pencil_Line;
    public GameObject Bcollider;

    //����Ʈ
    public List<Line> Lines;
    public Color PenColor;
    bool isDraw = true;
    GameObject now;
    Vector3 pos;

    private void Start()
    {
        SetMarketWhite();
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            int layer = 1 << LayerMask.NameToLayer("Board");
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
            {
                //�׸��� ��
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
                //����� ��
                if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
                {
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Pencil"))
                    {
                        if(!isDraw)
                            Destroy(hitInfo.transform.gameObject);
                    }
                }
            }
        }
        if (Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            int layer = 1 << LayerMask.NameToLayer("Board");
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
            {
                //�׸��� ��
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
                    if (Vector3.Distance(pos, hitInfo.point) > 0.001f)
                    {
                        //���� �׸���
                        LineRenderer tmp = now.GetComponent<LineRenderer>();
                        tmp.positionCount++;
                        tmp.SetPosition(tmp.positionCount-1, hitInfo.point + hitInfo.normal * 0.01f);

                        //�ڽ� �ݶ��̴� �߰��ϱ�
                        GameObject box_collider = Instantiate(Bcollider);
                        box_collider.transform.position = tmp.GetPosition(tmp.positionCount - 1);
                        box_collider.transform.forward = gameObject.transform.forward;
                        box_collider.transform.parent = now.transform;

                        Lines[Lines.Count-1].BoxVectors.Add(box_collider.transform);
                    }                    
                    pos = hitInfo.point;
                }
            }
            else
            {
                //�׸��� ������ ������
                now = null;
            }

            //����� ��
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Pencil"))
                {
                    if (!isDraw)
                        Destroy(hitInfo.transform.parent.gameObject);
                }
            }
        }
    }

    public void resetPos()
    {
        //�̵��ϰ� ���� �ٽ� �׷��ִ� �Լ�
        foreach(Line a in Lines)
        {
            if(a.lineObj != null)
            {
                LineRenderer line = a.lineObj.GetComponent<LineRenderer>();

                line.SetPosition(0, a.lineObj.transform.position);
                for (int i = 1; i < line.positionCount; i++)
                {
                    line.SetPosition(i, a.BoxVectors[i-1].position);
                }
            }
        }
    }

    public void SetMarkerColour(Color new_color)
    {
        PenColor = new_color;
    }
    public void SetMarkerRed()
    {
        Color c = Color.red;
        SetMarkerColour(c);
    }
    public void SetMarkerGreen()
    {
        Color c = Color.green;
        SetMarkerColour(c);
    }
    public void SetMarkerBlue()
    {
        Color c = Color.blue;
        SetMarkerColour(c);
    }
    public void SetMarketWhite()
    {
        Color c = Color.white;
        SetMarkerColour(c);
    }

    public void EaraseAll()
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
    }
    public void isErasing()
    {
        isDraw = false;
    }

}
