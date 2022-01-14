using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KHJ_GhostMove : MonoBehaviour
{
	public PathType pathType = PathType.CatmullRom;
	public Vector3[] waypoints = new[] {
		new Vector3(-0.03f, 0.012f, -0.015f),
		new Vector3(0, 0.022f, 0.015f),
		new Vector3(0.04f, -0.007f, 0.012f),
	};
	void Start()
	{
		//이동
		for(int i = 0; i < waypoints.Length; i++)
        {
			waypoints[i].x += transform.localPosition.x;
			waypoints[i].y += transform.localPosition.y;
			waypoints[i].z += transform.localPosition.z;
        }
		Tween t = transform.DOLocalPath(waypoints, 4, pathType)
			.SetOptions(true);
		t.SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Restart);
		//회전
		Vector3 tmp = transform.localRotation.eulerAngles;
		tmp.x -= 30;
		tmp.z += 15;
		Tween t1 = transform.DOLocalRotate(tmp, 2f)
			.SetOptions(true);
		t1.SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
	}
}
