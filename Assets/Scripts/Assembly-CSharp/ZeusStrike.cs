using System;
using UnityEngine;

public class ZeusStrike : MonoBehaviour
{
	public int levels;

	public int branchesOnLevel;

	public Vector3 startPos;

	public Vector3 endPos;

	public float branchGenerationDelay;

	public GameObject zeusBranchPrefab;

	public ZeusLevel[] zeusLevels;

	public int seed;

	public int totalBranchesCount;

	public AudioClip[] thunderAudio;

	private static ZeusStrike _instance;

	public float energy;

	private float randomEnergyTreshold;

	public ZeusBranch[] zeusBranches { get; set; }

	public System.Random sysRand { get; set; }

	public static ZeusStrike instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType<ZeusStrike>();
			}
			return _instance;
		}
	}

	private void Awake()
	{
		sysRand = new System.Random(seed);
	}

	private void Start()
	{
		totalBranchesCount = 1;
		for (int i = 0; i < zeusLevels.Length; i++)
		{
			totalBranchesCount *= zeusLevels[i].branchesCount;
		}
		totalBranchesCount++;
		ObjectPool.instance.CreatePool(zeusBranchPrefab, totalBranchesCount);
		randomEnergyTreshold = UnityEngine.Random.Range(4f, 10f);
	}

	private void Update()
	{
		if (WeatherManager.instance.interpolatedWeatherType.rainLevel > 0.5f)
		{
			energy += WeatherManager.instance.interpolatedWeatherType.rainLevel * Time.deltaTime;
			if (energy > randomEnergyTreshold)
			{
				randomEnergyTreshold = UnityEngine.Random.Range(4f, 30f);
				GenerateStrike();
				energy = 0f;
			}
		}
	}

	private void GenerateStrike()
	{
		GameObject go = ObjectPool.instance.ReuseObject(zeusBranchPrefab).go;
		ZeusBranch component = go.GetComponent<ZeusBranch>();
		startPos.x = Camera.main.transform.position.x + UnityEngine.Random.Range(-1000f, 1000f);
		endPos.x = startPos.x + UnityEngine.Random.Range(-500f, 500f);
		component.GenerateLine(0, startPos, endPos);
	}

	private void PlaySound()
	{
	}
}
