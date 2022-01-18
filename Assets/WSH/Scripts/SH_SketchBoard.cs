using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Photon.Pun;

// viewId 로 충돌물체 넘겨주기

public class SH_SketchBoard : MonoBehaviourPun
{
    public int i;
    [SerializeField]
    Texture paint_Complete;
    [SerializeField]
    Texture paint_Complete_NS;

    [SerializeField]
    LayerMask emptyBoardLayer;

    public bool onEasel;
    public Transform body;

    static List<SH_SketchBoard> failSketchs = new List<SH_SketchBoard>();
    enum PaintStatus
    {
        Empty,  //빈 상태
        Sketch, //밑그림
        Red,    //빨간 색칠, 그림자 없음
        Red_S,  //빨간 색칠, 그림자 있음
        Green,  
        Green_S,
        Blue,
        Blue_S,
        Complete,//완성, 그림자 없음
        Complete_S,//완성 그림자 있음
    }

    [SerializeField]
    PaintStatus current;

    Material paper;

    private void Awake()
    {
        current = PaintStatus.Empty;
        paper = GetComponent<MeshRenderer>().material;
        paper.SetTexture("_Texture", null);
        ColorMask(PaintColor.Red, false);
        ColorMask(PaintColor.Green, false);
        ColorMask(PaintColor.Blue, false);
    }

    void ColorMask(PaintColor color, bool value)
    {
        switch (color)
        {
            case PaintColor.Red:
                if (value)
                    paper.SetColor("_Primary", Color.white);
                else
                    paper.SetColor("_Primary", Color.red);
                break;

            case PaintColor.Green:
                if (value)
                    paper.SetColor("_Second", Color.white);
                else
                    paper.SetColor("_Second", Color.green);
                break;

            case PaintColor.Blue:
                if (value)
                    paper.SetColor("_Teritary", Color.white);
                else
                    paper.SetColor("_Teritary", Color.blue);
                break;
            default:
                break;
        }
    }
    public void OnBoard()
    {
        if (!onEasel)
            return;

        var board = FindObjectOfType<SH_BoardChanger>();
        body.transform.SetParent(board.transform.parent);
        body.transform.localPosition = board.gameObject.transform.localPosition;
        body.transform.localRotation = board.gameObject.transform.localRotation;

        photonView.RPC("Rpc_OnBoard", RpcTarget.Others);
    }

    [PunRPC]
    void Rpc_OnBoard()
    {
        if (!onEasel)
            return;

        var board = FindObjectOfType<SH_BoardChanger>();
        body.transform.SetParent(board.transform.parent);
        body.transform.localPosition = board.gameObject.transform.localPosition;
        body.transform.localRotation = board.gameObject.transform.localRotation;
    }

    //그림 순서
    //1. 흰색으로 밑그림
    //2. R
    //3. G
    //4. B
    //5. 검은색으로 그림자
    //순서가 잘못되면 메시지 뜨면서 보드 파기
    bool cantWrite;
    void WrongPaint(PaintColor color)
    {
        cantWrite = true;
        //body.GetComponent<Collider>().isTrigger = true;
        body.DOLocalMoveZ(-0.2f, 1f);// += delegate { body.GetComponent<Collider>().isTrigger = false; };
        paper.SetTexture("_Texture", paint_Complete_NS);
        ColorMask(PaintColor.Red, true);
        ColorMask(PaintColor.Green, true);
        ColorMask(PaintColor.Blue, true);
        switch (color)
        {
            case PaintColor.Red:
            case PaintColor.Green:
            case PaintColor.Blue:
                ColorMask(color, false);
                break;
            case PaintColor.Black:
                paper.SetTexture("_Texture", null);
                paper.SetFloat("_Alpha", 0f);
                break;
            case PaintColor.White:
                paper.SetTexture("_Texture", null);
                break;
        }

        failSketchs.Add(this);

        if(failSketchs.Count>10)
        {
            var f = failSketchs[0];
            failSketchs.RemoveAt(0);
            Destroy(f.body.gameObject);
        }
    }

    void PuzzleComplete()
    {
        body.GetComponent<Collider>().isTrigger = true;
        FindObjectOfType<SH_BoxParent>().On();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!onEasel)
            return;

        if (other.gameObject.tag == "Brush")
        {
            if (cantWrite)
                return;
            var head = other.GetComponent<SH_BrushHead>();
            var color = head.GetColor;

            if (color == PaintColor.None)
                return;

            switch (current)
            {
                case PaintStatus.Empty:
                    if (color != PaintColor.White)
                    {
                        WrongPaint(color);
                        return;
                    }
                    paper.SetTexture("_Texture", paint_Complete_NS);
                    ColorMask(PaintColor.Red, true);
                    ColorMask(PaintColor.Green, true);
                    ColorMask(PaintColor.Blue, true);
                    current = PaintStatus.Sketch;
                    break;

                case PaintStatus.Sketch:
                    if (color != PaintColor.Red)
                    {
                        if (color != PaintColor.White)
                            WrongPaint(color);
                        return;
                    }
                    current = PaintStatus.Red;
                    ColorMask(PaintColor.Red, false);
                    break;

                case PaintStatus.Red:
                    if (color != PaintColor.Green)
                    {
                        if (color != PaintColor.Red)
                            WrongPaint(color);
                        return;
                    }
                    current = PaintStatus.Green;
                    ColorMask(PaintColor.Green, false);
                    break;

                case PaintStatus.Green:
                    if (color != PaintColor.Blue)
                    {
                        if (color != PaintColor.Green)
                            WrongPaint(color);
                        return;
                    }
                    current = PaintStatus.Complete;
                    ColorMask(PaintColor.Blue, false);
                    break;

                case PaintStatus.Complete:
                    if (color != PaintColor.Black)
                    {
                        if (color != PaintColor.Blue)
                            WrongPaint(color);
                        return;
                    }
                    current = PaintStatus.Complete_S;
                    paper.SetTexture("_Texture", paint_Complete);
                    PuzzleComplete();
                    break;
            }
        }
    }
}
