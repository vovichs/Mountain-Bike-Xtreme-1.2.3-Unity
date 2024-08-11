using System;
using System.Collections;
using UnityEngine;

public class ZeusBranch : MonoBehaviour
{
	public int pointsCount;

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

	public float lineWidth;

	private LineRenderer lineRend;

	private Vector3[] linePoints;

	public int seed;

	public int level;

	[Range(0f, 1f)]
	public float alpha;

	private Material material;

	private Material glowMat;

	private MaterialPropertyBlock propBlock;

	private MaterialPropertyBlock glowPropBlock;

	private Renderer rend;

	public MeshRenderer glowMeshRend;

	public Transform glowTransform;

	private Mesh mesh;

	private Vector3[] meshPoints;

	private int[] triangles;

	private Vector2[] uv;

	private Vector3[] normals;

	private Animator anim;

	private System.Random sysRand;

	private AudioSource audioSource;

	private float audioVolume = 1f;

	private float startOffsetX;

	public void Awake()
	{
		lineRend = GetComponent<LineRenderer>();
		material = GetComponent<MeshRenderer>().material;
		rend = GetComponent<MeshRenderer>();
		propBlock = new MaterialPropertyBlock();
		rend.GetPropertyBlock(propBlock);
		mesh = new Mesh();
		anim = GetComponent<Animator>();
		glowPropBlock = new MaterialPropertyBlock();
		glowMeshRend.GetPropertyBlock(glowPropBlock);
	}

	private void Start()
	{
		sysRand = ZeusStrike.instance.sysRand;
	}

	private void Update()
	{
		propBlock.SetFloat("_Alpha", alpha);
		rend.SetPropertyBlock(propBlock);
		glowPropBlock.SetFloat("_Alpha", alpha);
		glowMeshRend.SetPropertyBlock(glowPropBlock);
	}

	public void GenerateLine(int _level, Vector3 _startPos, Vector3 _endPos)
	{
		if (level == 0)
		{
			if (audioSource == null)
			{
				audioSource = base.gameObject.AddComponent<AudioSource>();
				audioSource.volume = audioVolume * AmbienceAudioManager.instance.effectsVolume;
			}
			audioSource.clip = ZeusStrike.instance.thunderAudio[UnityEngine.Random.Range(0, ZeusStrike.instance.thunderAudio.Length - 1)];
			audioSource.PlayDelayed(1.2f);
		}
		glowTransform.position = new Vector3(_startPos.x, _startPos.y, posZ);
		anim.Play("zeusBranch");
		strength = ZeusStrike.instance.zeusBranchPrefab.GetComponent<ZeusBranch>().strength;
		lineWidth = ZeusStrike.instance.zeusBranchPrefab.GetComponent<ZeusBranch>().lineWidth;
		level = _level;
		lineWidth /= Mathf.Pow(2f, _level);
		lineRend.startWidth = lineWidth;
		lineRend.endWidth = lineWidth;
		lineRend.positionCount = pointsCount;
		linePoints = new Vector3[pointsCount];
		offset = new Vector2((float)ZeusStrike.instance.sysRand.NextDouble() * 1000f, (float)ZeusStrike.instance.sysRand.NextDouble() * 1000f);
		Vector2 point = offset;
		float num = 1f / (float)pointsCount;
		for (int i = 0; i < pointsCount; i++)
		{
			linePoints[i] = default(Vector3);
			linePoints[i].x = Mathf.Lerp(_startPos.x, _endPos.x, (float)i * num);
			float num2 = Noise.Sum(point, freq, octaves, lac, pers, NoiseType.Ridged) * strength;
			linePoints[i].x += num2;
			if (i == 0)
			{
				startOffsetX = num2;
			}
			linePoints[i].x -= startOffsetX;
			linePoints[i].y = Mathf.Lerp(_startPos.y, _endPos.y, (float)i * num);
			linePoints[i].z = posZ;
			point.x += 0.1f;
		}
		lineRend.SetPositions(linePoints);
		GenerateMesh();
		linePosStart = _startPos;
		linePosEnd = _endPos;
		if (_level != ZeusStrike.instance.zeusLevels.Length)
		{
			StartCoroutine(InvokeMethod(ZeusStrike.instance.branchGenerationDelay + UnityEngine.Random.Range(0f, 0.01f)));
		}
	}

	private void DefineBranches()
	{
		for (int i = 0; i < ZeusStrike.instance.zeusLevels[level].branchesCount; i++)
		{
			GameObject go = ObjectPool.instance.ReuseObject(ZeusStrike.instance.zeusBranchPrefab).go;
			ZeusBranch component = go.GetComponent<ZeusBranch>();
			component.strength /= Mathf.Pow(4f, level + 1);
			System.Random random = ZeusStrike.instance.sysRand;
			int num = random.Next(0, linePoints.Length - 1);
			Vector3 vector = linePoints[num];
			Vector3 endPos = vector + new Vector3((float)random.NextDouble() * 400f - 200f, (float)random.NextDouble() * 0f - 200f);
			component.GenerateLine(level + 1, vector, endPos);
		}
	}

	private IEnumerator InvokeMethod(float _time)
	{
		yield return new WaitForSeconds(_time);
		DefineBranches();
	}

	private void GenerateMesh()
	{
		meshPoints = new Vector3[pointsCount * 2];
		triangles = new int[(pointsCount - 1) * 6];
		uv = new Vector2[meshPoints.Length];
		normals = new Vector3[pointsCount];
		int i = 0;
		int num = 0;
		for (; i < pointsCount; i++)
		{
			int num2 = 0;
			while (num2 < 2)
			{
				meshPoints[num] = linePoints[i];
				float num3 = Mathf.Lerp(lineWidth, lineWidth / 2f, (float)i * (1f / (float)pointsCount));
				meshPoints[num].x += (0f - num3) / 2f + num3 * (float)num2;
				uv[num] = new Vector2(num2, (float)i * (1f / (float)pointsCount));
				num2++;
				num++;
			}
		}
		normals[0] = Vector3.zero;
		for (int j = 1; j < pointsCount; j++)
		{
			normals[j] = (meshPoints[j * 2] - meshPoints[j * 2 - 2]).normalized;
		}
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		while (num4 < pointsCount - 1)
		{
			triangles[num5] = num6;
			triangles[num5 + 1] = num6 + 1;
			triangles[num5 + 2] = num6 + 2;
			triangles[num5 + 3] = num6 + 2;
			triangles[num5 + 4] = num6 + 1;
			triangles[num5 + 5] = num6 + 2 + 1;
			num4++;
			num5 += 6;
			num6 += 2;
		}
		mesh.vertices = meshPoints;
		mesh.triangles = triangles;
		mesh.uv = uv;
		GetComponent<MeshFilter>().mesh = mesh;
	}
}
