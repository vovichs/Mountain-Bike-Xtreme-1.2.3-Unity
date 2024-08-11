using UnityEngine;
using UnityEngine.Rendering;

public class TerrainLayer : MonoBehaviour
{
	[HideInInspector]
	public EdgeCollider2D edgeCollider;

	[HideInInspector]
	public Vector2[] terrainPoints_A;

	[HideInInspector]
	public Vector2[] terrainPoints_B;

	private Mesh mesh;

	private MeshFilter meshFilter;

	private MeshRenderer meshRend;

	private Vector2[] uv;

	private bool reset;

	private Vector2[] edgeColPoints;

	private Vector3 layerPosition;

	private float lastCamHeight;

	[HideInInspector]
	public float noisePoint { get; set; }

	[HideInInspector]
	public float downOffset { get; set; }

	[HideInInspector]
	public Vector2[] meshPoints { get; set; }

	[HideInInspector]
	public Vector3[] meshNormals { get; set; }

	[HideInInspector]
	public TerrainLayerValue terrainLayerValue { get; set; }

	private void Awake()
	{
		mesh = new Mesh();
		meshFilter = base.gameObject.AddComponent<MeshFilter>();
		meshRend = base.gameObject.AddComponent<MeshRenderer>();
		meshRend.material = TerrainManager.instance.terrainMaterial;
		meshRend.lightProbeUsage = LightProbeUsage.Off;
		meshRend.reflectionProbeUsage = ReflectionProbeUsage.Off;
		meshRend.shadowCastingMode = ShadowCastingMode.Off;
		meshRend.receiveShadows = false;
		base.gameObject.AddComponent<ObjectColor>();
		lastCamHeight = Camera.main.transform.position.y;
	}

	private void Start()
	{
		terrainLayerValue.indexTable = new int[terrainLayerValue.curves.Length * 5];
		for (int i = 0; i < terrainLayerValue.curves.Length * 5; i++)
		{
			terrainLayerValue.indexTable[i] = Random.Range(0, terrainLayerValue.curves.Length);
		}
		terrainLayerValue.noiseNextPresetIndex = 123564879;
		layerPosition = base.transform.position;
		if (!reset)
		{
			base.transform.position = new Vector3(base.transform.position.x + terrainLayerValue.offsetX, terrainLayerValue.positionY, terrainLayerValue.depth);
		}
		meshPoints = new Vector2[terrainLayerValue.pointsN];
		if (terrainLayerValue.hasCollider)
		{
			edgeColPoints = new Vector2[terrainLayerValue.pointsN];
			edgeCollider = base.gameObject.AddComponent<EdgeCollider2D>();
			edgeCollider.sharedMaterial = Resources.Load("PhysicMaterials/GroundMaterial") as PhysicsMaterial2D;
			edgeCollider.edgeRadius = 0.1f;
		}
		if (terrainLayerValue.updateRealTime)
		{
			reset = true;
		}
		if (terrainLayerValue != TerrainManager.instance.terrainLayerValues[0] && terrainLayerValue != TerrainManager.instance.terrainLayerValues[1])
		{
			float num = 0f;
			for (int j = 0; j < TerrainManager.instance.terrainLayerValues[0].noisePresets.Length; j++)
			{
				num += TerrainManager.instance.terrainLayerValues[0].noisePresets[j].downStep;
			}
			num /= (float)TerrainManager.instance.terrainLayerValues[0].noisePresets.Length;
			for (int k = 0; k < terrainLayerValue.noisePresets.Length; k++)
			{
				terrainLayerValue.noisePresets[k].downStep = Mathf.Lerp(0.9f * (terrainLayerValue.length / TerrainManager.instance.terrainLayerValues[0].length) * num, terrainLayerValue.length / TerrainManager.instance.terrainLayerValues[0].length * num / 1.7f, terrainLayerValue.depth / 3500f);
				terrainLayerValue.noisePresets[k].downStep *= (float)TerrainManager.instance.terrainLayerValues[0].pointsN / (float)terrainLayerValue.pointsN;
			}
		}
		terrainPoints_A = TerrainNoiseGenerator.instance.GeneratePoints(this, false);
		terrainPoints_B = TerrainNoiseGenerator.instance.GeneratePoints(this, reset);
		terrainLayerValue.positionX = base.transform.position.x;
		CalculatePoints();
		GenerateGrid(meshPoints);
	}

