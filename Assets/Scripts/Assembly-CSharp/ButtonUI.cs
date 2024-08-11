using UnityEngine.UI;

public class ButtonUI : Button
{
	private ButtonHelp bh;

	protected override void Start()
	{
		bh = GetComponent<ButtonHelp>();
	}

	public void Update()
	{
		if (IsPressed())
		{
			bh.e.Invoke();
		}
	}
}
