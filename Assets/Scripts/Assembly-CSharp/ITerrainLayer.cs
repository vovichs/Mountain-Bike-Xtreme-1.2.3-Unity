using UnityEngine;

public interface ITerrainLayer
{
	float length { get; set; }

	int pointsN { get; set; }

	float depth { get; set; }

	float positionY { get; set; }

	float noisePoint { get; set; }

	float downOffset { get; set; }

	Vector2[] meshPoints { get; set; }

	Vector3[] meshNormals { get; set; }

	TerrainLayerValue terrainLayerValue { get; set; }

	void CalculatePoints();

	void GenerateTerrain();

	void GenerateGrid(Vector2[] _points);

	void SpawnObjects();
}
