using System;
using UnityEngine;

[Serializable]
public class AnimationCurveKey
{
	[Range(0.01810617f, 1f)]
	public float time;

	[Range(0f, 1f)]
	public float value;
}
