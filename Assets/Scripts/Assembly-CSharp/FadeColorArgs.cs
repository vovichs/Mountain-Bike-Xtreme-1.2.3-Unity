using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class FadeColorArgs : AnimationArgs
{
	public Graphic elem;

	public Color colorA;

	public Color colorB;

	public float duration;
}
