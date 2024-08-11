using System;
using UnityEngine;

[Serializable]
public class WeatherTypes : IComparable
{
	public WeatherTypeNames weatherTypeName;

	[Range(0f, 2.999f)]
	public float tWeatherTargetValue;

	[Range(0f, 1f)]
	public float chance;

	public float duration;

	[Range(0f, 1f)]
	public float rainLevel;

	[Range(0f, 1f)]
	public float overCastLevel;

	public int CompareTo(object obj)
	{
		WeatherTypes weatherTypes = obj as WeatherTypes;
		return chance.CompareTo(weatherTypes.chance);
	}
}
