using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MText;

[RequireComponent(typeof(AudioSource))]
public class KHJ_Diary : MonoBehaviour
{
	/* not use
	//�ڹ��� ��ư��
	public KHJ_DiaryButton[] buttons;
	public GameObject PasswordHint;
		//�ڹ��� + �ڹ��� ��ư
	public GameObject Lock;
	*/

	public static KHJ_Diary instance;
	//���ȴ���
	public bool isOpened;
	public AnimatedBookController bookController;
	//��й�ȣ �Է� ���� ��������
	public bool able;
	//����
	public AudioClip clickSound;
	public AudioClip correctSound;
	public AudioClip incorrectSound;
	AudioSource source;
	//��
	/*old
	 * int Answer
	public List<int> AnswerList;
	public List<int> ButtonInputList;
	 */
	public string answer;
	string answerInput;
	public Modular3DText hintText;
	public List<KHJ_DiaryButton> inputButtonList = new List<KHJ_DiaryButton>();
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}
	void Start()
	{
		bookController = GetComponent<AnimatedBookController>();
		source = GetComponent<AudioSource>();

		/*old
		Answer = PasswordHint.GetComponent<MText.Modular3DText>().Text;
		//��Ʈ�� �ִ� ���� AnswerList�� �߰�
		for (int i =0; i< Answer.ToString().Length; i++)
        {
			AnswerList.Add(int.Parse(Answer[i].ToString()));
        }*/

		hintText.Text = answer;
		able = true;
	}
	//void Update()
	//{
	//	/* not use
	//	//if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
	//	if (Input.GetButtonDown("Fire1") && able)
	//	{
	//		//Ray ray = new Ray(trRight.position, trRight.forward);
	//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	//		RaycastHit hitInfo;
	//		if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
	//		{			
	//			//Ű�е� Ŭ�� ����
 //               if (hitInfo.collider.name.Contains("KeyPad"))
 //               {
	//				hitInfo.collider.gameObject.GetComponent<KHJ_DiaryButton>().ClickButton();
 //               }
	//		}
	//	}
	//	//���/���� ����
	//	if (!isOpened)
 //       {
	//		Lock.SetActive(true);
	//		return;
 //       }
 //       else
 //       {
	//		Lock.SetActive(false);
 //       }
	//	//å �ѱ�� ����
	//	if (Input.GetKeyDown(KeyCode.RightArrow))
	//	{
	//		//�� ��������
	//		TurnPrePage();
	//	}
	//	else if (Input.GetKeyDown(KeyCode.LeftArrow))
	//	{
	//		//�� ��������
	//		TurnNextPage();
	//	}

 //       if (Input.GetKeyDown(KeyCode.Alpha3))
 //       {
	//		//å ����
	//		CloseBook();
 //       }
	//	*/
	//}

	public void Input(KHJ_DiaryButton button)
	{
		if(button.num == "0")
        {
			ClearBtn();
			return;
        }

		if (inputButtonList.Contains(button))
			return;

		button.BtnInputEft();
		answerInput += button.num;
		inputButtonList.Add(button);

		Debug.Log("Answer : " + answerInput);
		if (CheckAnswer())
		{
			isOpened = true;
			PlaySound(correctSound);
			TurnPrePage();
		}
		else
		{
			able = true;
			PlaySound(incorrectSound);
			ClearBtn();
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

	/* old
	public bool CheckAnswer()
    {
		bool check = false;
		List<int> duplicates = ButtonInputList.Intersect(AnswerList).ToList();
		if (duplicates.Count == AnswerList.Count)
			check = true;

		return check;
    }*/

	public bool CheckAnswer()
	{
		if (answer == answerInput)
			return true;

		if (answer.Length == answerInput.Length)
			answerInput = null;

		return false;
	}

	/*old
	public void ClearBtn()
	{
		ButtonInputList.Clear();
		foreach (KHJ_DiaryButton btn in buttons)
		{
			btn.ClearButton();
		}
	}
	*/
	public void ClearBtn()
	{
		foreach (KHJ_DiaryButton btn in inputButtonList)
		{
			btn.ClearButton();
		}
		inputButtonList.Clear();
	}
	public void PlaySound(AudioClip clip)
	{
		source.PlayOneShot(clip);
	}
}
