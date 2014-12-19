using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PongJutsu
{
	public class GameManager : MonoBehaviour
	{

		public bool instantPlay = false;

		private bool pause = false;
		private bool inGame = false;

		void Awake()
		{
			if (instantPlay)
			{
				Camera.main.GetComponent<Animator>().SetBool("InstantGame", true);
				loadGame();
			}
		}

		void loadGame()
		{
			FindObjectOfType<EventSystem>().sendNavigationEvents = false;

			foreach (GameSetup gs in this.GetComponents<GameSetup>())
			{
				gs.run();
			}

			inGame = true;
		}

		public void guie_Start()
		{
			if (!inGame)
			{
				Camera.main.GetComponent<Animator>().SetTrigger("StartGame");
				loadGame();
			}
		}
		public void guie_Options()
		{

		}
		public void guie_Credits()
		{

		}
		public void guie_Help()
		{

		}
		public void guie_Exit()
		{
			Application.Quit();
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
