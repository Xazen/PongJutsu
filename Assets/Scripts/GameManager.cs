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

		void Awake()
		{
			if (instantPlay)
			{
				GameObject.Find("UI").GetComponent<Animator>().SetBool("InstantGame", true);
				loadGame();
			}
		}

		void loadGame()
		{
			if (!isIngame)
			{
				FindObjectOfType<EventSystem>().sendNavigationEvents = false;

				foreach (GameSetup gs in this.GetComponents<GameSetup>())
				{
					gs.run();
				}

				isIngame = true;
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
