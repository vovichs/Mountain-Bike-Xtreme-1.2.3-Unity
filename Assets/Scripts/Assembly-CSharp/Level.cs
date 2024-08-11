using UnityEngine;

public class Level : MonoBehaviour
{
	public int requiredPlayerLevel;

	public CanvasGroup lockObj;

	public bool isUnlocked;

	public int levelIndex;

	private void Start()
	{
		if (PlayerPrefs.GetInt("level") >= requiredPlayerLevel)
		{
			isUnlocked = true;
		}
		if (isUnlocked)
		{
			lockObj.alpha = 0f;
		}
		else
		{
			lockObj.alpha = 1f;
		}
	}

	public void UnlockLevel()
	{
		isUnlocked = true;
		lockObj.alpha = 0f;
	}

	public void LoadLevel()
	{
		if (isUnlocked)
		{
			GameStateManager.inst.ScreenFadeIn();
			PersistentData.levelIndexToLoad = levelIndex;
			GameStateManager.inst.ResetGame();
		}
	}
}
