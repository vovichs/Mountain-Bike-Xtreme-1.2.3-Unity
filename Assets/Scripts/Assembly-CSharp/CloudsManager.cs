using UnityEngine;

public class CloudsManager : MonoBehaviour
{
	[Range(0f, 3f)]
	public float overcast;

	public CloudObject[] cloudObjects;

	public Material cloudMaterial;

	private Cloud[] cloudComps;

	private static CloudsManager _instance;

	public static CloudsManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindObjectOfType<CloudsManager>();
			}
			return _instance;
		}
	}

	public void Start()
	{
		CloudObject[] array = cloudObjects;
		foreach (CloudObject cloudObject in array)
		{
			cloudObject.sprRend = cloudObject.cloudGameObject.GetComponent<SpriteRenderer>();
		}
		cloudComps = new Cloud[cloudObjects.Length];
		for (int j = 0; j < cloudComps.Length; j++)
		{
			cloudComps[j] = cloudObjects[j].cloudGameObject.GetComponent<Cloud>();
		}
	}

	private void Update()
	{
		CloudObject[] array = cloudObjects;
		foreach (CloudObject cloudObject in array)
		{
			cloudObject.sprRend.color = new Color(cloudObject.sprRend.color.r, cloudObject.sprRend.color.g, cloudObject.sprRend.color.b, 1f);
		}
		for (int j = 0; j < cloudComps.Length; j++)
		{
			float alpha = Mathf.Clamp(WeatherManager.instance.interpolatedWeatherType.overCastLevel * cloudObjects[j].maxAlpha - cloudObjects[j].alphaTreshold * cloudObjects[j].maxAlpha, 0f, cloudObjects[j].maxAlpha);
			cloudComps[j].alpha = alpha;
		}
		SetCloudColors();
	}

	private void SetCloudColors()
	{
		Color[] skyColors = DayTimeManager.instance.interpolatedDayState.intState.skyColors;
		cloudMaterial.SetColor("_ColorA", skyColors[0]);
		cloudMaterial.SetColor("_ColorB", skyColors[1]);
		cloudMaterial.SetColor("_ColorC", skyColors[2]);
	}
}
