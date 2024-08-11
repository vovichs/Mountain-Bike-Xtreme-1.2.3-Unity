using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
	public AudioSource rideWind;

	public AudioSource bikeRoad;

	public AudioSource bikeWheel;

	public AudioSource bodyAudioSource;

	public AudioClip bikePedal;

	public AudioClip bikeFreeWheel;

	public AudioClip[] bikeBrake;

	public AudioClip[] bikeSpoke;

	public AudioClip[] breathe;

	public Rigidbody2D targetRb;

	private static PlayerAudioManager _instance;

	private float bikeSpokeTimer;

	private float breatheTimer;

	public static PlayerAudioManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindObjectOfType<PlayerAudioManager>();
			}
			return _instance;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		float t = targetRb.velocity.x / 30f;
		t = Smooth(t);
		rideWind.volume = AmbienceAudioManager.instance.effectsVolume * t;
		bikeRoad.volume = AmbienceAudioManager.instance.effectsVolume * Mathf.Clamp(t * 2f, 0f, 0.2f);
		switch (CycleControl.instance.playerState)
		{
		case PlayerStates.Pedal:
			bikeWheel.clip = bikePedal;
			bikeWheel.mute = false;
			bikeWheel.volume = AmbienceAudioManager.instance.effectsVolume * Mathf.Clamp(t * 10f, 0f, 1f);
			break;
		case PlayerStates.FreeWheel:
			PlaySpokeSound();
			break;
		case PlayerStates.Brake:
			bikeWheel.clip = bikeBrake[0];
			bikeWheel.volume = AmbienceAudioManager.instance.effectsVolume * Mathf.Clamp(t * 30f, 0f, 0.3f);
			bikeWheel.mute = false;
			bikeWheel.loop = false;
			break;
		}
		if (!bikeWheel.isPlaying)
		{
			bikeWheel.Play();
		}
		if (GameStateManager.inst.gameState == GameStateManager.GameStates.GameSession)
		{
			PlayBreatheSound();
		}
	}

	private void PlaySpokeSound()
	{
		bikeWheel.loop = false;
		float num = Mathf.Abs(CycleControl.instance.backWheel.angularVelocity / 100f);
		if (num < 1f)
		{
			bikeWheel.mute = true;
		}
		if (bikeSpokeTimer > 1f / num)
		{
			bikeWheel.clip = bikeSpoke[Random.Range(0, bikeSpoke.Length - 1)];
			bikeWheel.Play();
			bikeWheel.volume = AmbienceAudioManager.instance.effectsVolume * 0.4f;
			bikeSpokeTimer = 0f;
		}
		bikeSpokeTimer += Time.deltaTime;
	}

	private void PlayBreatheSound()
	{
		if (breatheTimer > Mathf.Clamp(PlayerStats.instance.currentStamina * 2f, 1f, 100f) && PlayerStats.instance.currentStamina < 0.7f)
		{
			bodyAudioSource.volume = AmbienceAudioManager.instance.effectsVolume * 1f;
			bodyAudioSource.clip = breathe[Random.Range(0, breathe.Length - 1)];
			bodyAudioSource.Play();
			breatheTimer = 0f;
		}
		breatheTimer += Time.deltaTime;
	}

	public void PlayBodyFallSound()
	{
		if (!bodyAudioSource.isPlaying)
		{
			bodyAudioSource.Play();
		}
	}

	private static float Smooth(float t)
	{
		return t * t * t * (t * (t * 6f - 15f) + 10f);
	}
}
