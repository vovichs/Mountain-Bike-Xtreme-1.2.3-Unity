using UnityEngine;

[ExecuteInEditMode]
public class ImageEffects : MonoBehaviour
{
	public Material effectMaterial;

	[Range(0f, 10f)]
	public int iterations;

	[Range(0f, 4f)]
	public int downRes;

	private void Awake()
	{
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		int width = source.width >> downRes;
		int height = source.height >> downRes;
		RenderTexture renderTexture = RenderTexture.GetTemporary(width, height);
		renderTexture.filterMode = FilterMode.Trilinear;
		Graphics.Blit(source, renderTexture);
		for (int i = 0; i < iterations; i++)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(width, height);
			temporary.filterMode = FilterMode.Trilinear;
			Graphics.Blit(renderTexture, temporary, effectMaterial);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
		}
		Graphics.Blit(renderTexture, destination);
		RenderTexture.ReleaseTemporary(renderTexture);
	}
}
