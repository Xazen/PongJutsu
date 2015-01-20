using UnityEngine;
using System.Collections;
using System;

namespace PongJutsu
{
	public class UIScript : MonoBehaviour
	{
		[HideInInspector] public Animator ui;

		void Awake()
		{
			ui = GameObject.Find("UI").GetComponent<Animator>();
		}


		// Aniamtion Events

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
}
