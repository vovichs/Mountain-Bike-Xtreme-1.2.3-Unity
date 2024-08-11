using System;
using UnityEngine;

public class TerrainNoiseGenerator
{
	private int presetHashMask = 7;

	private int presetNextIndex;

	private float startT = 0.5f;

	private int presetIndex = 1;

	private int aheadIndex;

	private int curveIndex;

	private int curveIndexAhead;

	private NoisePreset[] noisePresets;

	private static TerrainNoiseGenerator _instance;

	private int[] presetIndexesTable = new int[8] { 2, 1, 12, 0, 1, 0, 2, 0 };

	private bool firstTime = true;

	public static TerrainNoiseGenerator instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new TerrainNoiseGenerator();
			}
			return _instance;
		}
	}

	public int DefineCurveIndex(TerrainLayer _terrainLayer)
	{
		float num = (float)Utility.intance.rand.NextDouble();
		Array.Sort(_terrainLayer.terrainLayerValue.curves);
		float num2 = 0f;
		for (int i = 0; i < _terrainLayer.terrainLayerValue.curves.Length; i++)
		{
			num2 += _terrainLayer.terrainLayerValue.curves[i].propability;
			if (num2 >= num)
			{
				return i;
			}
		}
		return 0;
	}

	public Vector2[] GeneratePoints(TerrainLayer terrainLayer, bool reset)
	{
		Vector2[] array = new Vector2[terrainLayer.terrainLayerValue.pointsN / 2];
		presetIndex = terrainLayer.terrainLayerValue.noiseNextPresetIndex;
		presetIndex &= presetHashMask;
		presetIndex = presetIndexesTable[presetIndex];
		presetIndex &= terrainLayer.terrainLayerValue.noisePresets.Length - 1;
		terrainLayer.terrainLayerValue.noiseNextPresetIndex++;
		aheadIndex = terrainLayer.terrainLayerValue.noiseNextPresetIndex;
		aheadIndex &= presetHashMask;
		aheadIndex = presetIndexesTable[aheadIndex];
		aheadIndex &= terrainLayer.terrainLayerValue.noisePresets.Length - 1;
		curveIndex = terrainLayer.terrainLayerValue.curveNextPresetIndex;
		curveIndex &= terrainLayer.terrainLayerValue.indexTable.Length - 1;
		curveIndex = terrainLayer.terrainLayerValue.indexTable[curveIndex];
		terrainLayer.terrainLayerValue.curveNextPresetIndex++;
		curveIndexAhead = terrainLayer.terrainLayerValue.curveNextPresetIndex;
		curveIndexAhead &= terrainLayer.terrainLayerValue.indexTable.Length - 1;
		curveIndexAhead = terrainLayer.terrainLayerValue.indexTable[curveIndexAhead];
		for (int i = 0; i < array.Length; i++)
		{
			float num = (float)i / (float)(array.Length - 1);
			float num2 = num;
			num2 = (num2 - startT) / (1f - startT);
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			float num3 = (1f - Mathf.Cos(num2 * (float)Math.PI)) / 2f;
			float num4 = Noise.Sum(new Vector3(terrainLayer.noisePoint + terrainLayer.terrainLayerValue.noisePresets[presetIndex].xOffset, terrainLayer.terrainLayerValue.noisePresets[presetIndex].noiseZpos, 0f), terrainLayer.terrainLayerValue.noisePresets[presetIndex].frequency, terrainLayer.terrainLayerValue.noisePresets[presetIndex].octaves, terrainLayer.terrainLayerValue.noisePresets[presetIndex].lacunarity, terrainLayer.terrainLayerValue.noisePresets[presetIndex].persistance, terrainLayer.terrainLayerValue.noisePresets[presetIndex].noiseType);
			num4 += terrainLayer.terrainLayerValue.curves[curveIndex].curve.Evaluate(num);
			num4 *= terrainLayer.terrainLayerValue.noisePresets[presetIndex].coefficient;
			float num5 = Noise.Sum(new Vector3(terrainLayer.noisePoint + terrainLayer.terrainLayerValue.noisePresets[presetIndex].xOffset, terrainLayer.terrainLayerValue.noisePresets[aheadIndex].noiseZpos, 0f), terrainLayer.terrainLayerValue.noisePresets[aheadIndex].frequency, terrainLayer.terrainLayerValue.noisePresets[aheadIndex].octaves, terrainLayer.terrainLayerValue.noisePresets[aheadIndex].lacunarity, terrainLayer.terrainLayerValue.noisePresets[aheadIndex].persistance, terrainLayer.terrainLayerValue.noisePresets[aheadIndex].noiseType);
			num5 += terrainLayer.terrainLayerValue.curves[curveIndexAhead].curve.Evaluate(num);
			num5 *= terrainLayer.terrainLayerValue.noisePresets[aheadIndex].coefficient;
			num4 = (1f - num3) * num4 + num3 * num5;
			float num6 = (1f - num3) * terrainLayer.terrainLayerValue.noisePresets[presetIndex].downStep + num3 * terrainLayer.terrainLayerValue.noisePresets[aheadIndex].downStep;
			num4 -= terrainLayer.downOffset;
			terrainLayer.downOffset += num6;
			array[i].y = num4;
			array[i].x = 0f;
			terrainLayer.noisePoint += 0.1f;
		}
		terrainLayer.noisePoint -= 0.1f;
		if (reset)
		{
			terrainLayer.noisePoint = terrainLayer.terrainLayerValue.noisePresets[presetIndex].xOffset;
			terrainLayer.downOffset = 0f;
		}
		return array;
	}
}
