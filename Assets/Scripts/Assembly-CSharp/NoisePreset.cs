using System;
using UnityEngine;

[Serializable]
public class NoisePreset
{
	[Range(0f, 1f)]
	public float frequency;

	public int octaves;

	[Range(0f, 7f)]
	public float lacunarity;

	[Range(0f, 1f)]
	public float persistance;

	public float coefficient;

	public float xOffset;

	public float noiseZpos;

	public float downStep;

	public NoiseType noiseType;
}
