using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class KHJ_Diary : MonoBehaviour
{
	public static KHJ_Diary instance;
	public bool isOpened;
	public GameObject Lock;
	public KHJ_DiaryButton[] buttons;
	public AnimatedBookController bookController;
	public GameObject PasswordHint;
	public bool able;

	public AudioClip clickSound;
	public AudioClip correctSound;
	public AudioClip incorrectSound;
	AudioSource source; 

	string Answer;
	public List<int> AnswerList;
	public List<int> ButtonInputList;
    private void Awake()
    {
        if(instance == null)
        {
			instance = this;
        }
        else
        {
			Destroy(gameObject);
        }
    }
    void Start()
	{
		bookController = GetComponent<AnimatedBookController>();
		Answer = PasswordHint.GetComponent<MText.Modular3DText>().Text;
		source = GetComponent<AudioSource>();

		for (int i =0; i< Answer.ToString().Length; i++)
        {
			AnswerList.Add(int.Parse(Answer[i].ToString()));
        }
		able = true;
	}
	void Update()
	{
		//if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
		if (Input.GetButtonDown("Fire1") && able)
		{
			//Ray ray = new Ray(trRight.position, trRight.forward);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
			{			
                if (hitInfo.collider.name.Contains("KeyPad"))
                {
					hitInfo.collider.gameObject.GetComponent<KHJ_DiaryButton>().ClickButton();
                }
			}
		}

		//셋팅
		if (!isOpened)
        {
			Lock.SetActive(true);
			return;
        }
        else
        {
			Lock.SetActive(false);
        }
		//조작
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			TurnPrePage();
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			TurnNextPage();
		}

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
			CloseBook();
        }
	}
	public void TurnPrePage()
    {
		bookController.TurnToPreviousPage();
	}
	public void TurnNextPage()
    {
		bookController.TurnToNextPage();
	}
	public void CloseBook()
    {
		bookController.CloseBook();
	}

	public bool CheckAnswer()
    {
		bool check = false;
		List<int> duplicates = ButtonInputList.Intersect(AnswerList).ToList();
		if (duplicates.Count == AnswerList.Count)
			check = true;

		return check;
    }

	public void ClearBtn()
    {
		ButtonInputList.Clear();
		foreach(KHJ_DiaryButton btn in buttons)
        {
			btn.ClearButton();
        }
    }
	public void PlaySound(AudioClip clip)
    {
		source.PlayOneShot(clip);
	}
}
