using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreensManager : MonoBehaviour
{
	public Animator anim;

	private static ScreensManager _inst;

	public static ScreensManager inst
	{
		get
		{
			if (_inst == null)
			{
				_inst = Object.FindObjectOfType<ScreensManager>();
			}
			return _inst;
		}
	}

	public void CallGameOverScreen()
	{
		anim.Play("gameOverScreenEnter");
	}

	public void MoveToGameScreen()
	{
		Invoke("ResetGame", 0.5f);
		CallScreenFadeIn();
	}

	public void MoveToLevelSelectionScreen()
	{
		Invoke("CallLevelSelectionScreen", 0.5f);
		CallScreenFadeIn();
	}

	public void CallScreenFadeIn()
	{
		anim.Play("screenFadeIn");
	}

	public void CallScreenFadeOut()
	{
		anim.Play("screenFadeOut");
	}

	public void ResetGame()
	{
		JointSubject.instance = null;
		SceneManager.LoadScene(1);
	}

	private void CallLevelSelectionScreen()
	{
		SceneManager.LoadScene(0);
	}

	public void CallStartGame()
	{
		anim.Play("startScreenExit");
		CycleControl.instance.enabled = true;
	}

	public void CallUpgradeScreenEnter()
	{
		anim.Play("upgradePanelScreenEnter");
	}

	public void CallUpgradeScreenExit()
	{
		anim.Play("upgradePanelScreenExit");
	}

	public void CallSettingsScreenEnter()
	{
		anim.Play("settingsEnter");
	}

	public void CallSettingsScreenExit()
	{
		anim.Play("settingsExit");
	}

	public void CallParametersScreenEnter()
	{
	}
}
