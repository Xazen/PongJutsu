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
		[HideInInspector] public static bool isEnd = false;

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
					Destroy(s.gameObject);
				}
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
				checkPause();

			if (isIngame)
				checkEnd();
		}

		void checkPause()
		{
			if (Input.GetButtonDown("Pause") && !isPause && !isEnd)
			{
				ui.SetTrigger("PauseGame");
				PauseGame();
			}
		}

		void checkEnd()
		{
			if ((flow.fortsLeft <= 0 || flow.fortsRight <= 0) && !isEnd)
			{
				EndGame();
			}
		}

		public void StartGame()
		{
			LoadGame();

			isIngame = true;
			isPause = false;
			isEnd = false;
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

		public void EndGame()
		{
			ui.SetTrigger("EndGame");

			foreach (PlayerMovement pm in GameObject.FindObjectsOfType<PlayerMovement>())
			{
				pm.stopMovement();
			}

			isEnd = true;
		}

		public void QuitGame()
		{
			UnloadGame();

			isIngame = false;
			isPause = false;
			isEnd = false;
		}
	}
}
