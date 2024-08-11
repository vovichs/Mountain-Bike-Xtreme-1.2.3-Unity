using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public float distance;

	public float rearWheelDistance;

	public float frontWheelDistance;

	public int backFlips;

	public int frontFlips;

	public float airTime;

	public int totalScore;

	public int highScore;

	private float frontWheelAcumulateDist;

	private float rearWheelAcumulateDist;

	public float oneWheelDistTrheshold;

	private float airTimeAcumulate;

	public float airTimeTreshold;

	private Vector3 lastPosition;

	public Text distanceText;

	public Text frontWheelText;

	public Text rearWheelText;

	public Text frontFlipsText;

	public Text backFlipsText;

	public Text airTimeText;

	public Text totalScoreText;

	public Text newLevelText;

	public Text highScoreText;

	public Text tricksStatusText;

	private string frontFlipStatus = "Frontflip +10";

	private string backFlipStatus = "Backflip +10";

	private string rearWheelDistanceStatus = "Rear wheel riding ";

	private string frontWheelDistancestatus = "Front wheel riding ";

	public Slider expSlider;

	public Text debugText;

	public Text distanceShow;

	public Text scoreShow;

	public Slider staminaSlider;

	public CanvasGroup staminaSliderGroup;

	public AnimationCurve sliderAlphaByStamina;

	public AudioClip newLevelAudioClip;

	private static ScoreManager _instance;

	private string meters = "m";

	private string space = " ";

	private float flipTimer = 3f;

	private float lastRotation;

	private Color trickStatusColor = default(Color);

	public int runCount { get; set; }

	public static ScoreManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindObjectOfType<ScoreManager>();
			}
			return _instance;
		}
	}

	private void Awake()
	{
		lastPosition = CycleControl.instance.bikerBody.transform.position;
		runCount = PlayerPrefs.GetInt("runCount");
		PlayerStats.instance.level = PlayerPrefs.GetInt("level");
		PlayerStats.instance.freePoints = PlayerPrefs.GetInt("freePoints");
		PlayerStats.instance.experiance = PlayerPrefs.GetInt("exp");
		if (runCount == 0)
		{
			ResetStats();
		}
		PlayerStats.instance.nextLevelExp = PlayerPrefs.GetInt("nextLevelExp");
		expSlider.maxValue = PlayerStats.instance.nextLevelExp;
		expSlider.value = PlayerPrefs.GetInt("exp");
		highScore = PlayerPrefs.GetInt("highScore" + PersistentData.levelIndexToLoad);
		expSlider.maxValue = PlayerStats.instance.nextLevelExp;
		PlayerPrefs.SetInt("runCount", runCount + 1);
	}

	private void ResetStats()
	{
		PlayerPrefs.SetInt("level", 1);
		PlayerPrefs.SetInt("freePoints", 1);
		PlayerPrefs.SetInt("exp", 0);
		PlayerPrefs.SetInt("nextLevelExp", 30);
		for (int i = 1; i <= 6; i++)
		{
			PlayerPrefs.SetInt("highScore" + i, 0);
		}
		PlayerPrefs.SetInt("runCount", 0);
		PlayerPrefs.SetInt("strength", 130);
		PlayerPrefs.SetInt("stamina", 20);
		PlayerPrefs.SetInt("suspension", 1200);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			ResetStats();
		}
		CalculateDistance();
		CalculateFlips();
		CalculateAirTime();
		distanceShow.text = (int)distance + meters;
		scoreShow.text = ((int)(distance / 10f + rearWheelDistance + frontWheelDistance + (float)(backFlips * 10) + (float)(frontFlips * 10))).ToString();
		float time = (PlayerStats.instance.strength - PlayerStats.instance.currentStrength) / PlayerStats.instance.strength;
		staminaSlider.value = PlayerStats.instance.currentStamina;
		staminaSliderGroup.alpha = sliderAlphaByStamina.Evaluate(time);
	}

	private void CalculateDistance()
	{
		float num = Vector3.Distance(CycleControl.instance.bikerBody.transform.position, lastPosition);
		if (CycleControl.instance.bikerBody.transform.position.x > lastPosition.x)
		{
			distance += num;
		}
		lastPosition = CycleControl.instance.bikerBody.transform.position;
		if (!CycleControl.instance.isFrontWheelOnGround && CycleControl.instance.isRearWheelOnGround)
		{
			rearWheelAcumulateDist += num;
		}
		else
		{
			if (rearWheelAcumulateDist > oneWheelDistTrheshold)
			{
				rearWheelDistance += rearWheelAcumulateDist;
			}
			rearWheelAcumulateDist = 0f;
		}
		if (!CycleControl.instance.isRearWheelOnGround && CycleControl.instance.isFrontWheelOnGround)
		{
			frontWheelAcumulateDist += num;
			return;
		}
		if (frontWheelAcumulateDist > oneWheelDistTrheshold)
		{
			frontWheelDistance += frontWheelAcumulateDist;
		}
		frontWheelAcumulateDist = 0f;
	}

	private void CalculateFlips()
	{
		int rotationDir = GetRotationDir();
		if (CycleControl.instance.bikerBody.transform.eulerAngles.z > 150f && CycleControl.instance.bikerBody.transform.eulerAngles.z < 160f && GameStateManager.inst.gameState != GameStateManager.GameStates.GameOver && rotationDir == 1 && flipTimer > 0.5f)
		{
			backFlips++;
			flipTimer = 0f;
			SetTricksStatus(backFlipStatus);
		}
		if (CycleControl.instance.bikerBody.transform.eulerAngles.z > 150f && CycleControl.instance.bikerBody.transform.eulerAngles.z < 160f && GameStateManager.inst.gameState != GameStateManager.GameStates.GameOver && rotationDir == -1 && flipTimer > 0.5f)
		{
			frontFlips++;
			flipTimer = 0f;
			SetTricksStatus(frontFlipStatus);
		}
		flipTimer += Time.deltaTime;
	}

	private int GetRotationDir()
	{
		int result = (int)Mathf.Sign(CycleControl.instance.bikerBody.transform.eulerAngles.z - lastRotation);
		lastRotation = CycleControl.instance.bikerBody.transform.eulerAngles.z;
		return result;
	}

	private void CalculateAirTime()
	{
		if (!CycleControl.instance.isFrontWheelOnGround && !CycleControl.instance.isRearWheelOnGround)
		{
			airTimeAcumulate += Time.deltaTime;
			return;
		}
		if (airTimeAcumulate > airTimeTreshold)
		{
			airTime += airTimeAcumulate;
		}
		airTimeAcumulate = 0f;
	}

	public void CalculateTotalScore()
	{
		totalScore = (int)(distance / 10f + rearWheelDistance + frontWheelDistance + (float)(backFlips * 10) + (float)(frontFlips * 10) + (float)((int)airTime * 10));
		int num = totalScore;
		if (num > highScore)
		{
			highScore = num;
		}
		do
		{
			int num2 = ((!((float)num > expSlider.maxValue)) ? num : ((int)expSlider.maxValue));
			num -= num2;
			PlayerStats.instance.experiance += num2;
			expSlider.value = PlayerStats.instance.experiance;
			if (PlayerStats.instance.experiance > PlayerStats.instance.nextLevelExp)
			{
				PlayerStats.instance.IncreaseLevel();
				PlayerStats.instance.nextLevelExp = (int)((float)PlayerStats.instance.nextLevelExp * 1.5f);
				PlayerStats.instance.experiance = 0;
				newLevelText.text = PlayerStats.instance.level.ToString();
				expSlider.value = 0f;
				expSlider.maxValue = PlayerStats.instance.nextLevelExp;
			}
		}
		while (num > 0);
	}

	public void SaveScore()
	{
		distanceText.text = (int)distance + "m";
		frontWheelText.text = (int)frontWheelDistance + "m";
		rearWheelText.text = (int)rearWheelDistance + "m";
		frontFlipsText.text = frontFlips.ToString();
		backFlipsText.text = backFlips.ToString();
		airTimeText.text = (int)airTime + "s";
		totalScoreText.text = totalScore.ToString();
		highScoreText.text = PlayerPrefs.GetInt("highScore" + PersistentData.levelIndexToLoad).ToString();
		PlayerPrefs.SetInt("level", PlayerStats.instance.level);
		PlayerPrefs.SetInt("freePoints", PlayerStats.instance.freePoints);
		PlayerPrefs.SetInt("exp", (int)expSlider.value);
		PlayerPrefs.SetInt("nextLevelExp", PlayerStats.instance.nextLevelExp);
		PlayerPrefs.SetInt("highScore" + PersistentData.levelIndexToLoad, highScore);
		PlayerPrefs.SetFloat("globalTime", DayTimeManager.instance.globalTime);
		PlayerPrefs.SetFloat("tWeather", WeatherManager.instance.tWeather);
	}

	private void SetTricksStatus(string _status)
	{
		tricksStatusText.text = _status;
		StartCoroutine(TrickStatusAlpha());
	}

	private IEnumerator TrickStatusAlpha()
	{
		while (tricksStatusText.color.a < 1f)
		{
			trickStatusColor = tricksStatusText.color;
			trickStatusColor.a += 3f * Time.deltaTime;
			tricksStatusText.color = trickStatusColor;
			yield return null;
		}
		yield return new WaitForSeconds(2f);
		while (tricksStatusText.color.a > 0f)
		{
			trickStatusColor = tricksStatusText.color;
			trickStatusColor.a -= 3f * Time.deltaTime;
			tricksStatusText.color = trickStatusColor;
			yield return null;
		}
	}

	private void OneWheelRidingStatus(string _status)
	{
		tricksStatusText.text = _status;
		StartCoroutine(TrickStatusAlphaInc());
	}

	private IEnumerator TrickStatusAlphaInc()
	{
		while (tricksStatusText.color.a < 1f)
		{
			trickStatusColor = tricksStatusText.color;
			trickStatusColor.a += 3f * Time.deltaTime;
			tricksStatusText.color = trickStatusColor;
			yield return null;
		}
	}

	private IEnumerator TrickStatusAlphaDec()
	{
		while (tricksStatusText.color.a > 0f)
		{
			trickStatusColor = tricksStatusText.color;
			trickStatusColor.a -= 3f * Time.deltaTime;
			tricksStatusText.color = trickStatusColor;
			yield return null;
		}
	}
}
