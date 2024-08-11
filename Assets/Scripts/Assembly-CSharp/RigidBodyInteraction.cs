using UnityEngine;

public class RigidBodyInteraction : MonoBehaviour
{
	private Rigidbody2D obj;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
		if (Input.GetMouseButton(0))
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (raycastHit2D.collider != null && obj == null)
			{
				obj = raycastHit2D.rigidbody;
			}
			if (obj != null)
			{
				obj.MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
		}
		else
		{
			obj = null;
		}
	}
}
