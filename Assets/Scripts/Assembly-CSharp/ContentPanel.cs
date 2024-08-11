using UnityEngine;

public class ContentPanel : MonoBehaviour
{
	private Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void Enter()
	{
		anim.Play("contentPanelEnter");
	}

	public void Exit()
	{
		anim.Play("contentPanelExit");
	}
}
