using UnityEngine;

public static class Noise
{
	public static int[] hash = new int[512]
	{
		151, 160, 137, 91, 90, 15, 131, 13, 201, 95,
		96, 53, 194, 233, 7, 225, 140, 36, 103, 30,
		69, 142, 8, 99, 37, 240, 21, 10, 23, 190,
		6, 148, 247, 120, 234, 75, 0, 26, 197, 62,
		94, 252, 219, 203, 117, 35, 11, 32, 57, 177,
		33, 88, 237, 149, 56, 87, 174, 20, 125, 136,
		171, 168, 68, 175, 74, 165, 71, 134, 139, 48,
		27, 166, 77, 146, 158, 231, 83, 111, 229, 122,
		60, 211, 133, 230, 220, 105, 92, 41, 55, 46,
		245, 40, 244, 102, 143, 54, 65, 25, 63, 161,
		1, 216, 80, 73, 209, 76, 132, 187, 208, 89,
		18, 169, 200, 196, 135, 130, 116, 188, 159, 86,
		164, 100, 109, 198, 173, 186, 3, 64, 52, 217,
		226, 250, 124, 123, 5, 202, 38, 147, 118, 126,
		255, 82, 85, 212, 207, 206, 59, 227, 47, 16,
		58, 17, 182, 189, 28, 42, 223, 183, 170, 213,
		119, 248, 152, 2, 44, 154, 163, 70, 221, 153,
		101, 155, 167, 43, 172, 9, 129, 22, 39, 253,
		19, 98, 108, 110, 79, 113, 224, 232, 178, 185,
		112, 104, 218, 246, 97, 228, 251, 34, 242, 193,
		238, 210, 144, 12, 191, 179, 162, 241, 81, 51,
		145, 235, 249, 14, 239, 107, 49, 192, 214, 31,
		181, 199, 106, 157, 184, 84, 204, 176, 115, 121,
		50, 45, 127, 4, 150, 254, 138, 236, 205, 93,
		222, 114, 67, 29, 24, 72, 243, 141, 128, 195,
		78, 66, 215, 61, 156, 180, 151, 160, 137, 91,
		90, 15, 131, 13, 201, 95, 96, 53, 194, 233,
		7, 225, 140, 36, 103, 30, 69, 142, 8, 99,
		37, 240, 21, 10, 23, 190, 6, 148, 247, 120,
		234, 75, 0, 26, 197, 62, 94, 252, 219, 203,
		117, 35, 11, 32, 57, 177, 33, 88, 237, 149,
		56, 87, 174, 20, 125, 136, 171, 168, 68, 175,
		74, 165, 71, 134, 139, 48, 27, 166, 77, 146,
		158, 231, 83, 111, 229, 122, 60, 211, 133, 230,
		220, 105, 92, 41, 55, 46, 245, 40, 244, 102,
		143, 54, 65, 25, 63, 161, 1, 216, 80, 73,
		209, 76, 132, 187, 208, 89, 18, 169, 200, 196,
		135, 130, 116, 188, 159, 86, 164, 100, 109, 198,
		173, 186, 3, 64, 52, 217, 226, 250, 124, 123,
		5, 202, 38, 147, 118, 126, 255, 82, 85, 212,
		207, 206, 59, 227, 47, 16, 58, 17, 182, 189,
		28, 42, 223, 183, 170, 213, 119, 248, 152, 2,
		44, 154, 163, 70, 221, 153, 101, 155, 167, 43,
		172, 9, 129, 22, 39, 253, 19, 98, 108, 110,
		79, 113, 224, 232, 178, 185, 112, 104, 218, 246,
		97, 228, 251, 34, 242, 193, 238, 210, 144, 12,
		191, 179, 162, 241, 81, 51, 145, 235, 249, 14,
		239, 107, 49, 192, 214, 31, 181, 199, 106, 157,
		184, 84, 204, 176, 115, 121, 50, 45, 127, 4,
		150, 254, 138, 236, 205, 93, 222, 114, 67, 29,
		24, 72, 243, 141, 128, 195, 78, 66, 215, 61,
		156, 180
	};

	private const int hashMask = 255;

	private static float sqr2 = Mathf.Sqrt(2f);

	private static Vector2[] gradients2D = new Vector2[8]
	{
		new Vector2(1f, 0f),
		new Vector2(-1f, 0f),
		new Vector2(0f, 1f),
		new Vector2(0f, -1f),
		new Vector2(1f, 1f).normalized,
		new Vector2(-1f, 1f).normalized,
		new Vector2(1f, -1f).normalized,
		new Vector2(-1f, -1f).normalized
	};

	private const int gradientsMask2D = 3;

	public static float GetValue(Vector3 point, float frequency)
	{
		point *= frequency;
		int num = Mathf.FloorToInt(point.x);
		int num2 = Mathf.FloorToInt(point.y);
		float num3 = point.x - (float)num;
		float num4 = point.y - (float)num2;
		float x = num3 - 1f;
		float y = num4 - 1f;
		num &= 0xFF;
		num2 &= 0xFF;
		int num5 = num + 1;
		int num6 = num2 + 1;
		int num7 = hash[num];
		int num8 = hash[num5];
		Vector2 g = gradients2D[hash[num7 + num2] & 3];
		Vector2 g2 = gradients2D[hash[num8 + num2] & 3];
		Vector2 g3 = gradients2D[hash[num7 + num6] & 3];
		Vector2 g4 = gradients2D[hash[num8 + num6] & 3];
		float a = Dot(g, num3, num4);
		float b = Dot(g2, x, num4);
		float a2 = Dot(g3, num3, y);
		float b2 = Dot(g4, x, y);
		float t = Smooth(num3);
		float t2 = Smooth(num4);
		float num9 = Mathf.Lerp(Mathf.Lerp(a, b, t), Mathf.Lerp(a2, b2, t), t2);
		return num9 * sqr2;
	}

	private static float Dot(Vector2 g, float x, float y)
	{
		return g.x * x + g.y * y;
	}

	private static float Smooth(float t)
	{
		return t * t * t * (t * (t * 6f - 15f) + 10f);
	}

	public static float Sum(Vector2 point, float frequency, int octaves, float lacunarity, float persistance, NoiseType noiseType)
	{
		float num = 0f;
		num = ((noiseType != 0) ? RidgedPerlinNoise(point, frequency) : GetValue(point, frequency));
		float num2 = 1f;
		float num3 = 1f;
		for (int i = 1; i < octaves; i++)
		{
			num2 *= persistance;
			frequency *= lacunarity;
			num3 += num2;
			num = ((noiseType != 0) ? (num + RidgedPerlinNoise(point, frequency) * num2) : (num + GetValue(point, frequency) * num2));
		}
		return (num / num3 + 1f) / 2f;
	}

	private static float RidgedPerlinNoise(Vector2 point, float frequency)
	{
		return 1f - Mathf.Abs(GetValue(point, frequency));
	}
}
