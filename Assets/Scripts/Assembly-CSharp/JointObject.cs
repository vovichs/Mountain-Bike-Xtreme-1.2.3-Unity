using System.Collections.Generic;
using UnityEngine;

public class JointObject : MonoBehaviour, ISubscriber
{
	private Joint2D[] allObjectJoints;

	private List<Joint2D> jointsToBreak;

	private Collider2D col;

	private static JointSubject jointSubject;

	public static bool isBraked;

	private void Start()
	{
		isBraked = false;
		allObjectJoints = GetComponents<Joint2D>();
		jointsToBreak = new List<Joint2D>();
		col = GetComponent<Collider2D>();
		Joint2D[] array = allObjectJoints;
		foreach (Joint2D joint2D in array)
		{
			if (joint2D.connectedBody.tag != "BikerBody")
			{
				jointsToBreak.Add(joint2D);
			}
		}
		JointSubject.instance.AddObserver(this);
	}

	private void Update()
	{
	}

	void ISubscriber.UpdateState()
	{
		foreach (Joint2D item in jointsToBreak)
		{
			if (item != null)
			{
				item.enabled = false;
			}
		}
		col.enabled = true;
	}

	private void OnJointBreak2D(Joint2D joint)
	{
		if (!isBraked)
		{
			JointSubject.instance.Notify();
			isBraked = true;
			GameStateManager.inst.OnGameOverBegin();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		string text = LayerMask.LayerToName(collision.gameObject.layer);
		if (text != "Biker" && text != "MountainBike")
		{
			JointSubject.instance.Notify();
			if (!isBraked)
			{
				isBraked = true;
				GameStateManager.inst.OnGameOverBegin();
			}
		}
		if (base.gameObject.name == "torso" && text != "Biker" && text != "MountainBike")
		{
			PlayerAudioManager.instance.PlayBodyFallSound();
		}
	}
}
