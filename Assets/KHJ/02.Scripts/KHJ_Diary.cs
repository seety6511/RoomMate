using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHJ_Diary : MonoBehaviour
{
	public bool isOpened;
	public GameObject Lock;
	public AnimatedBookController bookController;
	void Start()
	{
		bookController = GetComponent<AnimatedBookController>();
	}

	void Update()
	{
		if (!isOpened)
        {
			Lock.SetActive(true);
			return;
        }
        else
        {
			Lock.SetActive(false);
        }

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			bookController.TurnToPreviousPage();
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			bookController.TurnToNextPage();
		}

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
			bookController.CloseBook();
        }
	}
}
