using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PongJutsu
{
	public class GameManager : MonoBehaviour
	{
		public bool allowPause = true;
		public bool instantPlay = false;

		public static bool allowInput = false;

		private static bool isIngame = false;
		private static bool isPause = false;
		private static bool isEnd = false;

		private static Animator ui;

		void Awake()
		{
			ui = GameObject.Find("UI").GetComponent<Animator>();

			if (instantPlay)
			{
				ui.SetTrigger("InstantGame");
				StartGame();
			}
		}

		static void LoadGame()
		{
			if (!isIngame)
			{
				FindObjectOfType<EventSystem>().sendNavigationEvents = false;

				foreach (GameSetup gs in GameObject.FindObjectsOfType<GameSetup>())
				{
					gs.build();
				}

				resetChangedPrefabs();

				GameFlow.run();

				isIngame = true;
				isPause = false;
				isEnd = false;
				allowInput = true;

				Time.timeScale = 1;
			}
		}

		static void UnloadGame()
		{
			if (isIngame)
			{
				FindObjectOfType<EventSystem>().sendNavigationEvents = true;

				resetChangedPrefabs();

				foreach (GameSetup gs in GameObject.FindObjectsOfType<GameSetup>())
				{
					gs.remove();
				}
				foreach (Shuriken s in GameObject.FindObjectsOfType<Shuriken>())
				{
					DestroyImmediate(s.gameObject);
				}

				isIngame = false;
				isPause = false;
				isEnd = false;
				allowInput = false;

				Time.timeScale = 1;
			}
		}

		static void resetChangedPrefabs()
		{
			foreach (Item item in GameObject.FindGameObjectWithTag("River").GetComponent<River>().itemList.Values)
				item.resetProbability();

			Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/Shuriken.prefab").GetComponent<Shuriken>().reset();
		}

		void OnApplicationQuit()
		{
			if (GameManager.isIngame)
				resetChangedPrefabs();
		}

		void Update()
		{
			if (allowPause && isIngame)
				updatePause();

			if (isIngame)
				updateEnd();
		}

		void updatePause()
		{
			if (Input.GetButtonDown("Pause") && !isEnd)
			{
				if (isPause)
					ResumeGame();
				else
					PauseGame();
			}
		}

		void updateEnd()
		{
			if ((GameFlow.fortsLeft <= 0 || GameFlow.fortsRight <= 0) && !isEnd && !isPause)
			{
				EndGame();
			}
		}

		public static void StartGame()
		{
			LoadGame();
		}

		public static void PauseGame()
		{
			Time.timeScale = 0;
			isPause = true;
			allowInput = false;
	
			ui.SetTrigger("PauseGame");
		}

		public static void ResumeGame()
		{
			ui.SetTrigger("ResumeGame");

			Time.timeScale = 1;
			isPause = false;
			allowInput = true;
		}

		public static void RestartGame()
		{
			UnloadGame();
			LoadGame();
		}

		public static void EndGame()
		{
			ui.SetTrigger("EndGame");

			foreach (PlayerMovement pm in GameObject.FindObjectsOfType<PlayerMovement>())
			{
				pm.stopMovement();
			}

			isEnd = true;
			allowInput = false;
		}

		public static void ExitGame()
		{
			ui.SetTrigger("ExitGame");
			UnloadGame();
		}
	}
}
