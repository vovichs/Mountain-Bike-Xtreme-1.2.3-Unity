using UnityEngine;
using UnityEngine.UI;

public class DisplayFps : MonoBehaviour
{
	private Text text;

	private float timer;

	private void Start()
	{
		text = GetComponent<Text>();
	}

	private void Update()
	{
		if (timer > 1f)
		{
			float num = 1f / Time.deltaTime;
			text.text = "FPS: " + num;
			timer = 0f;
		}
		timer += Time.deltaTime;
	}
}
