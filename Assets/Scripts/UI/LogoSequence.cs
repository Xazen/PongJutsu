using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogoSequence : MonoBehaviour
{
	public Image imageRef;

	[System.Serializable]
	public class SplashScreenObject
	{
		public Sprite logoImage;
		public float duration = 1f;
		public float fadeInDuration = 1f;
		public float fadeOutDuration = 1f;
	}

	public SplashScreenObject[] splashScreens;
	private int currentSplashScreenIndex = 0;

	public bool allowSkipping = true;
	public float skipFadeOutFactor = 3f;

	public bool automaticLevelLoading = false;
	public Text loadingtext;

	void Start()
	{
		if (automaticLevelLoading)
			loadingtext.enabled = false;

		if(splashScreens.Length > 0)
			StartSplashScreen(0);
	}

	private void SetTransparency(float transparency)
	{
		Color tempColor = imageRef.color;
		tempColor.a = transparency;
		imageRef.color = tempColor;
	}

	private float SmoothedLerp(float from, float to, float t)
	{
		t = Mathf.Clamp01(t);
		return Mathf.Lerp(from, to, Mathf.SmoothStep(0, 1f, t));
	}

	private void StartSplashScreen(int i)
	{
		currentSplashScreenIndex = i;

		imageRef.sprite = splashScreens[i].logoImage;

		StartCoroutine("FadeIn", i);
	}

	private bool NextSplashScreen()
	{
		if (currentSplashScreenIndex < splashScreens.Length - 1)
		{
			currentSplashScreenIndex++;
			StartSplashScreen(currentSplashScreenIndex);
			return true;
		}
		else
		{
			GameLoader.ActivateLoadedLevel();
		}
		return false;
	}

	IEnumerator FadeIn(int i)
	{
		SetTransparency(0f);

		float fadeDuration = splashScreens[i].fadeInDuration;
		while (fadeDuration > 0)
		{
			fadeDuration -= Time.deltaTime;

			SetTransparency(SmoothedLerp(1f, 0f, fadeDuration / splashScreens[i].fadeInDuration));
			if (Input.anyKey == true && allowSkipping == true)
				fadeDuration = 0;

			yield return new WaitForEndOfFrame();
		}

		StartCoroutine("SplashScreenDuration", i);
		yield break;
	}

	IEnumerator SplashScreenDuration(int i)
	{
		float duration = splashScreens[i].duration;
		while(duration > 0)
		{
			duration -= Time.deltaTime;
			if(Input.anyKey == true && allowSkipping == true)
				duration = 0;

			yield return new WaitForEndOfFrame();
		}

		if (automaticLevelLoading && currentSplashScreenIndex >= splashScreens.Length - 1 && !GameLoader.isLoaded())
		{
			StartCoroutine("ShowLoadingText");
			while (!GameLoader.isLoaded())
			{
				yield return new WaitForEndOfFrame();
			}
		}

		StartCoroutine("FadeOut", i);
	}

	IEnumerator FadeOut(int i)
	{		
		SetTransparency(1f);

		float fadeDuration = splashScreens[i].fadeOutDuration;
		while(fadeDuration > 0)
		{
			float factor = 1f;

			if(Input.anyKey == true && allowSkipping == true)
				factor = skipFadeOutFactor;

			fadeDuration -= factor * Time.deltaTime;
			
			SetTransparency(SmoothedLerp(0f, 1f, fadeDuration / splashScreens[i].fadeOutDuration));
			
			yield return new WaitForEndOfFrame();
		}

		NextSplashScreen();
	}

	IEnumerator ShowLoadingText()
	{
		float defaultTextTransparency = loadingtext.color.a;
		loadingtext.color = new Color (loadingtext.color.r, loadingtext.color.g, loadingtext.color.b, 0f);

		loadingtext.enabled = true;

		while (loadingtext.color.a < defaultTextTransparency)
		{
			Color tempColor = loadingtext.color;
			tempColor.a += Time.deltaTime;
			loadingtext.color = tempColor;
		}

		while (!GameLoader.isLoaded())
		{
			loadingtext.text = "Loading... " + GameLoader.getProgress() + "%";
			yield return new WaitForEndOfFrame();
		}

		float fadeDuration = splashScreens[currentSplashScreenIndex].fadeOutDuration;
		while (fadeDuration > 0f)
		{
			Color tempColor = loadingtext.color;
			tempColor.a = SmoothedLerp(0f, defaultTextTransparency, fadeDuration / splashScreens[currentSplashScreenIndex].fadeOutDuration);
			loadingtext.color = tempColor;
		}
	}
}
