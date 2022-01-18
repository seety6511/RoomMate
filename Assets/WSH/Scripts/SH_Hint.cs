using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���ϴ� ������Ʈ�� ���δ�.
/// ���ϴ� ������ �ν������� info�� ����ִ´�.
/// ���̾��ũ�� �����Ѵ�.
/// ������ �ִ� ������Ʈ�� �����ϴ�.
/// ��. 
/// </summary>
public class SH_Hint : MonoBehaviour
{
    protected SH_HintManager hintManager;
    public Sprite portrait;
    [SerializeField]
    LayerMask interactorLayer;
    public List<string> info = new List<string>();
    public int infoIndex;
    public bool hasOn; //�ѹ��̶� �ߵ��Ǿ���?
    void Start()
    {
        hintManager = FindObjectOfType<SH_HintManager>();

        var col = GetComponent<Collider>();
        if (col == null)
        {
            gameObject.AddComponent<MeshCollider>().convex = true;
        }
        infoIndex = 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (hasOn)
            return;
        if ((1 << collision.gameObject.layer) != interactorLayer)
            return;
        if (!hintManager.alreadyInfo)
            Hint();
    }

    public void Hint()
    {
        print("Hint");
        hasOn = true;
        hintManager.EnableHint(this);
    }

    ///���� infoText�� ������ ���� �ʹٸ�
    ///�Ʒ��� ���� ����Ѵ�.
    ///1. info List�� ������� �ؽ�Ʈ�� ����ִ´�.
    ///2. ���ʷ� �ؽ�Ʈ�� �������.
    ///3. �ؽ�Ʈ�� ������
    ///4. ResetInfo() �� ȣ���Ѵ�.
    ///5. �ٽ� ������ ������ �ؽ�Ʈ�� ����.
    public void ResetInfo()
    {
        hasOn = false;
    }
}