	private void FixedUpdate()
	{
		if (reset)
		{
			base.transform.position = new Vector3(layerPosition.x + terrainLayerValue.offsetX, layerPosition.y + terrainLayerValue.positionY, layerPosition.z + terrainLayerValue.depth);
		}
		if (!reset)
		{
			CameraPosCheck();
		}
		else
		{
			GenerateTerrain();
			ObjectPlacer.ResetIndex(terrainLayerValue);
		}
		if (terrainLayerValue.moveWithCameraY)
		{
			MoveWithCamera();
		}
	}

	private void CameraPosCheck()
	{
		if (TerrainManager.instance.trackTarget.position.x > base.transform.position.x + terrainLayerValue.length / 1.3f)
		{
			GenerateTerrain();
			ObjectPlacer.ResetIndex(terrainLayerValue);
		}
	}

	public void GenerateTerrain()
	{
		if (!reset)
		{
			base.transform.position += new Vector3(terrainLayerValue.length / 2f, 0f, 0f);
		}
		terrainLayerValue.positionX = base.transform.position.x;
		if (!reset)
		{
			terrainPoints_A = (Vector2[])terrainPoints_B.Clone();
		}
		else
		{
			terrainPoints_A = TerrainNoiseGenerator.instance.GeneratePoints(this, false);
		}
		terrainPoints_B = TerrainNoiseGenerator.instance.GeneratePoints(this, reset);
		CalculatePoints();
		GenerateGrid(meshPoints);
	}

	public void CalculatePoints()
	{
		for (int i = 0; i < terrainLayerValue.pointsN / 2; i++)
		{
			terrainPoints_A[i].x = (float)i * (terrainLayerValue.length / (float)(terrainLayerValue.pointsN - 1));
			meshPoints[i].x = terrainPoints_A[i].x;
			meshPoints[i].y = terrainPoints_A[i].y;
			if (terrainLayerValue.hasCollider)
			{
				edgeColPoints[i].x = terrainPoints_A[i].x;
				edgeColPoints[i].y = terrainPoints_A[i].y - edgeCollider.edgeRadius - 0.05f;
			}
		}
		for (int j = 0; j < terrainLayerValue.pointsN / 2; j++)
		{
			terrainPoints_B[j].x = (float)(j + terrainLayerValue.pointsN / 2) * (terrainLayerValue.length / (float)(terrainLayerValue.pointsN - 1));
			meshPoints[j + terrainLayerValue.pointsN / 2].x = terrainPoints_B[j].x;
			meshPoints[j + terrainLayerValue.pointsN / 2].y = terrainPoints_B[j].y;
			if (terrainLayerValue.hasCollider)
			{
				edgeColPoints[j + terrainLayerValue.pointsN / 2].x = terrainPoints_B[j].x;
				edgeColPoints[j + terrainLayerValue.pointsN / 2].y = terrainPoints_B[j].y - edgeCollider.edgeRadius - 0.05f;
			}
		}
		if (terrainLayerValue.hasCollider)
		{
			edgeCollider.points = edgeColPoints;
		}
	}

	public void GenerateGrid(Vector2[] _points)
	{
		int num = _points.Length;
		int[] array = new int[(num - 1) * 6];
		float num2 = 1f / (float)num;
		Vector3[] array2 = new Vector3[num * 2];
		uv = new Vector2[array2.Length];
		int i = 0;
		int num3 = 0;
		for (; i < 2; i++)
		{
			int num4 = 0;
			while (num4 < num)
			{
				array2[num3] = new Vector3(_points[num4].x, (float)i * _points[num4].y, 0f);
				if (i == 0)
				{
					array2[num3].y -= 200f + downOffset;
				}
				uv[num3] = new Vector2((float)num4 * num2, (float)i + _points[num4].y * num2);
				num4++;
				num3++;
			}
		}
		int num5 = 0;
		int num6 = 0;
		while (num5 < num - 1)
		{
			array[num6] = num5;
			array[num6 + 1] = num5 + num;
			array[num6 + 2] = num5 + 1;
			array[num6 + 3] = num5 + 1;
			array[num6 + 4] = num5 + num;
			array[num6 + 5] = num5 + num + 1;
			num5++;
			num6 += 6;
		}
		mesh.vertices = array2;
		mesh.triangles = array;
		mesh.uv = uv;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		meshFilter.mesh = mesh;
		meshNormals = mesh.normals;
	}

	private void MoveWithCamera()
	{
		float y = Camera.main.transform.position.y;
		float num = y - lastCamHeight;
		Vector3 position = new Vector3(base.transform.position.x, base.transform.position.y + num, base.transform.position.z);
		base.transform.position = position;
		lastCamHeight = y;
	}
}
