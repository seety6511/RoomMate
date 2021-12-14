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
	//��й�ȣ �Է� ���� ��������
	public bool able;
	*/

	public static KHJ_Diary instance;
	//�ڹ��� + �ڹ��� ��ư
	public GameObject locker;
	//���ȴ���
	public bool isOpened;
	public AnimatedBookController bookController;

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
	public Modular3DText hintText;
	public List<KHJ_DiaryButton> answer = new List<KHJ_DiaryButton>();
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
		string text = null;
		foreach (var t in answer)
		{
			text += t.num;
		}
		hintText.UpdateText(text);
	}
	/* not use
	//void Update()
	//{
	//	
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
*/
	//}

	public void Input(KHJ_DiaryButton button)
	{
		if (button.num == "0")
		{
			ClearBtn();
			return;
		}

		if (inputButtonList.Contains(button))
		{
			return;
		}

		button.BtnInputEft();
		inputButtonList.Add(button);

		CheckAnswer();
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

	public void CheckAnswer()
	{
		if (answer.Count != inputButtonList.Count)
		{
			Debug.Log("Incorrect Answer");
			return;
		}

		for (int i = 0; i < answer.Count; ++i)
		{
			if (!answer.Contains(inputButtonList[i]))
			{
				StartCoroutine(FailEvent());
				return;
			}
		}
		StartCoroutine(OpenEvent());
	}

	IEnumerator OpenEvent()
	{
		Debug.Log("Clear");
		isOpened = true;
		PlaySound(correctSound);
		yield return new WaitForSeconds(1f);
		//Lock.Fracture();
		locker.gameObject.transform.parent = null;
		locker.gameObject.GetComponent<Rigidbody>().useGravity = true;
		locker.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		Destroy(locker.gameObject, 5f);
		bookController.TurnToPreviousPage();
	}

	IEnumerator FailEvent()
	{
		Debug.Log("False");
		PlaySound(incorrectSound);
		yield return new WaitForSeconds(1f);
		ClearBtn();
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
		Debug.Log("ClearBTn");
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
