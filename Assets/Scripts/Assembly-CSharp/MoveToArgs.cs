using System;
using UnityEngine;

[Serializable]
public class MoveToArgs : AnimationArgs
{
	public RectTransform elem;

	public Vector3 posA;

	public Vector3 posB;

	public float duration;
}
