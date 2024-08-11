using System;
using UnityEngine;

[Serializable]
public class DayState
{
	public string name;

	public float duration;

	public WeatherState[] weatherStates;

	public WeatherState intState { get; set; }

	public WeatherState currentState { get; set; }

	public WeatherState targetState { get; set; }

	public void Start()
	{
		intState = new WeatherState();
	}

	public void Update()
	{
		DefineCurrentState();
		DefineTargetState();
		InterpolateStates();
	}

	private void DefineCurrentState()
	{
		currentState = weatherStates[(int)WeatherManager.instance.tWeather];
	}

	private void DefineTargetState()
	{
		int num = (int)WeatherManager.instance.tWeather + 1;
		num = ((num < 3) ? num : 0);
		targetState = weatherStates[num];
	}

	private void InterpolateStates()
	{
		float tWeather = WeatherManager.instance.tWeather;
		float t = tWeather - (float)(int)tWeather;
		intState.mainColor = Color.Lerp(currentState.mainColor, targetState.mainColor, t);
		intState.secondColor = Color.Lerp(currentState.secondColor, targetState.secondColor, t);
		intState.fogColor = Color.Lerp(currentState.fogColor, targetState.fogColor, t);
		intState.closeLayerValue = Mathf.Lerp(currentState.closeLayerValue, targetState.closeLayerValue, t);
		intState.closeLayerSaturation = Mathf.Lerp(currentState.closeLayerSaturation, targetState.closeLayerSaturation, t);
		for (int i = 0; i < 2; i++)
		{
			intState.colorCurveKeys[i].time = Mathf.Lerp(currentState.colorCurveKeys[i].time, targetState.colorCurveKeys[i].time, t);
			intState.colorCurveKeys[i].value = Mathf.Lerp(currentState.colorCurveKeys[i].value, targetState.colorCurveKeys[i].value, t);
		}
		for (int j = 0; j < 2; j++)
		{
			intState.valueCurveKeys[j].time = Mathf.Lerp(currentState.valueCurveKeys[j].time, targetState.valueCurveKeys[j].time, t);
			intState.valueCurveKeys[j].value = Mathf.Lerp(currentState.valueCurveKeys[j].value, targetState.valueCurveKeys[j].value, t);
		}
		for (int k = 0; k < 3; k++)
		{
			intState.skyColors[k] = Color.Lerp(currentState.skyColors[k], targetState.skyColors[k], t);
		}
		intState.sunColor = Color.Lerp(currentState.sunColor, targetState.sunColor, t);
		for (int l = 0; l < 2; l++)
		{
			intState.sunGlowColors[l] = Color.Lerp(currentState.sunGlowColors[l], targetState.sunGlowColors[l], t);
		}
		for (int m = 0; m < 2; m++)
		{
			intState.sunFlareColors[m] = Color.Lerp(currentState.sunFlareColors[m], targetState.sunFlareColors[m], t);
		}
		intState.moonColor = Color.Lerp(currentState.moonColor, targetState.moonColor, t);
		for (int n = 0; n < 2; n++)
		{
			intState.moonGlowColors[n] = Color.Lerp(currentState.moonGlowColors[n], targetState.moonGlowColors[n], t);
		}
		for (int num = 0; num < 2; num++)
		{
			intState.moonFlareColors[num] = Color.Lerp(currentState.moonFlareColors[num], targetState.moonFlareColors[num], t);
		}
		for (int num2 = 0; num2 < 2; num2++)
		{
			intState.bottomSkyGlowColors[num2] = Color.Lerp(currentState.bottomSkyGlowColors[num2], targetState.bottomSkyGlowColors[num2], t);
		}
		intState.rainLevel = Mathf.Lerp(currentState.rainLevel, targetState.rainLevel, t);
		intState.overcastLevel = Mathf.Lerp(currentState.overcastLevel, targetState.overcastLevel, t);
	}
}
