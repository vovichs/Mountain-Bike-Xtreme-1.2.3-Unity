using System;
using UnityEngine;

[Serializable]
public class CloudObject
{
	public GameObject cloudGameObject;

	public float alphaTreshold;

	public float maxAlpha;

	public SpriteRenderer sprRend { get; set; }

	public float alpha { get; set; }
}
