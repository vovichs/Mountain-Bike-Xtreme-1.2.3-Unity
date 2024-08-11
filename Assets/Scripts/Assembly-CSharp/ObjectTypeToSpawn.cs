using System;
using UnityEngine;

[Serializable]
public class ObjectTypeToSpawn
{
	public ObjectToSpawn[] objectToSpawn;

	public int density;

	public Vector2 zPosRange;

	public Vector2 scaleAddRange;

	public Vector2 angleAddRange;

	public bool randomFlip;

	public AnimationCurve[] distributionCurve;

	[HideInInspector]
	public int distributionCurveIndex;

	public bool spawn;

	[HideInInspector]
	public bool reset = true;

	[HideInInspector]
	public float currentTrailPosIndex;

	[HideInInspector]
	public float spriteMeshPosIndex;

	[HideInInspector]
	public int spriteMeshInstanceIndex;

	public float meshHeight;

	public float meshOffsetY;

	public float noResetPosIndex { get; set; }
}
