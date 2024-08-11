using UnityEngine;

public class SpriteObjectColor : MonoBehaviour
{
	private SpriteRenderer sprRend;

	private int _Color;

	private Color[] resultColors = new Color[2];

	private void Start()
	{
		sprRend = GetComponent<SpriteRenderer>();
		InvokeRepeating("DefineColor", 0f, Random.Range(0.05f, 0.1f));
	}

	private void DefineColor()
	{
		float z = base.transform.position.z;
		float time = z / 3200f;
		Color mainColor = DayTimeManager.instance.interpolatedDayState.intState.mainColor;
		Color secondColor = DayTimeManager.instance.interpolatedDayState.intState.secondColor;
		float H;
		float S;
		float V;
		Color.RGBToHSV(mainColor, out H, out S, out V);
		float t = DayTimeManager.instance.dtms.distanceValueCurve.Evaluate(time);
		V = Mathf.Lerp(0.2f, 0.7f, t);
		if (z == 5f)
		{
			V = 0.19f;
		}
		if (z < 3f)
		{
			V = 0.15f;
		}
		if (z > 7f && z < 50f)
		{
			V = DayTimeManager.instance.interpolatedDayState.intState.closeLayerValue;
			S /= DayTimeManager.instance.interpolatedDayState.intState.closeLayerSaturation;
		}
		float H2;
		float S2;
		float V2;
		Color.RGBToHSV(secondColor, out H2, out S2, out V2);
		V2 = Mathf.Lerp(0.2f, 1f, t);
		mainColor = Color.HSVToRGB(H, S, V);
		secondColor = Color.HSVToRGB(H2, S2, V2);
		time = DayTimeManager.instance.dtms.distanceColorCurve.Evaluate(time);
		resultColors[0] = Color.Lerp(mainColor, secondColor, time);
		sprRend.color = resultColors[0];
	}
}
