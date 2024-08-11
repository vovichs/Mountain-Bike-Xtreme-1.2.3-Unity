using UnityEngine;
using UnityEngine.Rendering;

public class SpriteMesh : MonoBehaviour
{
	private Mesh mesh;

	private MeshFilter meshFilter;

	private MeshRenderer meshRend;

	public Texture2D texture;

	public Vector2[] meshPoints;

	public int terrainPointIndex;

	public Vector2 uvScale = new Vector2(1f, 1f);

	public Vector2 uvOffset = new Vector2(0f, 0f);

	private float lastCamHeight;

	private Vector3 newPos = default(Vector3);

	[HideInInspector]
	public Vector3[] meshNormals { get; set; }

	[HideInInspector]
	public Vector2[] uv { get; set; }

	public bool moveWithCameraY { get; set; }

	private void Awake()
	{
		mesh = new Mesh();
		meshFilter = base.gameObject.AddComponent<MeshFilter>();
		meshRend = base.gameObject.AddComponent<MeshRenderer>();
		meshRend.material = TerrainManager.instance.spriteMeshMaterial;
		meshRend.lightProbeUsage = LightProbeUsage.Off;
		meshRend.reflectionProbeUsage = ReflectionProbeUsage.Off;
		meshRend.shadowCastingMode = ShadowCastingMode.Off;
		meshRend.receiveShadows = false;
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		meshRend.GetPropertyBlock(materialPropertyBlock);
		materialPropertyBlock.SetTexture("_MainTex", texture);
		meshRend.SetPropertyBlock(materialPropertyBlock);
		meshFilter.mesh = mesh;
		base.gameObject.AddComponent<ObjectColor>();
	}

	public void SetPoints(TerrainLayerValue _tlv, ObjectTypeToSpawn _objType, int _instanceIndex)
	{
		base.transform.position = new Vector3(_tlv.terrainLayer.terrainLayerValue.positionX, _tlv.terrainLayer.terrainLayerValue.positionY, _tlv.terrainLayer.terrainLayerValue.depth);
		Vector2[] array = _tlv.terrainLayer.meshPoints;
		int num = (array.Length - 2) / _objType.density;
		num += 2;
		Vector2[] array2 = new Vector2[num];
		int num2 = (int)_objType.spriteMeshPosIndex;
		int num3 = num2;
		int num4 = 0;
		while (num3 < num2 + num)
		{
			array2[num4] = array[num3];
			array2[num4].y -= _objType.meshOffsetY;
			num3++;
			num4++;
		}
		meshPoints = array2;
		_objType.spriteMeshPosIndex += num - 2;
		GenerateGrid(array2, _tlv, _objType);
	}

	public void GenerateGrid(Vector2[] _points, TerrainLayerValue _tlv, ObjectTypeToSpawn _objType)
	{
		int num = _points.Length - 1;
		Vector3[] array = new Vector3[(num + 1) * 2];
		int[] array2 = new int[num * 6];
		uv = new Vector2[array.Length];
		float num2 = 1f / (float)num;
		int i = 0;
		int num3 = 0;
		for (; i < 2; i++)
		{
			int num4 = 0;
			while (num4 <= num)
			{
				if (i == 0)
				{
					array[num3] = new Vector3(_points[num4].x, _points[num4].y, 0f);
				}
				else
				{
					array[num3] = new Vector3(_points[num4].x, _points[num4].y + _objType.meshHeight, 0f);
				}
				uv[num3] = new Vector2((float)num4 * num2 * uvScale.x + uvOffset.x, (float)i * uvScale.y + uvOffset.y);
				num4++;
				num3++;
			}
		}
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		while (num5 < 1)
		{
			int num8 = 0;
			while (num8 < num)
			{
				array2[num6] = num7;
				array2[num6 + 1] = num7 + num + 1;
				array2[num6 + 2] = num7 + 1;
				array2[num6 + 3] = num7 + 1;
				array2[num6 + 4] = num7 + num + 1;
				array2[num6 + 5] = num7 + num + 2;
				num8++;
				num6 += 6;
				num7++;
			}
			num5++;
			num7++;
		}
		mesh.vertices = array;
		mesh.triangles = array2;
		mesh.uv = uv;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}

	private void Update()
	{
		if (moveWithCameraY)
		{
			MoveWithCamera();
		}
	}

	private void MoveWithCamera()
	{
		float y = Camera.main.transform.position.y;
		float num = y - lastCamHeight;
		newPos.x = base.transform.position.x;
		newPos.y = base.transform.position.y + num;
		newPos.z = base.transform.position.z;
		base.transform.position = newPos;
		lastCamHeight = y;
	}
}
