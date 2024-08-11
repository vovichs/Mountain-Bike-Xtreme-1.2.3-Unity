using UnityEngine;

public class Cloud : MonoBehaviour
{
	private MaterialPropertyBlock propBlock;

	private Material material;

	private SpriteRenderer sprRend;

	public float alpha { get; set; }

	private void Start()
	{
		propBlock = new MaterialPropertyBlock();
		sprRend = GetComponent<SpriteRenderer>();
		sprRend.GetPropertyBlock(propBlock);
	}

	private void Update()
	{
		propBlock.SetFloat("_Alpha", alpha);
		sprRend.SetPropertyBlock(propBlock);
	}
}
