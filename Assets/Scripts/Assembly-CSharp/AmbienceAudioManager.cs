using UnityEngine;

public class AmbienceAudioManager : MonoBehaviour
{
	public AmbienceAudio[] ambienceAudio;

	private static AmbienceAudioManager _instance;

	public float musicVolume { get; set; }

	public float effectsVolume { get; set; }

	public static AmbienceAudioManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindObjectOfType<AmbienceAudioManager>();
			}
			return _instance;
		}
	}

	private void Start()
	{
		musicVolume = PlayerPrefs.GetFloat("musicVolume");
		effectsVolume = PlayerPrefs.GetFloat("effectsVolume");
	}

	private void Update()
	{
		AmbienceAudio[] array = ambienceAudio;
		foreach (AmbienceAudio audio in array)
		{
			PlayAudio(audio);
		}
	}

	private void PlayAudio(AmbienceAudio _audio)
	{
		float num = _audio.volumeByTime.Evaluate(DayTimeManager.instance.globalTime);
		float num2 = _audio.volumeByWeather.Evaluate(WeatherManager.instance.tWeather);
		num -= num2;
		_audio.audioSource.volume = num * effectsVolume;
	}
}
