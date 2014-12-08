using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PongJutsu
{
	public class GameManager : MonoBehaviour
	{

		public bool autoStartGame = false;

		private bool pause = false;
		private bool inGame = false;

		void Awake()
		{
			if (autoStartGame)
				StartGame();
		}

		public void StartGame()
		{
			if (!inGame)
			{
				foreach (GameSetup gs in this.GetComponents<GameSetup>())
				{
					gs.run();
				}
				
				FindObjectOfType<EventSystem>().sendNavigationEvents = false;
				inGame = true;
			}
		}

		void Update()
		{
			if (inGame)
				CheckPause();
		}

		void CheckPause()
		{
			// Toggle Pause
			if (Input.GetButtonDown("Pause"))
			{
				pause = !pause;
			}

			// Set TimeScale
			if (pause)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
		}
	}
}
