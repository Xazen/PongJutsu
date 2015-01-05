using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PongJutsu
{
	public class GameManager : MonoBehaviour
	{
		public bool allowPause = true;
		public bool instantPlay = false;

		[HideInInspector] public static bool isPause = false;
		[HideInInspector] public static bool isIngame = false;

		private Animator UI;

		private GameObject river;

		void Awake()
		{
			UI = GameObject.Find("UI").GetComponent<Animator>();

			if (instantPlay)
			{
				UI.SetBool("InstantGame", true);
				LoadGame();
			}
		}

		public void LoadGame()
		{
			if (!isIngame)
			{
				FindObjectOfType<EventSystem>().sendNavigationEvents = false;

				foreach (GameSetup gs in this.GetComponents<GameSetup>())
				{
					gs.build();
				}
				this.GetComponent<GameFlow>().run();

				isIngame = true;
				isPause = false;
			}
		}

		public void UnloadGame()
		{
			if (isIngame)
			{
				FindObjectOfType<EventSystem>().sendNavigationEvents = true;

				foreach (GameSetup gs in this.GetComponents<GameSetup>())
				{
					gs.remove();
				}
				foreach (Shuriken s in FindObjectsOfType<Shuriken>())
				{
					Destroy(s.gameObject);
				}

				isIngame = false;
				isPause = false;
			}
		}

		public void guiE_Start()
		{
			UI.SetTrigger("StartGame");
			LoadGame();
		}
		public void guiE_Options()
		{

		}
		public void guiE_Credits()
		{

		}
		public void guiE_Help()
		{

		}
		public void guiE_Exit()
		{
			Application.Quit();
		}

		void Update()
		{
			if (allowPause && isIngame)
				PauseToggle();
		}

		void PauseToggle()
		{
			// Toggle Pause
			if (Input.GetButtonDown("Pause"))
			{
				isPause = !isPause;
			}

			// Set TimeScale
			if (isPause)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
		}
	}
}
