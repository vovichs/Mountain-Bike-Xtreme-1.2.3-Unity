using UnityEngine;

public class SunMoon : MonoBehaviour
{
	public Transform sunMoonTransform;

	public GameObject sunMoonObject;

	public GameObject sunGo;

	public GameObject sunFlareGo;

	public GameObject sunGlowGo;

	[HideInInspector]
	public SpriteRenderer sunRend;

	[HideInInspector]
	public SpriteRenderer sunFlareRend;

	[HideInInspector]
	public SpriteRenderer sunGlowRend;

	public GameObject moonGo;

	public GameObject moonFlareGo;

	public GameObject moonGlowGo;

	[HideInInspector]
	public SpriteRenderer moonRend;

	[HideInInspector]
	public SpriteRenderer moonFlareRend;

	[HideInInspector]
	public SpriteRenderer moonGlowRend;

	public GameObject bottomSkyGlow;

	[HideInInspector]
	public SpriteRenderer bottomSkyGlowRend;

	public AnimationCurve moonOpacityByTime;

	public AnimationCurve sunFlareOpacityByTime;

	private float startTime;

	private int _ColorA;

	private int _ColorB;

	private void Start()
	{
		startTime = Time.deltaTime;
		sunRend = sunGo.GetComponent<SpriteRenderer>();
		sunGlowRend = sunGlowGo.GetComponent<SpriteRenderer>();
		sunFlareRend = sunFlareGo.GetComponent<SpriteRenderer>();
		moonRend = moonGo.GetComponent<SpriteRenderer>();
		moonGlowRend = moonGlowGo.GetComponent<SpriteRenderer>();
		moonFlareRend = moonFlareGo.GetComponent<SpriteRenderer>();
		bottomSkyGlowRend = bottomSkyGlow.GetComponent<SpriteRenderer>();
		_ColorA = Shader.PropertyToID("_ColorA");
		_ColorB = Shader.PropertyToID("_ColorB");
	}

	private void LateUpdate()
	{
		Rotate();
		SetColors();
	}

	private void Rotate()
	{
		float num = Time.time - startTime;
		float num2 = num / DayTimeManager.instance.dtms.totalDayDuration;
		if (num2 >= 1f)
		{
			startTime = Time.time;
		}
		float globalTime = DayTimeManager.instance.globalTime;
		globalTime = Mathf.Clamp(globalTime, 0f, 4f);
		globalTime /= 4f;
		float z = Mathf.Lerp(0f, -360f, globalTime);
		sunMoonTransform.eulerAngles = new Vector3(0f, 0f, z);
	}

	private void SetColors()
	{
		Color sunColor = DayTimeManager.instance.interpolatedDayState.intState.sunColor;
		sunRend.color = sunColor;
		Color[] sunGlowColors = DayTimeManager.instance.interpolatedDayState.intState.sunGlowColors;
		sunGlowRend.color = sunGlowColors[0];
		Color[] sunFlareColors = DayTimeManager.instance.interpolatedDayState.intState.sunFlareColors;
		sunFlareRend.color = sunFlareColors[0];
		Color moonColor = DayTimeManager.instance.interpolatedDayState.intState.moonColor;
		moonRend.color = moonColor;
		Color[] moonGlowColors = DayTimeManager.instance.interpolatedDayState.intState.moonGlowColors;
		moonGlowRend.color = moonGlowColors[0];
		Color[] moonFlareColors = DayTimeManager.instance.interpolatedDayState.intState.moonFlareColors;
		moonFlareRend.color = moonFlareColors[0];
		Color[] bottomSkyGlowColors = DayTimeManager.instance.interpolatedDayState.intState.bottomSkyGlowColors;
		bottomSkyGlowRend.color = bottomSkyGlowColors[0];
	}
}
