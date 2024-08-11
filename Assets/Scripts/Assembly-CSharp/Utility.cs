using System;
using UnityEngine;

public class Utility
{
	public System.Random rand;

	private static Utility _intance;

	public static Utility intance
	{
		get
		{
			if (_intance == null)
			{
				_intance = new Utility();
			}
			return _intance;
		}
	}

	private Utility()
	{
		rand = new System.Random(UnityEngine.Random.Range(0, 10000));
	}
}
