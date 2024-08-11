using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	private Dictionary<int, Queue<ObjectInstance>> pool = new Dictionary<int, Queue<ObjectInstance>>();

	public static ObjectPool _instance;

	public static ObjectPool instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindObjectOfType<ObjectPool>();
			}
			return _instance;
		}
	}

	public void CreatePool(ObjectToSpawn[] _objectsToSpawn, int _size, TerrainLayerValue terrainLayerValue)
	{
		int key = _objectsToSpawn.GetHashCode() + terrainLayerValue.terrainLayer.GetHashCode();
		if (!pool.ContainsKey(key))
		{
			Queue<ObjectInstance> queue = new Queue<ObjectInstance>();
			for (int i = 0; i < _size; i++)
			{
				int num = Random.Range(0, _objectsToSpawn.Length);
				GameObject gameObject = Object.Instantiate(_objectsToSpawn[num].prefab);
				TerrainObject terrainObject = gameObject.AddComponent<TerrainObject>();
				SpriteMesh component = gameObject.GetComponent<SpriteMesh>();
				ObjectInstance objectInstance = new ObjectInstance();
				objectInstance.go = gameObject;
				objectInstance.terrainObject = terrainObject;
				objectInstance.spriteMesh = component;
				terrainObject.prefabID = num;
				gameObject.SetActive(false);
				gameObject.transform.parent = base.transform;
				queue.Enqueue(objectInstance);
			}
			pool.Add(key, queue);
		}
	}

	public void CreatePool(GameObject _objectsToSpawn, int _size)
	{
		int hashCode = _objectsToSpawn.GetHashCode();
		if (!pool.ContainsKey(hashCode))
		{
			Queue<ObjectInstance> queue = new Queue<ObjectInstance>();
			for (int i = 0; i < _size; i++)
			{
				GameObject gameObject = Object.Instantiate(_objectsToSpawn);
				gameObject.SetActive(false);
				gameObject.transform.parent = base.transform;
				ObjectInstance objectInstance = new ObjectInstance();
				objectInstance.go = gameObject;
				queue.Enqueue(objectInstance);
			}
			pool.Add(hashCode, queue);
		}
	}

	public ObjectInstance ReuseObject(ObjectToSpawn[] _objectsToSpawn, TerrainLayerValue terrainLayerValue, Vector3 _position, Quaternion _rotation, Vector3 _scale)
	{
		int hashCode = _objectsToSpawn.GetHashCode();
		ObjectInstance objectInstance = null;
		if (pool.ContainsKey(hashCode))
		{
			Queue<ObjectInstance> queue = pool[hashCode];
			objectInstance = queue.Dequeue();
			queue.Enqueue(objectInstance);
			objectInstance.go.SetActive(true);
			objectInstance.go.transform.position = _position;
			objectInstance.go.transform.rotation = _rotation;
			objectInstance.go.transform.localScale = _scale;
		}
		return objectInstance;
	}

	public ObjectInstance ReuseObject(ObjectToSpawn[] _objectsToSpawn, TerrainLayerValue terrainLayerValue)
	{
		int key = _objectsToSpawn.GetHashCode() + terrainLayerValue.terrainLayer.GetHashCode();
		ObjectInstance objectInstance = null;
		if (pool.ContainsKey(key))
		{
			Queue<ObjectInstance> queue = pool[key];
			objectInstance = queue.Dequeue();
			queue.Enqueue(objectInstance);
			objectInstance.go.SetActive(true);
		}
		return objectInstance;
	}

	public ObjectInstance ReuseObject(GameObject _objectsToSpawn)
	{
		int hashCode = _objectsToSpawn.GetHashCode();
		ObjectInstance objectInstance = null;
		if (pool.ContainsKey(hashCode))
		{
			Queue<ObjectInstance> queue = pool[hashCode];
			objectInstance = queue.Dequeue();
			queue.Enqueue(objectInstance);
			objectInstance.go.SetActive(true);
		}
		return objectInstance;
	}
}
