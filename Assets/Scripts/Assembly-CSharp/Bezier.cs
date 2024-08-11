using UnityEngine;

public class Bezier : MonoBehaviour
{
	public Vector3[] samplePoints;

	public Vector3[] curvePoins;

	public int pointsN;

	public Vector3 GetPoint(float _t)
	{
		return base.transform.TransformPoint(Vector3.Lerp(Vector3.Lerp(samplePoints[0], samplePoints[1], _t), Vector3.Lerp(samplePoints[1], samplePoints[2], _t), _t));
	}
}
