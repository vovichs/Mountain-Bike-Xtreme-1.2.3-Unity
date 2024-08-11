using UnityEngine;

public class MonoManager : MonoBehaviour
{
	private static MonoManager _inst;

	public static MonoManager inst
	{
		get
		{
			if (_inst == null)
			{
				_inst = Object.FindObjectOfType<MonoManager>();
			}
			return _inst;
		}
	}
}
