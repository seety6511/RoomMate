using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_DoorLock : MonoBehaviour
{
    public string inputNumber;

    public Transform numberParent;
    public GameObject numberText;
    public SH_DoorLockButtton[] buttons;

    SH_ConnectManager manager;

    [SerializeField]
    bool unlock;
    [SerializeField]
    Scene scene;

    private void Awake()
    {
        inputNumber = "";
        buttons = GetComponentsInChildren<SH_DoorLockButtton>();
        manager = FindObjectOfType<SH_ConnectManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            AddNumber("1");
        if (Input.GetKeyDown(KeyCode.Alpha2))
            AddNumber("2");
        if (Input.GetKeyDown(KeyCode.Alpha3))
            AddNumber("3");
        if (Input.GetKeyDown(KeyCode.Alpha4))
            AddNumber("4");
        if (Input.GetKeyDown(KeyCode.Alpha5))
            AddNumber("5");
        if (Input.GetKeyDown(KeyCode.Alpha6))
            AddNumber("6");
        if (Input.GetKeyDown(KeyCode.Alpha7))
            AddNumber("7");
        if (Input.GetKeyDown(KeyCode.Alpha8))
            AddNumber("8");
        if (Input.GetKeyDown(KeyCode.Alpha9))
            AddNumber("9");
        if (Input.GetKeyDown(KeyCode.Alpha0))
            AddNumber("0");
        if (Input.GetKeyDown(KeyCode.Q))
            AddNumber("*");
        if (Input.GetKeyDown(KeyCode.W))
            AddNumber("#");
    }

    public void Clear()
    {
        inputNumber = "";
        int c = texts.Count;
        for(int i = 0; i < c; ++i)
        {
            var temp = texts[0];
            texts.RemoveAt(0);
            Destroy(temp);
        }
    }

    List<GameObject> texts = new List<GameObject>();
    public void AddNumber(string n)
    {
        if (n == "*")
        {
            Clear();
            return;
        }
        else if (n == "#")
        {
            manager.Check(inputNumber);
            Clear();
            return;
        }

        if (inputNumber!= null && inputNumber.Length == 4)
            return;

        inputNumber += n;
        var num = Instantiate(numberText, numberParent);
        num.GetComponent<Text>().text = n;
        texts.Add(num.gameObject);
        
    }
}
