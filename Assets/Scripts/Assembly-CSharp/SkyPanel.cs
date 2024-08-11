using UnityEngine;

public class SkyPanel : MonoBehaviour
{
	private MeshFilter meshFilter;

	private Mesh mesh;

	private Vector3[] vertices;

	private int[] triangles;

	private Color[] meshColors;

	private int skyColorsCount = 3;

	public float[] skyVerticesHeights;

	private void Start()
	{
		base.gameObject.GetComponent<MeshRenderer>().enabled = true;
		meshFilter = GetComponent<MeshFilter>();
		mesh = new Mesh();
		vertices = new Vector3[2 * skyColorsCount];
		int i = 0;
		int num = 0;
		for (; i < skyColorsCount; i++)
		{
			int num2 = 0;
			while (num2 < 2)
			{
				vertices[num] = new Vector3(num2, (float)i / (float)skyColorsCount, 0f);
				num2++;
				num++;
			}
		}
		triangles = new int[(skyColorsCount - 1) * 6];
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		while (num3 < skyColorsCount - 1)
		{
			triangles[num4] = num5;
			triangles[num4 + 1] = 2 + num5;
			triangles[num4 + 2] = 1 + num5;
			triangles[num4 + 3] = 1 + num5;
			triangles[num4 + 4] = 2 + num5;
			triangles[num4 + 5] = 3 + num5;
			num3++;
			num4 += 6;
			num5 += 2;
		}
		meshColors = new Color[vertices.Length];
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		meshFilter.mesh = mesh;
	}

	private void SetMiddleHeight()
	{
		int i = 0;
		int num = 0;
		for (; i < skyVerticesHeights.Length; i++)
		{
			int num2 = 0;
			while (num2 < 2)
			{
				vertices[num].y = skyVerticesHeights[i];
				num2++;
				num++;
			}
		}
		mesh.vertices = vertices;
	}

	private void SetSkyColors()
	{
		Color[] skyColors = DayTimeManager.instance.interpolatedDayState.intState.skyColors;
		int i = 0;
		int num = 0;
		for (; i < skyColorsCount; i++)
		{
			int num2 = 0;
			while (num2 < 2)
			{
				meshColors[num] = skyColors[skyColors.Length - 1 - i];
				num2++;
				num++;
			}
		}
		mesh.colors = meshColors;
	}

	private void Update()
	{
		SetSkyColors();
	}
}
