using UnityEngine;
using System.Collections;

public class MainMenu : UIBase
{
	public void click_Start()
	{
		GameManager.EnterGame();
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

	// Online Placeholder
	void OnGUI()
	{
		if (!isActiveAndEnabled)
			return;

		GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

		if (PhotonNetwork.connected && !PhotonNetwork.inRoom)
		{
			if (GUI.Button(new Rect(Screen.width / 2f - 75, Screen.height / 3f - 15, 150, 30), "Start Online Multiplayer", buttonStyle))
				NetworkManager.StartMatchmaking();
		}
	}
}
