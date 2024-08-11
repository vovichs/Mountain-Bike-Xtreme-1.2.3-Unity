using UnityEngine;

public class CycleControl : MonoBehaviour
{
	public Rigidbody2D backWheel;

	public Rigidbody2D frontWheel;

	public Rigidbody2D bikeFrame;

	public Rigidbody2D pedal;

	public Rigidbody2D[] legs;

	public Rigidbody2D bikerBody;

	private SpringJoint2D[] bikerJoints;

	private float topJointDist;

	private float rightJointDist;

	private AudioSource audioSource;

	private int layerMask;

	public PlayerStates playerState;

	public bool isFrontWheelOnGround;

	public bool isRearWheelOnGround;

	public AnimationCurve speedCurve;

	public AnimationCurve pedalCurve;

	private static CycleControl _instance;

	public int terrainLayer;

	private float spaceHoldTime;

	private Vector2 tmpPoint;

	private float bms = 5f;

	private float leanLeftJointDist = 1.3f;

	private float leanRightJointDist;

	private Vector2 leanRightPos;

	public float rayLength;

	public MobileInput mobileInput_Move;

	public MobileInput mobileInput_Lean;

	public static CycleControl instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindObjectOfType<CycleControl>();
			}
			return _instance;
		}
	}

	private void Start()
	{
		layerMask = LayerMask.GetMask("Ground");
		audioSource = GetComponent<AudioSource>();
		bikerJoints = bikerBody.GetComponents<SpringJoint2D>();
		topJointDist = bikerJoints[0].distance;
		rightJointDist = bikerJoints[1].distance;
		terrainLayer = LayerMask.GetMask("Terrain");
	}

	private void FixedUpdate()
	{
		CheckOnGround();
		if (Input.GetMouseButton(0) && Input.mousePosition.x < (float)Screen.width / 2f)
		{
			BodyControl();
		}
		else
		{
			tmpPoint = Vector2.zero;
		}
		if (Input.GetMouseButtonUp(0))
		{
			bikerJoints[1].distance = rightJointDist;
		}
		if (Input.GetKey(KeyCode.D) || mobileInput_Move == MobileInput.Pedal)
		{
			Pedal();
			playerState = PlayerStates.Pedal;
			PlayerStats.instance.currentStamina -= Time.deltaTime / PlayerStats.instance.stamina;
		}
		else if (Input.GetKey(KeyCode.A) || mobileInput_Move == MobileInput.Brake)
		{
			Brake();
			playerState = PlayerStates.Brake;
		}
		else
		{
			playerState = PlayerStates.FreeWheel;
			PlayerStats.instance.currentStamina += Time.deltaTime / 4f;
		}
		if (Input.GetKey(KeyCode.Q) || mobileInput_Lean == MobileInput.LeanLeft)
		{
			LeanLeft();
		}
		else if (Input.GetKey(KeyCode.E) || mobileInput_Lean == MobileInput.LeanRight)
		{
			LeanRight();
		}
		else
		{
			bikerJoints[1].distance = rightJointDist;
		}
	}

	private void BodyControl()
	{
		if (tmpPoint == Vector2.zero)
		{
			tmpPoint = Input.mousePosition;
		}
		Vector2 vector = Input.mousePosition;
		float magnitude = (tmpPoint - vector).magnitude;
		Vector2 normalized = (vector - tmpPoint).normalized;
		Vector2 vector2 = normalized * magnitude;
		bikerJoints[0].distance = topJointDist - vector2.y / 500f;
		bikerJoints[1].distance = rightJointDist - vector2.x / 500f;
		tmpPoint = vector;
	}

	public void Pedal()
	{
		pedal.constraints = RigidbodyConstraints2D.None;
		backWheel.constraints = RigidbodyConstraints2D.None;
		float torque = 0f - PlayerStats.instance.currentStrength;
		float angularVelocity = backWheel.angularVelocity;
		float angularVelocity2 = frontWheel.angularVelocity;
		angularVelocity = speedCurve.Evaluate(0f - angularVelocity);
		angularVelocity2 = speedCurve.Evaluate(0f - angularVelocity2);
		frontWheel.angularVelocity = 0f - angularVelocity;
		backWheel.angularVelocity = 0f - angularVelocity2;
		backWheel.AddTorque(torque, ForceMode2D.Force);
		float torque2 = 0f - pedalCurve.Evaluate(0f - backWheel.angularVelocity);
		pedal.AddTorque(torque2, ForceMode2D.Force);
	}

	public void Brake()
	{
		backWheel.constraints = RigidbodyConstraints2D.FreezeRotation;
		pedal.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	public void LeanLeft()
	{
		bikerJoints[1].distance = leanLeftJointDist;
		bikeFrame.AddTorque(PlayerStats.instance.strength * 1.3f, ForceMode2D.Force);
	}

	public void LeanRight()
	{
		bikerJoints[1].distance = leanRightJointDist;
		bikeFrame.AddTorque((0f - PlayerStats.instance.strength) * 1.1f, ForceMode2D.Force);
	}

	private void CheckOnGround()
	{
	}

	public void MB_Pedal()
	{
		mobileInput_Move = MobileInput.Pedal;
	}

	public void MB_Brake()
	{
		mobileInput_Move = MobileInput.Brake;
	}

	public void MB_LeanLeft()
	{
		mobileInput_Lean = MobileInput.LeanLeft;
	}

	public void MB_LeanRight()
	{
		mobileInput_Lean = MobileInput.LeanRight;
	}

	public void MB_Move_None()
	{
		mobileInput_Move = MobileInput.None;
	}

	public void MB_Lean_None()
	{
		mobileInput_Lean = MobileInput.None;
	}
}
