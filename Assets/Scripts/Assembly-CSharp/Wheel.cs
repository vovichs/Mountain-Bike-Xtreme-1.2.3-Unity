using UnityEngine;

public class Wheel : MonoBehaviour
{
	public bool isFrontWheel;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
		{
			if (isFrontWheel)
			{
				CycleControl.instance.isFrontWheelOnGround = true;
			}
			else
			{
				CycleControl.instance.isRearWheelOnGround = true;
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
		{
			if (isFrontWheel)
			{
				CycleControl.instance.isFrontWheelOnGround = false;
			}
			else
			{
				CycleControl.instance.isRearWheelOnGround = false;
			}
		}
	}
}
