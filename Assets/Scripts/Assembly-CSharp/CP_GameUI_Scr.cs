using UnityEngine;
using UnityEngine.UI;

public class CP_GameUI_Scr : MonoBehaviour
{
	public Image[] controlImages;

	private Color color = default(Color);

	private void Start()
	{
		float @float = PlayerPrefs.GetFloat("controlsOpacity");
		for (int i = 0; i < controlImages.Length; i++)
		{
			color = controlImages[i].color;
			color.a = @float;
			controlImages[i].color = color;
		}
	}
}
