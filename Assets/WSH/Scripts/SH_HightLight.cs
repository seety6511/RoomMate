using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//하이라이트 하고 싶은 개체에 붙입시다.
//Renderer 없으면 에러남.
public class SH_HightLight : MonoBehaviour
{
    GameObject copy;
    public Color color;
    public bool on;
    public float size;
    new Renderer renderer;
    private void Start()
    {
        var temp = transform.parent.GetComponent<SH_HightLight>();
        if (temp != null)
        {
            if (temp.copy != null)
            {
                Destroy(GetComponent<SH_HightLight>());
                return;
            }
        }

        copy = Instantiate(gameObject, transform);
        renderer = copy.GetComponent<Renderer>();
        renderer.material.shader = Shader.Find("Custom/Outline");
        if (on)
            HightLight();
        else
            copy.SetActive(false);
    }

    private void Update()
    {
        if (on)
            HightLight();
        else
            Off();
    }

    void Off()
    {
        copy.SetActive(false);
    }

    void HightLight()
    {
        if (renderer != null)
        {
            copy.SetActive(true);
            var mat = renderer.material;
            mat.SetFloat("_Outline", size);
            mat.SetColor("_OutlineColor", color);
        }
    }
}
