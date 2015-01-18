using UnityEngine;
using System.Collections;

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

		void transition_loadgame()
		{
			GameManager.LoadGame();
		}

		void transition_buildup()
		{

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
