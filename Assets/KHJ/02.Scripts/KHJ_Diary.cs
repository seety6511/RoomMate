using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class KHJ_Diary : MonoBehaviour
{
	public static KHJ_Diary instance;
	//열렸는지
	public bool isOpened;
	//자물쇠 + 자물쇠 버튼
	public GameObject Lock;
	//자물쇠 버튼들
	public KHJ_DiaryButton[] buttons;
	public AnimatedBookController bookController;
	public GameObject PasswordHint;
	//비밀번호 입력 가능 상태인지
	public bool able;
	//사운드
	public AudioClip clickSound;
	public AudioClip correctSound;
	public AudioClip incorrectSound;
	AudioSource source; 
	//답
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
		//힌트에 있는 답을 AnswerList에 추가
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
				//키패드 클릭 실행
                if (hitInfo.collider.name.Contains("KeyPad"))
                {
					hitInfo.collider.gameObject.GetComponent<KHJ_DiaryButton>().ClickButton();
                }
			}
		}

		//잠금/해제 셋팅
		if (!isOpened)
        {
			Lock.SetActive(true);
			return;
        }
        else
        {
			Lock.SetActive(false);
        }
		//책 넘기기 조작
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			//뒷 페이지로
			TurnPrePage();
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			//앞 페이지로
			TurnNextPage();
		}

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
			//책 덮기
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
