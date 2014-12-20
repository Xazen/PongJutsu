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
			river.SetActive(!disableRiverByDefault);

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
			if (allowPause && inGame)
				PauseToggle();
			if (allowRiverToggle && inGame)
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
