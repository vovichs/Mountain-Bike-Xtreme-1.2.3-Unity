using UnityEngine;

public class GesturesManager : MonoBehaviour
{
	private static GesturesManager _inst;

	public Vector2 dir;

	private Vector2 touchPontTmp;

	public static GesturesManager inst
	{
		get
		{
			if (_inst == null)
			{
				_inst = Object.FindObjectOfType<GesturesManager>();
			}
			return _inst;
		}
	}

	public bool isOnScreen { get; set; }

	public Vector2 startTouchPoint { get; set; }

	public Vector2 currentTouchPoint { get; set; }

	public float totalDrag { get; set; }

	public float dragDelta { get; set; }

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			isOnScreen = true;
			startTouchPoint = Input.mousePosition;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			isOnScreen = false;
			startTouchPoint = Vector2.zero;
			totalDrag = 0f;
		}
		if (isOnScreen)
		{
			currentTouchPoint = Input.mousePosition;
			totalDrag = Vector2.Distance(startTouchPoint, currentTouchPoint);
			dragDelta = Vector2.Distance(currentTouchPoint, touchPontTmp);
			touchPontTmp = Input.mousePosition;
			Vector2 vector = currentTouchPoint - startTouchPoint;
			if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
			{
				dir = new Vector2(1f * Mathf.Sign(vector.x), 0f);
			}
			else
			{
				dir = new Vector2(0f, 1f * Mathf.Sign(vector.y));
			}
		}
		else
		{
			startTouchPoint = Vector2.zero;
			totalDrag = 0f;
		}
	}

	private void Swipe()
	{
	}
}
