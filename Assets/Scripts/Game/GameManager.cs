using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private bool allowPause = true;
	[SerializeField]
	private bool instantPlay = false;

	public static bool allowInput = false;
	public static bool allowPauseSwitch = true;

	private static bool isIngame = false;
	private static bool isPause = false;
	private static bool isEnd = false;

	private static Animator ui;
	private static GameFlow flow;

	void Awake()
	{
		ui = GameObject.Find("UI").GetComponent<Animator>();
		flow = this.GetComponent<GameFlow>();

		StartCoroutine("IWaitForInit");
	}

	IEnumerator IWaitForInit()
	{
		yield return new WaitForSeconds(1f);
		ui.SetTrigger("Init");
	}

	void Start()
	{
		if (instantPlay)
			InstantGame();
	}

	public static void InstantGame()
	{
		GameMatch.newMatch();
		LoadGame(false);
		ui.SetTrigger("InstantGame");
	}

	public static void LoadGame(bool waitForBuildup)
	{
		if (!isIngame)
		{
			foreach (SetupBase setup in GameObject.FindObjectsOfType<SetupBase>())
			{
				setup.build();
			}
				
			resetChangedPrefabs();

			GameVar.Refresh();
			flow.StartFlow();

			GameMatch.startRound();

			isIngame = true;
			isPause = false;
			isEnd = false;
			allowInput = false;

			Cursor.visible = false;

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

			foreach (SetupBase setup in GameObject.FindObjectsOfType<SetupBase>())
			{
				setup.remove();
			}
			foreach (Shuriken shuriken in GameObject.FindObjectsOfType<Shuriken>())
			{
				Destroy(shuriken.gameObject);
			}
			foreach (ItemFeedback itemFeedback in GameObject.FindObjectsOfType<ItemFeedback>())
			{
				Destroy(itemFeedback.gameObject);
			}

			isIngame = false;
			isPause = false;
			isEnd = false;
			allowInput = false;

			MusicManager.current.StopMusic();

			Time.timeScale = 1;
		}
	}

	public static void PrepareBuildup()
	{
		GameVar.players.left.gameObject.transform.FindChild("Shield").GetComponent<Animator>().SetTrigger("Standby");
		GameVar.players.right.gameObject.transform.FindChild("Shield").GetComponent<Animator>().SetTrigger("Standby");

		GameObject.Find("Arena").GetComponent<Animator>().SetTrigger("Standby");
	}

	public static void BuildupGame()
	{
		GameVar.players.left.gameObject.transform.FindChild("Shield").GetComponent<Animator>().SetTrigger("Buildup");
		GameVar.players.right.gameObject.transform.FindChild("Shield").GetComponent<Animator>().SetTrigger("Buildup");

		GameObject.Find("Arena").GetComponent<Animator>().SetTrigger("Buildup");
	}

	static void resetChangedPrefabs()
	{
		foreach (ItemBase item in GameObject.FindGameObjectWithTag("River").GetComponent<River>().itemList.Values)
			item.resetProbability();

		Storage.shuriken.GetComponent<Shuriken>().reset();
	}

	void OnApplicationQuit()
	{
		if (GameManager.isIngame)
			resetChangedPrefabs();
	}

	void FixedUpdate()
	{
		if (allowInput)
		{
			GameVar.FixedVarUpdate();
			flow.FixedFlowUpdate();
		}

		if ((allowPause && isIngame && allowInput) || (isPause && allowPauseSwitch))
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
				EndRound("right");
			else if (GameVar.forts.rightCount <= 0)
				EndRound("left");
		}
	}

	public static void NewGame()
	{
		GameMatch.newMatch();
		ui.SetTrigger("StartGame");
	}

	public static void StartGame()
	{
		MusicManager.current.StartMusic();
		allowInput = true;
	}

	public static void PauseGame()
	{
		Time.timeScale = 0;
		isPause = true;
		allowInput = false;
		MusicManager.current.PauseMusic();

		Cursor.visible = true;

		ui.SetTrigger("PauseGame");
	}

	public static void ResumeGame()
	{
		ui.SetTrigger("ResumeGame");

		Time.timeScale = 1;
		MusicManager.current.ResumeMusic();
		isPause = false;
		allowInput = true;

		Cursor.visible = false;
	}

	public static void RestartGame()
	{
		GameMatch.newMatch();
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
			pm.StopMovementAnimation();
		}

		isEnd = true;
		allowInput = false;

		Cursor.visible = true;

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
