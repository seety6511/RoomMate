using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//하이라이트 하고 싶은 개체에 붙입시다.
//Renderer, SkinedMeshRenderer 둘중 하나는 붙어있어야 한다.
//always == true 일 경우 항상 강조된 상태가 된다.
public class SH_HightLight : MonoBehaviour
{
    GameObject copy;
    public Color color;
    public bool always;
    public float size;
    new Renderer renderer;
    SkinnedMeshRenderer skinRender;
    private void Awake()
    {
        var temp = transform.parent.GetComponent<SH_HightLight>();
        if (temp != null)
        {
            if (temp.copy != null)
            {
                Destroy(this);
                return;
            }
        }

        copy = Instantiate(gameObject, transform);
        renderer = copy.GetComponent<Renderer>();
        skinRender = copy.GetComponent<SkinnedMeshRenderer>();

        if (always)
            HightLight();
        else
            copy.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    public void HightLight()
    {
        if (renderer != null)
        {
            copy.SetActive(true);
            var mat = renderer.material;
            mat.SetFloat("_Outline", size);
            mat.SetColor("_OutlineColor", color);
            return;
        }

        if (skinRender != null)
        {
            copy.SetActive(true);
            var mat = skinRender.material;
            mat.SetFloat("_Outline", size);
            mat.SetColor("_OutlineColor", color);
        }
    }
}
