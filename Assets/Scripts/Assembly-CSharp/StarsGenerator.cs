using UnityEngine;

public class StarsGenerator : MonoBehaviour
{
	public Sprite starSprite;

	public Material material;

	public int n;

	public AnimationCurve alphaByTime;

	public AnimationCurve alphaByWeather;

	public Transform starsHolder;

	private int _Alpha;

	public float starAlpha;

	private void Start()
	{
		_Alpha = Shader.PropertyToID("_Alpha");
		for (int i = 0; i < n; i++)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "star_" + i;
			gameObject.transform.parent = starsHolder;
			gameObject.transform.eulerAngles = new Vector3(0f, 0f, 45f);
			float num = Random.Range(200f, 500f);
			gameObject.transform.localScale = new Vector2(num, num);
			gameObject.transform.position = new Vector3(Random.Range(-2500f, 2500f), Random.Range(-2500f, 2500f), 2050f);
			SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.color = new Color(Random.Range(0.8f, 1f), Random.Range(0.8f, 0.9f), Random.Range(0.9f, 1f), Random.Range(0f, 0.7f));
			spriteRenderer.sortingOrder = 0;
			spriteRenderer.sprite = starSprite;
			spriteRenderer.material = material;
		}
	}

	private void Update()
	{
		float num = alphaByTime.Evaluate(DayTimeManager.instance.globalTime);
		float num2 = alphaByWeather.Evaluate(WeatherManager.instance.tWeather);
		starAlpha = num * num2;
		starsHolder.Rotate(0f, 0f, -0.3f * Time.deltaTime);
		material.SetFloat(_Alpha, starAlpha);
	}
}
