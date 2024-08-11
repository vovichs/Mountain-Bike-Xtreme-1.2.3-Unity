using System;
using System.Collections;
using UnityEngine;

public class Animations
{
	public static void FadeColor(FadeColorArgs fadeColorArgs)
	{
		MonoManager.inst.StartCoroutine(FadeColor_IE(fadeColorArgs));
	}

	private static IEnumerator FadeColor_IE(FadeColorArgs fca)
	{
		float t = 0f;
		float tSmoothed2 = 0f;
		float startTime = Time.time;
		while (t < 1f)
		{
			float totalDuration = Time.time - startTime;
			t = totalDuration / fca.duration;
			tSmoothed2 = (1f - Mathf.Cos(t * (float)Math.PI)) / 2f;
			fca.elem.color = Color.Lerp(fca.colorA, fca.colorB, tSmoothed2);
			yield return null;
		}
	}

	public static void MoveTo(MoveToArgs moveToArgs)
	{
		MonoManager.inst.StartCoroutine(MoveTo_IE(moveToArgs));
	}

	private static IEnumerator MoveTo_IE(MoveToArgs mta)
	{
		float t2 = 0f;
		float startTime = Time.time;
		while (t2 < 1f)
		{
			float totalDuration = Time.time - startTime;
			t2 = totalDuration / mta.duration;
			t2 = Mathf.Pow(t2, 3.5f) / (Mathf.Pow(t2, 3.5f) + Mathf.Pow(1f - t2, 3.5f));
			mta.elem.anchoredPosition = Vector3.Lerp(mta.posA, mta.posB, t2);
			yield return null;
		}
	}

	private static float Smooth(float t)
	{
		return t * t * t * (t * (t * 6f - 15f) + 10f);
	}
}
