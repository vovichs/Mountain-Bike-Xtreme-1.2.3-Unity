using System;
using UnityEngine;

[Serializable]
public class TerrainLayerValue
{
	public float length;

	public int pointsN;

	public float depth;

	public float offsetX;

	public float positionY;

	public bool hasCollider;

	public LayerType layerType;

	public ObjectTypeToSpawn[] terrainObjects;

	public NoisePreset[] noisePresets;

	public CurveContainer[] curves;

	public bool updateRealTime;

	public int[] indexTable;

	[HideInInspector]
	public TerrainLayer terrainLayer;

	[HideInInspector]
	public float positionX;

	[HideInInspector]
	public int noiseNextPresetIndex = 123564879;

	[HideInInspector]
	public int curveNextPresetIndex = 124124523;

	public bool moveWithCameraY { get; set; }

	public TerrainLayerValue()
	{
		noiseNextPresetIndex = 123564879;
		curveNextPresetIndex = 124124523;
		curves = new CurveContainer[1];
		indexTable = new int[1];
	}
}
