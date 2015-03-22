using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour 
{
	[SerializeField] private string levelname = "PongJutsu";

	[SerializeField] private bool automaticSceneActivation = true;

	private static AsyncOperation async = null;

	void Start()
	{
		if (levelname != "")
			StartCoroutine(ILoadLevel(levelname));
	}

	private IEnumerator ILoadLevel(string level)
	{
		yield return new WaitForSeconds(0.4f);
		async = Application.LoadLevelAsync(level);
		async.allowSceneActivation = automaticSceneActivation;
		yield return async;
	}

	public static void ActivateLoadedLevel()
	{
		if (async != null)
			async.allowSceneActivation = true;
	}

	public static bool isLoaded()
	{
		if (async != null)
			return async.progress > 0.89f;

		return false;
	}

	public static int getProgress()
	{
		if (async != null)
			return Mathf.RoundToInt(async.progress * 100);

		return 0;
	}
}
