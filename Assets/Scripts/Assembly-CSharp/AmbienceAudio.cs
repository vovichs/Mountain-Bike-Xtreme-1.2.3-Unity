using System;
using UnityEngine;

[Serializable]
public class AmbienceAudio
{
	public string name;

	public AudioSource audioSource;

	public AnimationCurve volumeByTime;

	public AnimationCurve volumeByWeather;
}
