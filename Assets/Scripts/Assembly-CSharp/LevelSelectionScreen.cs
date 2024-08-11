using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionScreen : MonoBehaviour
{
	public void LoadLevel(int index)
	{
	}

	private IEnumerator Caller(int index, float t)
	{
		yield return new WaitForSeconds(t);
		SceneManager.LoadScene(index);
	}
}
