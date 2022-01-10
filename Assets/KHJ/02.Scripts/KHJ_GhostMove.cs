using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class KHJ_GhostMove : MonoBehaviour
{
	public Transform target;
	public PathType pathType = PathType.CatmullRom;
	public Vector3 OriginPos;
	public Vector3[] waypoints = new[] {
		new Vector3(0, 0, 0),
		new Vector3(0, 0.1f, 0.022f),
		new Vector3(0, 0.24f, 0.29f),
		new Vector3(0, -0.07f, 0.12f),
	};

	void Start()
	{
		OriginPos = transform.localPosition;
		target = transform;
		// Create a path tween using the given pathType, Linear or CatmullRom (curved).
		// Use SetOptions to close the path
		// and SetLookAt to make the target orient to the path itself
		Tween t = target.DOLocalPath(waypoints, 4, pathType)
			.SetOptions(true)
			.SetLookAt(0.001f);
		// Then set the ease to Linear and use infinite loops
		t.SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);


		//purpleCube.DOMove(new Vector3(6, 0, 0), 2).SetRelative();
		// Also, let's set the color tween to loop infinitely forward and backwards
		//purpleCube.GetComponent<Renderer>().material.DOColor(Color.yellow, 2).SetLoops(-1, LoopType.Yoyo);
	}
}
