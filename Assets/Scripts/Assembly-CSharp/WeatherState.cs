using System;
using UnityEngine;

[Serializable]
public class WeatherState
{
	public Color mainColor;

	public Color secondColor;

	public Color fogColor;

	[Range(0f, 1f)]
	public float closeLayerValue = 0.32f;

	[Range(1f, 2f)]
	public float closeLayerSaturation = 1.4f;

	public AnimationCurveKey[] colorCurveKeys;

	public AnimationCurveKey[] valueCurveKeys;

	public Color[] skyColors;

	public Color sunColor;

	public Color[] sunGlowColors;

	public Color[] sunFlareColors;

	public Color moonColor;

	public Color[] moonGlowColors;

	public Color[] moonFlareColors;

	public Color[] bottomSkyGlowColors;

	[Range(0f, 1f)]
	public float tWeatherTargetValue;

	[Range(0f, 1f)]
	public float chance;

	public float duration;

	public float rainLevel;

	public float overcastLevel;

	public WeatherState()
	{
		skyColors = new Color[3];
		sunGlowColors = new Color[2];
		sunFlareColors = new Color[2];
		moonGlowColors = new Color[2];
		moonFlareColors = new Color[2];
		bottomSkyGlowColors = new Color[2];
		colorCurveKeys = new AnimationCurveKey[2];
		colorCurveKeys[0] = new AnimationCurveKey();
		colorCurveKeys[1] = new AnimationCurveKey();
		valueCurveKeys = new AnimationCurveKey[2];
		valueCurveKeys[0] = new AnimationCurveKey();
		valueCurveKeys[1] = new AnimationCurveKey();
		colorCurveKeys[0].time = 0.03410631f;
		colorCurveKeys[0].value = 0.144488f;
		colorCurveKeys[1].time = 0.2879381f;
		colorCurveKeys[1].value = 0.5010018f;
		valueCurveKeys[0].time = 0.03410631f;
		valueCurveKeys[0].value = 0.144488f;
		valueCurveKeys[1].time = 0.2879381f;
		valueCurveKeys[1].value = 0.5010018f;
	}
}
