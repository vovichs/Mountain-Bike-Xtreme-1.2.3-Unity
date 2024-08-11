using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
	public float currentStamina;

	public float currentStrength;

	public float strength;

	public float stamina;

	public float suspension;

	public Slider strengthSlider;

	public Slider staminaSlider;

	public Slider suspensionSlider;

	public Text levelText;

	public Text freePointsText;

	public int experiance;

	public int nextLevelExp;

	public int level;

	public int freePoints;

	public CanvasGroup upgradeNotification;

	public Joint2D[] suspensionJoints;

	private static PlayerStats _instance;

	public bool newLevelReached { get; set; }

	public static PlayerStats instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindObjectOfType<PlayerStats>();
			}
			return _instance;
		}
	}

	private void Start()
	{
		levelText.text = "Level: " + level;
		freePointsText.text = "Points available: " + freePoints;
		strength = PlayerPrefs.GetInt("strength");
		stamina = PlayerPrefs.GetInt("stamina");
		suspension = PlayerPrefs.GetInt("suspension");
		strengthSlider.value = strength;
		staminaSlider.value = stamina;
		SetInitialSuspension();
		SetUpgradeNotificationAlpha();
	}

	private void Update()
	{
		currentStamina = Mathf.Clamp(currentStamina, 0f, 1f);
		currentStrength = strength * currentStamina;
	}

	public void IncreaseLevel()
	{
		newLevelReached = true;
		upgradeNotification.alpha = 1f;
		level++;
		freePoints++;
		levelText.text = "Level: " + level;
		SetUpgradeNotificationAlpha();
	}

	public void UpgradeStrength()
	{
		if (freePoints > 0 && strength < 230f)
		{
			strength += 10f;
			strengthSlider.value = strength;
			freePoints--;
			freePointsText.text = "Points available: " + freePoints;
			PlayerPrefs.SetInt("strength", (int)strength);
			PlayerPrefs.SetInt("freePoints", freePoints);
			SetUpgradeNotificationAlpha();
		}
	}

	public void UpgradeStamina()
	{
		if (freePoints > 0 && stamina < 100f)
		{
			stamina += 10f;
			staminaSlider.value = stamina;
			freePoints--;
			freePointsText.text = "Points available: " + freePoints;
			PlayerPrefs.SetInt("stamina", (int)stamina);
			PlayerPrefs.SetInt("freePoints", freePoints);
			SetUpgradeNotificationAlpha();
		}
	}

	public void UpgradeSuspension()
	{
		if (freePoints > 0 && PlayerPrefs.GetInt("suspension") < 3000)
		{
			suspension += 200f;
			suspensionSlider.value = suspension;
			freePoints--;
			freePointsText.text = "Points available: " + freePoints;
			PlayerPrefs.SetInt("suspension", (int)suspension);
			PlayerPrefs.SetInt("freePoints", freePoints);
			SetUpgradeNotificationAlpha();
			SetSuspensionValue();
		}
	}

	private void SetUpgradeNotificationAlpha()
	{
		if (freePoints > 0)
		{
			upgradeNotification.alpha = 1f;
		}
		else
		{
			upgradeNotification.alpha = 0f;
		}
	}

	public void SetSuspensionValue()
	{
	}

	private void SetInitialSuspension()
	{
		Joint2D[] array = suspensionJoints;
		foreach (Joint2D joint2D in array)
		{
			joint2D.breakForce = 1000000f;
			joint2D.breakTorque = 1000000f;
		}
	}
}
