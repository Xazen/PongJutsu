using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private bool allowPause = true;
		[SerializeField] private bool instantPlay = false;

		public static bool allowInput = false;

		private static bool isIngame = false;
		private static bool isPause = false;
		private static bool isEnd = false;

		private static Animator ui;
		private static GameFlow flow;

		void Awake()
		{
			ui = GameObject.Find("UI").GetComponent<Animator>();
			flow = this.GetComponent<GameFlow>();
		}
		void Start()
		{
			if (instantPlay)
				InstantGame();
		}

		private static void InstantGame()
		{
			GameMatch.newMatch();
			LoadGame(false);
			StartGame();
			ui.SetTrigger("InstantGame");
		}

		public static void LoadGame(bool waitForBuildup)
		{
			if (!isIngame)
			{
				foreach (GameSetup gs in GameObject.FindObjectsOfType<GameSetup>())
					gs.build();
				foreach (GameSetup gs in GameObject.FindObjectsOfType<GameSetup>())
					gs.postbuild();

				resetChangedPrefabs();

				GameVar.Refresh();
				flow.StartFlow();

				isIngame = true;
				isPause = false;
				isEnd = false;
				allowInput = false;

				Time.timeScale = 1;

				// prepare construction
				if (waitForBuildup)
				{
					PrepareBuildup();
				}
			}
		}

		public static void UnloadGame()
		{
			if (isIngame)
			{
				resetChangedPrefabs();

				foreach (GameSetup gs in GameObject.FindObjectsOfType<GameSetup>())
				{
					gs.remove();
				}
				foreach (Shuriken s in GameObject.FindObjectsOfType<Shuriken>())
				{
					Destroy(s.gameObject);
				}

				isIngame = false;
				isPause = false;
				isEnd = false;
				allowInput = false;

				Time.timeScale = 1;
			}
		}
		
		public static void PrepareBuildup()
		{
			GameVar.players.left.reference.transform.FindChild("Shield").GetComponent<Animator>().SetTrigger("Standby");
			GameVar.players.right.reference.transform.FindChild("Shield").GetComponent<Animator>().SetTrigger("Standby");

			GameObject.Find("Arena").GetComponent<Animator>().SetTrigger("Standby");
		}

		public static void BuildupGame()
		{
			GameVar.players.left.reference.transform.FindChild("Shield").GetComponent<Animator>().SetTrigger("Buildup");
			GameVar.players.right.reference.transform.FindChild("Shield").GetComponent<Animator>().SetTrigger("Buildup");

			GameObject.Find("Arena").GetComponent<Animator>().SetTrigger("Buildup");
		}

		static void resetChangedPrefabs()
		{
			foreach (Item item in GameObject.FindGameObjectWithTag("River").GetComponent<River>().itemList.Values)
				item.resetProbability();

			 Storage.shuriken.GetComponent<Shuriken>().reset(); 
		}

		void OnApplicationQuit()
		{
			if (GameManager.isIngame)
				resetChangedPrefabs();
		}

		void Update()
		{
			if (allowInput)
			{
				GameVar.Update();
				flow.UpdateFlow();
			}

			if (allowPause && isIngame && allowInput)
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
			if (!isEnd && !isPause)
			{
				if (GameVar.forts.leftCount <= 0)
					EndRound("left");
				else if (GameVar.forts.rightCount <= 0)
					EndRound("right");
			}
		}

		public static void NewGame()
		{
			GameMatch.newMatch();
			ui.SetTrigger("StartGame");
		}

		public static void StartGame()
		{
			allowInput = true;
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
			ui.SetTrigger("RestartGame");
		}

		public static void NextRound()
		{
			ui.SetTrigger("NextRound");
		}

		public static void EndRound(string winner)
		{
			GameMatch.addWinner(winner);

			foreach (PlayerMovement pm in GameObject.FindObjectsOfType<PlayerMovement>())
			{
				pm.stopMovement();
			}

			isEnd = true;
			allowInput = false;

			ui.SetTrigger("EndRound");
		}

		public static void EndGame()
		{
			ui.SetTrigger("EndGame");
		}

		public static void ExitGame()
		{
			ui.SetTrigger("ExitGame");
		}
	}

	public class GameMatch : MonoBehaviour
	{
		private static List<string> results = new List<string>();
		private static int minRoundsToWin = 2;

		public static void newMatch()
		{
			results.Clear();
		}

		public static void addWinner(string winner)
		{
			results.Add(winner);
		}

		public static List<string> getWinnerList()
		{
			return results;
		}

		public static string getLastWinner()
		{
			return results[results.Count - 1];
		}

		public static string getMatchWinner()
		{
			if (results.FindAll(x => x == "left").Count >= minRoundsToWin)
				return "left";
			else if (results.FindAll(x => x == "right").Count >= minRoundsToWin)
				return "right";
			else
				return null;
		}
	}
}
