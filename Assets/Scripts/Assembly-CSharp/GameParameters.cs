using UnityEngine;
using UnityEngine.UI;

public class GameParameters : MonoBehaviour
{
	public static float effectsVolume;

	public static float musicVolume;

	public static float controlsOpacity;

	public Slider effectsVolumeSlider;

	public Slider musicVolumeSlider;

	public Slider controlsOpacitySlider;

	private void Start()
	{
		if (PlayerPrefs.GetInt("runCount") == 0)
		{
			PlayerPrefs.SetFloat("effectsVolume", 1f);
			PlayerPrefs.SetFloat("musicVolume", 1f);
			PlayerPrefs.SetFloat("controlsOpacity", 0.5f);
		}
		effectsVolumeSlider.value = PlayerPrefs.GetFloat("effectsVolume");
		musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
		controlsOpacitySlider.value = PlayerPrefs.GetFloat("controlsOpacity");
		effectsVolume = effectsVolumeSlider.value;
		musicVolume = musicVolumeSlider.value;
		controlsOpacity = controlsOpacitySlider.value;
	}

	public void SetEffectsVolume(Slider _slider)
	{
		effectsVolume = _slider.value;
		PlayerPrefs.SetFloat("effectsVolume", effectsVolume);
	}

	public void SetMusicVolume(Slider _slider)
	{
		musicVolume = _slider.value;
		PlayerPrefs.SetFloat("musicVolume", musicVolume);
	}

	public void SetControlsOpacityVolume(Slider _slider)
	{
		controlsOpacity = _slider.value;
		PlayerPrefs.SetFloat("controlsOpacity", controlsOpacity);
	}

	public void SetControlsOpacityPreviewAlpha(Image _image)
	{
		Color color = _image.color;
		color.a = controlsOpacity;
		_image.color = color;
	}
}
