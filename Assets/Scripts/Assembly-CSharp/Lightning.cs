using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Lightning : MonoBehaviour
{
	public int segmentsCount;

	public float freq;

	public float lac;

	[Range(0f, 1f)]
	public float pers;

	public float strength;

	public int octaves;

	public Vector2 offset;

	public Vector2 linePosStart;

	public Vector2 linePosEnd;

	public float posZ;

	private LineRenderer lineRend;

	private Vector3[] linePoints;

	private void Start()
	{
		lineRend = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		GenerateLine();
	}

	private void GenerateLine()
	{
		lineRend.positionCount = segmentsCount;
		linePoints = new Vector3[segmentsCount];
		Vector2 point = offset;
		float num = 1f / (float)segmentsCount;
		for (int i = 0; i < segmentsCount; i++)
		{
			linePoints[i] = default(Vector3);
			linePoints[i].x = Mathf.Lerp(linePosStart.x, linePosEnd.x, (float)i * num) + Noise.Sum(point, freq, octaves, lac, pers, NoiseType.Ridged) * strength;
			linePoints[i].y = Mathf.Lerp(linePosStart.y, linePosEnd.y, (float)i * num);
			linePoints[i].z = posZ;
			point.x += 0.1f;
		}
		lineRend.SetPositions(linePoints);
	}
}
