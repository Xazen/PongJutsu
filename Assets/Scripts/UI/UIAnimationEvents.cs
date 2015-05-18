using UnityEngine;
using System.Collections;
using System;

public class UIAnimationEvents : MonoBehaviour
{
	void transition_loadgame()
	{
		GameManager.LoadGame();
	}

	void transition_preparebuildup()
	{
		GameManager.PrepareBuildup();
	}

	void transition_buildup()
	{
		GameManager.BuildupGame();
	}

	void transition_startgame()
	{
		GameManager.StartGame();
	}

	void transition_startcountdown()
	{
		UIBase.ui.SetTrigger("StartCountdown");
	}

	void transition_unloadgame()
	{
		GameManager.UnloadGame();
	}
}
