using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PongJutsu
{
	public class GameManager : MonoBehaviour
	{
		public bool allowPause = true;
		public bool instantPlay = false;

		[HideInInspector] public static bool isIngame = false;
		[HideInInspector] public static bool isPause = false;

		private Animator ui;

		private GameObject river;

		void Awake()
		{
			ui = GameObject.Find("UI").GetComponent<Animator>();

			if (instantPlay)
			{
				ui.SetTrigger("InstantGame");
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

		public void PauseGame()
		{
			Time.timeScale = 0;
			isPause = true;
		}

		public void ContinueGame()
		{
			Time.timeScale = 1;
			isPause = false;
		}

		void Update()
		{
			if (allowPause && isIngame)
				checkPause();
		}

		void checkPause()
		{
			if (Input.GetButtonDown("Pause") && !isPause)
			{
				ui.SetTrigger("PauseGame");
				PauseGame();
			}
		}
	}
}
