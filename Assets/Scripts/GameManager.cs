using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PongJutsu
{
	public class GameManager : MonoBehaviour
	{
		public bool allowPause = true;
		public bool instantPlay = false;

		[HideInInspector] public static bool allowInput = false;

		private static bool isIngame = false;
		private static bool isPause = false;
		private static bool isEnd = false;

		private Animator ui;
		private GameFlow flow;

		void Awake()
		{
			ui = GameObject.Find("UI").GetComponent<Animator>();
			flow = GameObject.FindObjectOfType<GameFlow>();

			if (instantPlay)
			{
				ui.SetTrigger("InstantGame");
				StartGame();
			}
		}

		void LoadGame()
		{
			if (!isIngame)
			{
				FindObjectOfType<EventSystem>().sendNavigationEvents = false;

				foreach (GameSetup gs in this.GetComponents<GameSetup>())
				{
					gs.build();
				}

				resetChangedPrefabs();

				this.GetComponent<GameFlow>().run();

				isIngame = true;
				isPause = false;
				isEnd = false;
				allowInput = true;

				Time.timeScale = 1;
			}
		}

		void UnloadGame()
		{
			if (isIngame)
			{
				FindObjectOfType<EventSystem>().sendNavigationEvents = true;

				resetChangedPrefabs();

				foreach (GameSetup gs in this.GetComponents<GameSetup>())
				{
					gs.remove();
				}
				foreach (Shuriken s in FindObjectsOfType<Shuriken>())
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

		void resetChangedPrefabs()
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
			if ((flow.fortsLeft <= 0 || flow.fortsRight <= 0) && !isEnd && !isPause)
			{
				EndGame();
			}
		}

		public void StartGame()
		{
			LoadGame();
		}

		public void PauseGame()
		{
			Time.timeScale = 0;
			isPause = true;
			allowInput = false;
	
			ui.SetTrigger("PauseGame");
		}

		public void ResumeGame()
		{
			ui.SetTrigger("ResumeGame");

			Time.timeScale = 1;
			isPause = false;
			allowInput = true;
		}

		public void RestartGame()
		{
			UnloadGame();
			LoadGame();
		}

		public void EndGame()
		{
			ui.SetTrigger("EndGame");

			foreach (PlayerMovement pm in GameObject.FindObjectsOfType<PlayerMovement>())
			{
				pm.stopMovement();
			}

			isEnd = true;
			allowInput = false;
		}

		public void ExitGame()
		{
			ui.SetTrigger("ExitGame");
			UnloadGame();
		}
	}
}
