using UnityEngine;
using System.Collections;
using System;

namespace PongJutsu
{
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

		void transition_unloadgame()
		{
			GameManager.UnloadGame();
		}
	}
}
