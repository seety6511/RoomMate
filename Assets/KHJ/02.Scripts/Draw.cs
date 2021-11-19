using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
    public GameObject lineObj;
    public List<Vector3> vectors;
}


public class Draw : MonoBehaviour
{
    //프리팹
    public GameObject Pencil_Trail;
    public GameObject Pencil_Line;

    //라인들을 담아 둘 구조체 리스트
    public List<Line> Lines;

    //
    public List<GameObject> Pencils;
    public List<Vector3> Pencilvectors;
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
                    Pencils.Add(dot);
                    now = dot;
                }
                //지우는 중
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
                        Pencils.Add(dot);
                        now = dot;
                    }
                    if (Vector3.Distance(pos, hitInfo.point) > 0.001f)
                    {
                        LineRenderer tmp = now.GetComponent<LineRenderer>();
                        tmp.positionCount++;
                        tmp.SetPosition(tmp.positionCount-1, hitInfo.point + hitInfo.normal * 0.01f);
                        Vector3 pos = tmp.GetPosition(tmp.positionCount - 1) - tmp.GetPosition(0);
                        BoxCollider box = now.AddComponent<BoxCollider>();
                        box.center = pos;
                        box.size = new Vector3(0.01f, 0.01f, 0.001f);
                        Pencilvectors.Add(box.center);

                    }                    
                    pos = hitInfo.point;
                }
            }
            else
            {
                //그림판 밖으로 나가면
                now = null;
            }

            //지우는 중
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Pencil"))
                {
                    if (!isDraw)
                        Destroy(hitInfo.transform.gameObject);
                }
            }
        }
    }

    public void resetPos()
    {
        //이동하고 나서 다시 그려주는 함수
        foreach(GameObject a in Pencils)
        {
            LineRenderer line = a.GetComponent<LineRenderer>();

            //line.SetPosition(0, tmp);
            for (int i = 1; i < line.positionCount; i++)
            {
                Vector3 tmp = transform.position;
                tmp.x += Pencilvectors[i].x + a.transform.position.x;
                tmp.y += Pencilvectors[i].y + a.transform.position.y;
                tmp.z += Pencilvectors[i].z + a.transform.position.z;


                line.SetPosition(i, tmp);
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
        foreach (GameObject lines in Pencils)
        {
            Destroy(lines);
        }
        Pencils.Clear();
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
