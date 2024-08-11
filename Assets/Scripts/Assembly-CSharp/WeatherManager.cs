using System;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
	[Range(0f, 2.999f)]
	public float tWeather;

	public bool tSetManually;

	public WeatherTypes[] weatherTypes;

	public WeatherTypes currentWeatherType;

	public WeatherTypes targetWeatherType;

	public ParticleSystem rainCaster;

	private static WeatherManager _instance;

	private float weatherTimer;

	public WeatherTypes interpolatedWeatherType { get; set; }

	public static WeatherManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType<WeatherManager>();
			}
			return _instance;
		}
	}

	private void Start()
	{
		tWeather = PlayerPrefs.GetFloat("tWeather");
		interpolatedWeatherType = new WeatherTypes();
		tWeather = 0f;
	}

	public void DefineCurrentWeather()
	{
		if (currentWeatherType == null)
		{
			float num = UnityEngine.Random.Range(0f, 1f);
			Array.Sort(weatherTypes);
			float num2 = 0f;
			for (int i = 0; i < weatherTypes.Length; i++)
			{
				num2 += weatherTypes[i].chance;
				if (num2 >= num)
				{
					currentWeatherType = weatherTypes[i];
					break;
				}
			}
		}
		else
		{
			currentWeatherType = targetWeatherType;
		}
		DefineTargetWeather();
		weatherTimer = 0f;
	}

	public void DefineTargetWeather()
	{
		float num = UnityEngine.Random.Range(0f, 1f);
		Array.Sort(weatherTypes);
		float num2 = 0f;
		for (int i = 0; i < weatherTypes.Length; i++)
		{
			num2 += weatherTypes[i].chance;
			if (num2 >= num)
			{
				targetWeatherType = weatherTypes[i];
				break;
			}
		}
		weatherTimer = 0f;
	}

	private void DefineInterpolatedWeather()
	{
		float t = weatherTimer / (currentWeatherType.duration / 2f);
		tWeather = Mathf.Lerp(currentWeatherType.tWeatherTargetValue, targetWeatherType.tWeatherTargetValue, t);
		interpolatedWeatherType.overCastLevel = Mathf.Lerp(currentWeatherType.overCastLevel, targetWeatherType.overCastLevel, t);
		interpolatedWeatherType.rainLevel = Mathf.Lerp(currentWeatherType.rainLevel, targetWeatherType.rainLevel, t);
	}

	private void Update()
	{
		if (weatherTimer > currentWeatherType.duration)
		{
			DefineCurrentWeather();
		}
		if (!tSetManually)
		{
			weatherTimer += Time.deltaTime;
			DefineInterpolatedWeather();
		}
	}
}
