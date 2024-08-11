using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
	public enum GameStates
	{
		GameSession = 0,
		GameOver = 1,
		ScoreView = 2
	}

	public GameStates gameState;

	public GameObject[] objectsToReset;

	public ContentPanel CP_GameOver;

	public Animator screenFadeAnimator;

	public Animator blurAnimator;

	private Animator uiAnimator;

	public bool gameScene = true;

	public GameObject playerGo;

	private Rigidbody2D[] playerBodies;

	public float timeScale = 1f;

	public Image[] controls;

	public Button startGameSessionButton;

	private static GameStateManager _inst;

	public static GameStateManager inst
	{
		get
		{
			if (_inst == null)
			{
				_inst = Object.FindObjectOfType<GameStateManager>();
			}
			return _inst;
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
		SetControlsOpacity();
		uiAnimator = GetComponent<Animator>();
		if (PersistentData.notStart)
		{
			ScreenFadeOut();
		}
		if (playerGo != null)
		{
			playerBodies = playerGo.GetComponentsInChildren<Rigidbody2D>();
		}
		if (gameScene)
		{
			OnGameSessionBegin();
		}
		Invoke("EnableStartSessionGameButton", 1.5f);
	}

	public void ResetGame()
	{
		JointSubject.instance = null;
		Invoke("LoadGameScene", 1f);
	}

	private void LoadGameScene()
	{
		SceneManager.LoadScene(PersistentData.sceneToLoad);
	}

	public void LoadMenuScene()
	{
		ScreenFadeIn();
		Invoke("LoadMenuSceneInvoke", 1f);
	}

	private void LoadMenuSceneInvoke()
	{
		SceneManager.LoadScene(1);
	}

	public void OnGameOverBegin()
	{
		uiAnimator.Play("slowMotion");
		ScoreManager.instance.CalculateTotalScore();
		ScoreManager.instance.SaveScore();
		CycleControl.instance.enabled = false;
		ContentPanelsManager.instance.ChangeContentPanel(CP_GameOver);
		gameState = GameStates.GameOver;
	}

	public void OnGameSessionBegin()
	{
		gameState = GameStates.GameSession;
		ScreenFadeOut();
	}

	public void UnBlockControls()
	{
		CycleControl.instance.enabled = true;
		CycleControl.instance.backWheel.constraints = RigidbodyConstraints2D.None;
		CycleControl.instance.bikeFrame.constraints = RigidbodyConstraints2D.None;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (SceneManager.GetActiveScene().buildIndex == 0)
			{
				CloseGame();
			}
			else
			{
				ContentPanelsManager.instance.ChangeContentPanel(ContentPanelsManager.instance.pausePanel);
			}
		}
		Time.timeScale = timeScale;
		Time.fixedDeltaTime = timeScale * 0.02f;
		if (gameState == GameStates.GameOver && Input.GetMouseButtonDown(0))
		{
			gameState = GameStates.ScoreView;
		}
	}

	public void ScreenFadeIn()
	{
		screenFadeAnimator.Play("fadeIn");
	}

	public void ScreenFadeOut()
	{
		screenFadeAnimator.Play("fadeOut");
	}

	public void BlurIn()
	{
	}

	public void BlurOut()
	{
	}

	public void DisableSlowMotion()
	{
		uiAnimator.Play("defaultState");
	}

	public void SetGameOnPause()
	{
		Rigidbody2D[] array = playerBodies;
		foreach (Rigidbody2D rigidbody2D in array)
		{
			rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
		}
	}

	public void UnpauseGame()
	{
		Rigidbody2D[] array = playerBodies;
		foreach (Rigidbody2D rigidbody2D in array)
		{
			rigidbody2D.constraints = RigidbodyConstraints2D.None;
		}
	}

	public void CloseGame()
	{
		Application.Quit();
	}

	public void SetLevelToLoad(string sceneName)
	{
		PersistentData.sceneToLoad = sceneName;
	}

	public void ShowInterisitial()
	{
		int num = Random.Range(0, 5);
		if (num == 1 && Advertisement.IsReady())
		{
			Advertisement.Show();
		}
	}

	public void StartGame()
	{
		PlayerStats.instance.suspension = PlayerPrefs.GetInt("suspension");
		ScoreManager.instance.distance = 0f;
		ScoreManager.instance.airTime = 0f;
		ScoreManager.instance.backFlips = 0;
		ScoreManager.instance.frontFlips = 0;
		ScoreManager.instance.rearWheelDistance = 0f;
		ScoreManager.instance.frontWheelDistance = 0f;
		PlayerStats.instance.SetSuspensionValue();
	}

	private void SetControlsOpacity()
	{
		float @float = PlayerPrefs.GetFloat("controlsOpacity");
		if (controls != null)
		{
			for (int i = 0; i < controls.Length; i++)
			{
				Color color = controls[i].color;
				color.a = @float;
				controls[i].color = color;
			}
		}
	}

	private void EnableStartSessionGameButton()
	{
		if (startGameSessionButton != null)
		{
			startGameSessionButton.interactable = true;
		}
	}
}
