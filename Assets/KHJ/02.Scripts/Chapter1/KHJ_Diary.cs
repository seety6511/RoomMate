using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MText;

[RequireComponent(typeof(AudioSource))]
public class KHJ_Diary : MonoBehaviour
{
	public static KHJ_Diary instance;
	//자물쇠 + 자물쇠 버튼
	public GameObject locker;
	public GameObject BookControlBox;

	//열렸는지
	public bool isOpened;
	public AnimatedBookController bookController;

	//사운드
	public AudioClip clickSound;
	public AudioClip correctSound;
	public AudioClip incorrectSound;
	AudioSource source;
	//답
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
		string text = null;
		foreach (var t in answer)
		{
			text += t.num;
		}
		hintText.UpdateText(text);
		BookControlBox.SetActive(false);
	}
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
		//Destroy(locker.gameObject, 5f);
		yield return new WaitForSeconds(5);
		locker.SetActive(false);
		bookController.TurnToPreviousPage();
		BookControlBox.SetActive(true);
	}
	IEnumerator FailEvent()
	{
		Debug.Log("False");
		PlaySound(incorrectSound);
		yield return new WaitForSeconds(1f);
		ClearBtn();
	}
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
