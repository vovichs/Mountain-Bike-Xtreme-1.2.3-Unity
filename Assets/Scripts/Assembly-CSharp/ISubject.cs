public interface ISubject
{
	void AddObserver(ISubscriber sub);

	void RemoveObserver(ISubscriber sub);

	void Notify();
}
