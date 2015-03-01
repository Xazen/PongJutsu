using UnityEngine;
using System.Collections;

public class MainMenu : UIScript
{
	public void click_Start()
	{
		GameManager.NewGame();
	}

	public void click_Help()
	{
		ui.SetTrigger("Help");
	}

	public void click_Options()
	{
		ui.SetTrigger("Options");
	}

	public void click_Credits()
	{
		ui.SetTrigger("Credits");
	}

	public void click_Exit()
	{
		Application.Quit();
	}
}
