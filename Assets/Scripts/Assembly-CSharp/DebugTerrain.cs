using System;
using UnityEngine;

public class DebugTerrain : MonoBehaviour
{
	public bool isDebugPub;

	public static bool isDebug;

	public float scale;

	public int pointsN;

	public int layerIndex;

	private LineRenderer lineRend;

	private void Awake()
	{
		isDebug = isDebugPub;
		lineRend = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		Vector3[] values = GetValues();
		lineRend.SetPositions(values);
	}

	private Vector3[] GetValues()
	{
		int num = 7;
		int num2 = 123564879;
		int num3 = 124124523;
		float num4 = 0.5f;
		int num5 = 1;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		float num10 = 0f;
		float num11 = 0f;
		int[] array = new int[8] { 2, 1, 12, 0, 1, 0, 2, 0 };
		TerrainLayerValue terrainLayerValue = TerrainManager.instance.terrainLayerValues[layerIndex];
		lineRend.positionCount = pointsN;
		Vector3[] array2 = new Vector3[pointsN];
		int num12 = 0;
		for (int i = 0; i < array2.Length; i++)
		{
			if (i == terrainLayerValue.pointsN / 2 + num12 || i == 0)
			{
				num5 = num2;
				num5 &= num;
				num5 = array[num5];
				num5 &= terrainLayerValue.noisePresets.Length - 1;
				num2++;
				num6 = num2;
				num6 &= num;
				num6 = array[num6];
				num6 &= terrainLayerValue.noisePresets.Length - 1;
				num8 = num3;
				num8 &= terrainLayerValue.curves.Length - 1;
				num8 = terrainLayerValue.indexTable[num8];
				num3++;
				num9 = num3;
				num9 &= terrainLayerValue.curves.Length - 1;
				num9 = terrainLayerValue.indexTable[num9];
				if (i == terrainLayerValue.pointsN / 2 + num12)
				{
					num12 += terrainLayerValue.pointsN / 2;
					num10 -= 0.1f;
				}
			}
			float num13 = (float)(i - num12) / (float)(terrainLayerValue.pointsN / 2);
			float num14 = num13;
			num14 = (num14 - num4) / (1f - num4);
			if (num14 < 0f)
			{
				num14 = 0f;
			}
			float num15 = (1f - Mathf.Cos(num14 * (float)Math.PI)) / 2f;
			float num16 = Noise.Sum(new Vector3(num10 + terrainLayerValue.noisePresets[num5].xOffset, terrainLayerValue.noisePresets[num5].noiseZpos, 0f), terrainLayerValue.noisePresets[num5].frequency, terrainLayerValue.noisePresets[num5].octaves, terrainLayerValue.noisePresets[num5].lacunarity, terrainLayerValue.noisePresets[num5].persistance, terrainLayerValue.noisePresets[num5].noiseType);
			num16 += terrainLayerValue.curves[num8].curve.Evaluate(num13);
			num16 *= terrainLayerValue.noisePresets[num5].coefficient;
			float num17 = Noise.Sum(new Vector3(num10 + terrainLayerValue.noisePresets[num6].xOffset, terrainLayerValue.noisePresets[num6].noiseZpos, 0f), terrainLayerValue.noisePresets[num6].frequency, terrainLayerValue.noisePresets[num6].octaves, terrainLayerValue.noisePresets[num6].lacunarity, terrainLayerValue.noisePresets[num6].persistance, terrainLayerValue.noisePresets[num6].noiseType);
			num17 += terrainLayerValue.curves[num9].curve.Evaluate(num13);
			num17 *= terrainLayerValue.noisePresets[num6].coefficient;
			num16 = num16 * (1f - num15) + num15 * num17;
			float num18 = (1f - num15) * terrainLayerValue.noisePresets[num5].downStep + num15 * terrainLayerValue.noisePresets[num6].downStep;
			num16 -= num11;
			num11 += num18;
			array2[i].y = (num16 + terrainLayerValue.positionY) * scale;
			float num19 = terrainLayerValue.length * ((float)pointsN / (float)terrainLayerValue.pointsN);
			array2[i].x = terrainLayerValue.offsetX + (float)i * (num19 / (float)(pointsN - 1)) * scale;
			array2[i].z = terrainLayerValue.depth;
			num10 += 0.1f;
		}
		num2 = 123564879;
		num3 = 124124523;
		return array2;
	}
}
