using UnityEngine;

public class ZeusManager : MonoBehaviour
{
	public ZeusObject[] zeusObjects;

	public float energy;

	private float randomEnergyTreshold;

	private void Start()
	{
		ZeusObject[] array = zeusObjects;
		foreach (ZeusObject zeusObject in array)
		{
			zeusObject.anim = zeusObject.zeusGameObject.GetComponent<Animator>();
		}
	}

	public void PerformStrike()
	{
		int num = Random.Range(0, zeusObjects.Length - 1);
		Vector3 position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Random.Range(1000f, 2000f));
		zeusObjects[num].zeusGameObject.transform.position = position;
		zeusObjects[num].anim.Play("zeus");
		randomEnergyTreshold = Random.Range(4f, 10f);
	}

	private void Update()
	{
		if (WeatherManager.instance.interpolatedWeatherType.rainLevel > 0.5f)
		{
			energy += WeatherManager.instance.interpolatedWeatherType.rainLevel * Time.deltaTime;
			if (energy > randomEnergyTreshold)
			{
				PerformStrike();
				energy = 0f;
			}
		}
	}
}
