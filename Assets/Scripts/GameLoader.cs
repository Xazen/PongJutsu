using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour 
{
	[SerializeField] private string levelname = "PongJutsu";

	[SerializeField] private Text loadingtext;

	private AsyncOperation async = null;

	void Awake()
	{
		loadingtext.text = "";

		if (levelname != "")
			StartCoroutine(ILoadLevel(levelname));
	}

	void Update()
	{
		if (async != null)
			loadingtext.text = "Load " + levelname + "... " + Mathf.RoundToInt(async.progress * 100) + "%";
	}

	private IEnumerator ILoadLevel(string level)
	{
		yield return new WaitForSeconds(1f);
		async = Application.LoadLevelAsync(level);
		yield return async;
	}
}
