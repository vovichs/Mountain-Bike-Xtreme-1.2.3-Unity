using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class UnityAdsManager : MonoBehaviour
{
	private string gameID = "1807310";

	private void Awake()
	{
		Advertisement.Initialize(gameID);
		SceneManager.LoadScene(1);
	}
}
