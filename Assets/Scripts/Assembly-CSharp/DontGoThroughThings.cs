using UnityEngine;

public class DontGoThroughThings : MonoBehaviour
{
	public bool sendTriggerMessage;

	public LayerMask layerMask = -1;

	public float skinWidth = 0.1f;

	private float minimumExtent;

	private float partialExtent;

	private float sqrMinimumExtent;

	private Vector2 previousPosition;

	private Rigidbody2D myRigidbody;

	private Collider2D myCollider;

	private void Start()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D>();
		previousPosition = myRigidbody.position;
		minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
		partialExtent = minimumExtent * (1f - skinWidth);
		sqrMinimumExtent = minimumExtent * minimumExtent;
	}

	private void FixedUpdate()
	{
		Vector3 vector = myRigidbody.position - previousPosition;
		float sqrMagnitude = vector.sqrMagnitude;
		if (sqrMagnitude > sqrMinimumExtent)
		{
			float num = Mathf.Sqrt(sqrMagnitude);
			RaycastHit hitInfo;
			if (Physics.Raycast(previousPosition, vector, out hitInfo, num, layerMask.value))
			{
				if (!hitInfo.collider)
				{
					return;
				}
				if (hitInfo.collider.isTrigger)
				{
					hitInfo.collider.SendMessage("OnTriggerEnter", myCollider);
				}
				if (!hitInfo.collider.isTrigger)
				{
					myRigidbody.position = hitInfo.point - vector / num * partialExtent;
				}
			}
		}
		previousPosition = myRigidbody.position;
	}
}
