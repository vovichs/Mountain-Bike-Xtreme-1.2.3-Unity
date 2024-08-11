using System;
using UnityEngine;

[Serializable]
public class DayTimeManagerParameters
{
	public float totalDayDuration;

	[Range(0f, 3.999f)]
	public float manualTime;

	public bool setTimeManualy;

	public AnimationCurve distanceValueCurve;

	public AnimationCurve distanceColorCurve;

	public bool setKeys;

	[Range(0f, 1f)]
	public float closeLayerValue = 0.32f;

	[Range(1f, 2f)]
	public float clouseLayerSaturation = 1.4f;
}
