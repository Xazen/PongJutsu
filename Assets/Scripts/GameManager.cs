using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PongJutsu
{
	public class GameManager : MonoBehaviour
	{
		public bool allowPause = true;
		public bool allowRiverToggle = false;
		public bool disableRiverByDefault = true;

		public bool instantPlay = false;

		private bool pause = false;
		private bool inGame = false;

		private GameObject river;

		void Awake()
		{
			river = GameObject.Find("River");
			river.SetActive(false);

			if (instantPlay)
			{
				GameObject.Find("UI").GetComponent<Animator>().SetBool("InstantGame", true);
				loadGame();
			}
		}

		void loadGame()
		{
			if (!inGame)
			{
				FindObjectOfType<EventSystem>().sendNavigationEvents = false;

				river.SetActive(!disableRiverByDefault);

				foreach (GameSetup gs in this.GetComponents<GameSetup>())
				{
					gs.run();
				}

				inGame = true;
			}
		}

		public void guie_Start()
		{
			GameObject.Find("UI").GetComponent<Animator>().SetTrigger("StartGame");
			loadGame();
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
			if (allowPause && inGame)
				PauseToggle();
			if (allowRiverToggle && inGame && !pause)
				RiverToggle();
		}

		void PauseToggle()
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

		void RiverToggle()
		{
			if (Input.GetButtonDown("River"))
			{
				river.SetActive(!river.activeSelf);
			}
		}
	}
}
