using UnityEngine;

public class TerrainManager : MonoBehaviour
{
	public TerrainLayerValue[] terrainLayerValues;

	private static TerrainManager _instance;

	public Transform trackTarget;

	public Material terrainMaterial;

	public Material spriteMeshMaterial;

	public static TerrainManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindObjectOfType<TerrainManager>();
			}
			return _instance;
		}
	}

	private void Awake()
	{
		if (!DebugTerrain.isDebug)
		{
			for (int i = 0; i < terrainLayerValues.Length; i++)
			{
				GameObject gameObject = new GameObject();
				gameObject.transform.parent = base.transform;
				gameObject.name = "MainTerrainLayer";
				gameObject.layer = 12;
				TerrainLayer terrainLayer = gameObject.AddComponent<TerrainLayer>();
				terrainLayerValues[i].terrainLayer = terrainLayer;
				terrainLayer.terrainLayerValue = terrainLayerValues[i];
			}
		}
	}

	private void Start()
	{
		float noiseZpos = Random.Range(0, 10000);
		for (int i = 0; i < terrainLayerValues[0].noisePresets.Length; i++)
		{
			terrainLayerValues[0].noisePresets[i].noiseZpos = noiseZpos;
		}
	}
}
