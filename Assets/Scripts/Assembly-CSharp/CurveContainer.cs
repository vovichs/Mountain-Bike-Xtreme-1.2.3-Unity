using System;
using UnityEngine;

[Serializable]
public class CurveContainer : IComparable
{
	public AnimationCurve curve;

	public float distanceTheshold;

	public float propability;

	private CurveContainer()
	{
		curve = new AnimationCurve();
		curve.AddKey(0f, 1f);
		curve.AddKey(0f, 1f);
	}

	public int CompareTo(object obj)
	{
		CurveContainer curveContainer = obj as CurveContainer;
		return propability.CompareTo(curveContainer.propability);
	}
}
