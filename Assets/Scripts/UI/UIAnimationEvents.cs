using UnityEngine;
using System.Collections;
using System;

public class UIAnimationEvents : MonoBehaviour
{
	void transition_loadgame(int waitForBuildup)
	{
		GameManager.LoadGame(Convert.ToBoolean(waitForBuildup));
	}

	void transition_buildup()
	{
		GameManager.BuildupGame();
	}

	void transition_startgame()
	{
		GameManager.StartGame();
	}

	void transition_unloadgame()
	{
		GameManager.UnloadGame();
	}
}
