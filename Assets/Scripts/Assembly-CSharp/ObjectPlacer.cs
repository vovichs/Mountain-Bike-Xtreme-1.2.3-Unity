using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
	private ObjectToSpawn[] objToSpawn;

	public Transform target;

	private float timer;

	private Vector3 newPos = default(Vector3);

	private Vector3 newScale = default(Vector3);

	private void Start()
	{
		if (DebugTerrain.isDebug)
		{
			return;
		}
		TerrainLayerValue[] terrainLayerValues = TerrainManager.instance.terrainLayerValues;
		foreach (TerrainLayerValue terrainLayerValue in terrainLayerValues)
		{
			ObjectTypeToSpawn[] terrainObjects = terrainLayerValue.terrainObjects;
			foreach (ObjectTypeToSpawn objectTypeToSpawn in terrainObjects)
			{
				ObjectPool.instance.CreatePool(objectTypeToSpawn.objectToSpawn, objectTypeToSpawn.density, terrainLayerValue);
				objectTypeToSpawn.currentTrailPosIndex = 0f;
				objectTypeToSpawn.spriteMeshInstanceIndex = 0;
				objectTypeToSpawn.spriteMeshPosIndex = 0f;
			}
		}
	}

	private void Update()
	{
		if (!DebugTerrain.isDebug)
		{
			SpawnObjects();
		}
	}

	public void SpawnObjects()
	{
		TerrainLayerValue[] terrainLayerValues = TerrainManager.instance.terrainLayerValues;
		foreach (TerrainLayerValue terrainLayerValue in terrainLayerValues)
		{
			ObjectTypeToSpawn[] terrainObjects = terrainLayerValue.terrainObjects;
			foreach (ObjectTypeToSpawn objectTypeToSpawn in terrainObjects)
			{
				for (int k = 0; k < 1; k++)
				{
					int num = (int)objectTypeToSpawn.currentTrailPosIndex;
					Vector2[] meshPoints = terrainLayerValue.terrainLayer.meshPoints;
					if (num >= meshPoints.Length || !objectTypeToSpawn.spawn)
					{
						continue;
					}
					float num2 = objectTypeToSpawn.distributionCurve[objectTypeToSpawn.distributionCurveIndex].Evaluate(objectTypeToSpawn.noResetPosIndex / (float)meshPoints.Length);
					ObjectInstance objectInstance = ObjectPool.instance.ReuseObject(objectTypeToSpawn.objectToSpawn, terrainLayerValue);
					GameObject go = objectInstance.go;
					TerrainObject terrainObject = objectInstance.terrainObject;
					if (num2 < 1f)
					{
						go.SetActive(false);
					}
					SpriteMesh spriteMesh = objectInstance.spriteMesh;
					float num3 = (float)meshPoints.Length / (float)objectTypeToSpawn.density;
					if (spriteMesh != null)
					{
						spriteMesh.SetPoints(terrainLayerValue, objectTypeToSpawn, objectTypeToSpawn.spriteMeshInstanceIndex);
						objectTypeToSpawn.spriteMeshInstanceIndex++;
					}
					else
					{
						float num4 = Random.Range(objectTypeToSpawn.scaleAddRange.x, objectTypeToSpawn.scaleAddRange.y);
						newScale.Set(objectTypeToSpawn.objectToSpawn[terrainObject.prefabID].scale.x + num4, objectTypeToSpawn.objectToSpawn[terrainObject.prefabID].scale.y + num4, 1f);
						go.transform.localScale = newScale;
						float newX = meshPoints[num].x + terrainLayerValue.positionX;
						float newY = meshPoints[num].y + terrainLayerValue.positionY + objectTypeToSpawn.objectToSpawn[terrainObject.prefabID].OffsetY;
						float newZ = terrainLayerValue.depth + Random.Range(objectTypeToSpawn.zPosRange.x, objectTypeToSpawn.zPosRange.y);
						newPos.Set(newX, newY, newZ);
						go.transform.position = newPos;
						Vector3 vector = terrainLayerValue.terrainLayer.meshNormals[num];
						float num5 = Mathf.Atan2(vector.y, vector.x);
						num5 += Random.Range(objectTypeToSpawn.angleAddRange.x, objectTypeToSpawn.angleAddRange.y);
						go.transform.rotation = Quaternion.Euler(0f, 0f, num5);
						if (objectTypeToSpawn.randomFlip)
						{
							int num6 = Random.Range(0, 2);
							num6 = -num6 * 2 + 1;
							newScale.Set(go.transform.localScale.x * (float)num6, go.transform.localScale.y, go.transform.localScale.z);
							go.transform.localScale = newScale;
						}
					}
					objectTypeToSpawn.currentTrailPosIndex += num3;
					objectTypeToSpawn.noResetPosIndex += num3;
				}
			}
		}
	}

	public static void ResetIndex(TerrainLayerValue terrainLayerValue)
	{
		ObjectTypeToSpawn[] terrainObjects = terrainLayerValue.terrainObjects;
		foreach (ObjectTypeToSpawn objectTypeToSpawn in terrainObjects)
		{
			objectTypeToSpawn.currentTrailPosIndex /= 2f;
			objectTypeToSpawn.spriteMeshPosIndex /= 2f;
			if (objectTypeToSpawn.reset)
			{
				objectTypeToSpawn.distributionCurveIndex = Utility.intance.rand.Next(0, objectTypeToSpawn.distributionCurve.Length);
				objectTypeToSpawn.noResetPosIndex = 0f;
				objectTypeToSpawn.reset = false;
			}
			else
			{
				objectTypeToSpawn.reset = true;
			}
		}
	}
}
