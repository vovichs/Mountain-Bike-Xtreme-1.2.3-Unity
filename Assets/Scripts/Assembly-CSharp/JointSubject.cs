using System.Collections.Generic;

public class JointSubject : ISubject
{
	private List<ISubscriber> subscribers = new List<ISubscriber>();

	private static JointSubject _instance;

	public static JointSubject instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new JointSubject();
			}
			return _instance;
		}
		set
		{
			_instance = value;
		}
	}

	public void AddObserver(ISubscriber sub)
	{
		subscribers.Add(sub);
	}

	public void RemoveObserver(ISubscriber sub)
	{
		subscribers.Remove(sub);
	}

	public void Notify()
	{
		foreach (ISubscriber subscriber in subscribers)
		{
			subscriber.UpdateState();
		}
	}
}
