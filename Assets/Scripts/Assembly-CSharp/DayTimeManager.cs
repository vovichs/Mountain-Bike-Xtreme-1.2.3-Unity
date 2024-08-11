using UnityEngine;

public class DayTimeManager : MonoBehaviour
{
	[Header("Day Time Manager Parameters")]
	public DayTimeManagerParameters dtms;

	[Space(10f)]
	public DayState[] dayStates;

	[Space(10f)]
	private DayState currentDayState;

	private DayState targetDayState;

	[Space(10f)]
	public DayState interpolatedDayState;

	[Space(10f)]
	public Material[] materialsToModify;

	public Material[] materialsToColorize;

	[Space(10f)]
	public FogHeight fogHeight;

	private static DayTimeManager _instance;

	[Space(10f)]
	public OtherMaterials otherMaterial;

	private int _ColorA;

	private int _ColorB;

	private Color[] resultColors = new Color[2];

	private Keyframe kf = default(Keyframe);

	public float globalTime { get; set; }

	public float dayStateTime { get; set; }

	public CloudsManager cloudManager { get; set; }

	public static DayTimeManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindObjectOfType<DayTimeManager>();
			}
			return _instance;
		}
	}

	private void Start()
	{
		_ColorA = Shader.PropertyToID("_ColorA");
		_ColorB = Shader.PropertyToID("_ColorB");
		cloudManager = Object.FindObjectOfType<CloudsManager>();
		DayState[] array = dayStates;
		foreach (DayState dayState in array)
		{
			dayState.Start();
		}
		interpolatedDayState.Start();
		InvokeRepeating("DayTimeUpdate", 0f, 0.1f);
		globalTime = PlayerPrefs.GetFloat("globalTime");
	}

	public void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			globalTime = 0f;
		}
		else if (Input.GetKeyDown(KeyCode.O))
		{
			globalTime = 1f;
		}
		else if (Input.GetKeyDown(KeyCode.P))
		{
			globalTime = 2f;
		}
		else if (Input.GetKeyDown(KeyCode.L))
		{
			globalTime = 3f;
		}
	}

	private void DayTimeUpdate()
	{
		GetCurrentDayState();
		GetTargetDayState();
		GetInterpolatedDayState();
		SetGlobalMaterialValues();
		Refresh();
		SetCurveKeys();
	}

	public void Refresh()
	{
		globalTime += Time.deltaTime / (interpolatedDayState.duration * dtms.totalDayDuration);
		dayStateTime = globalTime - (float)(int)globalTime;
		if (globalTime >= 4f)
		{
			globalTime = 0f;
		}
		if (dtms.setTimeManualy)
		{
			globalTime = dtms.manualTime;
		}
		DayState[] array = dayStates;
		foreach (DayState dayState in array)
		{
			dayState.Update();
		}
	}

	public void GetCurrentDayState()
	{
		currentDayState = dayStates[(int)globalTime];
	}

	public void GetTargetDayState()
	{
		int num = (int)globalTime + 1;
		num = ((num != 4) ? num : 0);
		targetDayState = dayStates[num];
	}

	public void GetInterpolatedDayState()
	{
		interpolatedDayState.name = currentDayState.name;
		interpolatedDayState.duration = (currentDayState.duration + targetDayState.duration) / 2f;
		interpolatedDayState.intState.mainColor = Color.Lerp(currentDayState.intState.mainColor, targetDayState.intState.mainColor, dayStateTime);
		interpolatedDayState.intState.secondColor = Color.Lerp(currentDayState.intState.secondColor, targetDayState.intState.secondColor, dayStateTime);
		interpolatedDayState.intState.fogColor = Color.Lerp(currentDayState.intState.fogColor, targetDayState.intState.fogColor, dayStateTime);
		interpolatedDayState.intState.closeLayerValue = Mathf.Lerp(currentDayState.intState.closeLayerValue, targetDayState.intState.closeLayerValue, dayStateTime);
		interpolatedDayState.intState.closeLayerSaturation = Mathf.Lerp(currentDayState.intState.closeLayerSaturation, targetDayState.intState.closeLayerSaturation, dayStateTime);
		for (int i = 0; i < 2; i++)
		{
			interpolatedDayState.intState.colorCurveKeys[i].time = Mathf.Lerp(currentDayState.intState.colorCurveKeys[i].time, targetDayState.intState.colorCurveKeys[i].time, dayStateTime);
			interpolatedDayState.intState.colorCurveKeys[i].value = Mathf.Lerp(currentDayState.intState.colorCurveKeys[i].value, targetDayState.intState.colorCurveKeys[i].value, dayStateTime);
		}
		for (int j = 0; j < 2; j++)
		{
			interpolatedDayState.intState.valueCurveKeys[j].time = Mathf.Lerp(currentDayState.intState.valueCurveKeys[j].time, targetDayState.intState.valueCurveKeys[j].time, dayStateTime);
			interpolatedDayState.intState.valueCurveKeys[j].value = Mathf.Lerp(currentDayState.intState.valueCurveKeys[j].value, targetDayState.intState.valueCurveKeys[j].value, dayStateTime);
		}
		for (int k = 0; k < interpolatedDayState.intState.skyColors.Length; k++)
		{
			interpolatedDayState.intState.skyColors[k] = Color.Lerp(currentDayState.intState.skyColors[k], targetDayState.intState.skyColors[k], dayStateTime);
		}
		interpolatedDayState.intState.sunColor = Color.Lerp(currentDayState.intState.sunColor, targetDayState.intState.sunColor, dayStateTime);
		for (int l = 0; l < interpolatedDayState.intState.sunGlowColors.Length; l++)
		{
			interpolatedDayState.intState.sunGlowColors[l] = Color.Lerp(currentDayState.intState.sunGlowColors[l], targetDayState.intState.sunGlowColors[l], dayStateTime);
		}
		for (int m = 0; m < interpolatedDayState.intState.sunFlareColors.Length; m++)
		{
			interpolatedDayState.intState.sunFlareColors[m] = Color.Lerp(currentDayState.intState.sunFlareColors[m], targetDayState.intState.sunFlareColors[m], dayStateTime);
		}
		interpolatedDayState.intState.moonColor = Color.Lerp(currentDayState.intState.moonColor, targetDayState.intState.moonColor, dayStateTime);
		for (int n = 0; n < interpolatedDayState.intState.moonGlowColors.Length; n++)
		{
			interpolatedDayState.intState.moonGlowColors[n] = Color.Lerp(currentDayState.intState.moonGlowColors[n], targetDayState.intState.moonGlowColors[n], dayStateTime);
		}
		for (int num = 0; num < interpolatedDayState.intState.moonFlareColors.Length; num++)
		{
			interpolatedDayState.intState.moonFlareColors[num] = Color.Lerp(currentDayState.intState.moonFlareColors[num], targetDayState.intState.moonFlareColors[num], dayStateTime);
		}
		for (int num2 = 0; num2 < interpolatedDayState.intState.bottomSkyGlowColors.Length; num2++)
		{
			interpolatedDayState.intState.bottomSkyGlowColors[num2] = Color.Lerp(currentDayState.intState.bottomSkyGlowColors[num2], targetDayState.intState.bottomSkyGlowColors[num2], dayStateTime);
		}
		interpolatedDayState.intState.rainLevel = Mathf.Lerp(currentDayState.intState.rainLevel, targetDayState.intState.rainLevel, dayStateTime);
		interpolatedDayState.intState.overcastLevel = Mathf.Lerp(currentDayState.intState.overcastLevel, targetDayState.intState.overcastLevel, dayStateTime);
	}

	public void SetGlobalMaterialValues()
	{
		for (int i = 0; i < materialsToColorize.Length; i++)
		{
			float z = base.transform.position.z;
			float time = 0.0011627907f;
			Color mainColor = instance.interpolatedDayState.intState.mainColor;
			float H;
			float S;
			float V;
			Color.RGBToHSV(mainColor, out H, out S, out V);
			float t = dtms.distanceValueCurve.Evaluate(time);
			V = Mathf.Lerp(0.2f, 0.7f, t);
			V = 0.19f;
			mainColor = Color.HSVToRGB(H, S, V);
			time = instance.dtms.distanceColorCurve.Evaluate(time);
			resultColors[0] = Color.Lerp(mainColor, instance.interpolatedDayState.intState.secondColor, time);
			resultColors[1] = Color.Lerp(instance.interpolatedDayState.intState.fogColor, instance.interpolatedDayState.intState.secondColor, time);
			materialsToColorize[i].SetColor(_ColorA, resultColors[0]);
			materialsToColorize[i].SetColor(_ColorB, resultColors[1]);
		}
	}

	private void SetCurveKeys()
	{
		if (dtms.setKeys)
		{
			kf.time = interpolatedDayState.intState.colorCurveKeys[0].time;
			kf.value = interpolatedDayState.intState.colorCurveKeys[0].value;
			dtms.distanceColorCurve.MoveKey(2, kf);
			kf.time = interpolatedDayState.intState.colorCurveKeys[1].time;
			kf.value = interpolatedDayState.intState.colorCurveKeys[1].value;
			dtms.distanceColorCurve.MoveKey(3, kf);
			kf.time = interpolatedDayState.intState.valueCurveKeys[0].time;
			kf.value = interpolatedDayState.intState.valueCurveKeys[0].value;
			dtms.distanceValueCurve.MoveKey(2, kf);
			kf.time = interpolatedDayState.intState.valueCurveKeys[1].time;
			kf.value = interpolatedDayState.intState.valueCurveKeys[1].value;
			dtms.distanceValueCurve.MoveKey(3, kf);
		}
		else
		{
			currentDayState.currentState.colorCurveKeys[0].time = dtms.distanceColorCurve.keys[2].time;
			currentDayState.currentState.colorCurveKeys[0].value = dtms.distanceColorCurve.keys[2].value;
			currentDayState.currentState.colorCurveKeys[1].time = dtms.distanceColorCurve.keys[3].time;
			currentDayState.currentState.colorCurveKeys[1].value = dtms.distanceColorCurve.keys[3].value;
			currentDayState.currentState.valueCurveKeys[0].time = dtms.distanceValueCurve.keys[2].time;
			currentDayState.currentState.valueCurveKeys[0].value = dtms.distanceValueCurve.keys[2].value;
			currentDayState.currentState.valueCurveKeys[1].time = dtms.distanceValueCurve.keys[3].time;
			currentDayState.currentState.valueCurveKeys[1].value = dtms.distanceValueCurve.keys[3].value;
		}
	}
}
