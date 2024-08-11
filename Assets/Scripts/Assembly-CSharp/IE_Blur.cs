using UnityEngine;

[ExecuteInEditMode]
public class IE_Blur : MonoBehaviour
{
	[Range(1E-05f, 1f)]
	public float sigma;

	public Material effectMaterial;

	[Range(0f, 10f)]
	public int iterations;

	[Range(0f, 4f)]
	public int downRes;

	private void Awake()
	{
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		effectMaterial.SetFloat("_SIGMA", sigma);
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
