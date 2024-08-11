using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;

	public Rigidbody2D targetRb;

	private float smooth = 2f;

	private float cameraOffset = -1f;

	private float maxCameraOffset = -5f;

	public bool cameraFreeFly;

	public Camera bgCamera;

	private void Awake()
	{
		Application.targetFrameRate = 60;
	}

	private void Start()
	{
		Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
		if (cameraFreeFly)
		{
			target = base.transform;
		}
	}

	private void FixedUpdate()
	{
		if (!cameraFreeFly)
		{
			float num = cameraOffset - targetRb.velocity.x / 2f;
			if (num > cameraOffset)
			{
				num = cameraOffset;
			}
			else if (num < maxCameraOffset)
			{
				num = maxCameraOffset;
			}
			float num2 = Mathf.Lerp(base.transform.eulerAngles.z, targetRb.transform.eulerAngles.z, Time.deltaTime / 20f);
			float x = Mathf.Lerp(base.transform.position.x, target.position.x + 4f + targetRb.velocity.x / 7f, Time.deltaTime * smooth * 2f);
			float y = Mathf.Lerp(base.transform.position.y, target.position.y + targetRb.velocity.x / 80f + 0.5f, Time.deltaTime * smooth * 2f);
			float z = Mathf.Lerp(base.transform.position.z, num, Time.deltaTime * smooth);
			Vector3 position = new Vector3(x, y, z);
			base.transform.position = position;
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			base.transform.Translate(Vector2.right * 100f * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			base.transform.Translate(Vector2.down * 100f * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.UpArrow))
		{
			base.transform.Translate(Vector2.up * 100f * Time.deltaTime);
		}
	}
}
