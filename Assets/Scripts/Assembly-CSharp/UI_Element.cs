using System;
using UnityEngine;
using UnityEngine.Events;

public class UI_Element : MonoBehaviour
{
	[Serializable]
	public class SwipeEvent : UnityEvent<RectTransform, float, float>
	{
	}

	[Serializable]
	public class DragEvent : UnityEvent<RectTransform, float>
	{
	}

	public SwipeEvent se;

	public DragEvent de;

	public RectTransform rc;

	public float duration;

	public float offset;

	public float dragEventSpeed;

	public void Update()
	{
		se.Invoke(rc, duration, offset);
		de.Invoke(rc, dragEventSpeed);
	}
}
