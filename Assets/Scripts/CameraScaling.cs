using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Script from Rezan Kezilgün (BS II 2014)
// Modified by Jannik Szwaczk (BS II 2014)

public class CameraScaling : MonoBehaviour
{
	public int targetWidth = 1920;
	public float pixelsToUnit = 100f;

	public static float letterboxHeight = 0f;
	public static float letterboxHeightUnits = 0f;

	public enum HandleDiffAspect
	{
		LetterBox, Stretch
	}
	public HandleDiffAspect handleDiffAspect = HandleDiffAspect.LetterBox;

	public RectTransform topBlackBar;
	public RectTransform bottomBlackBar;

	Camera cameraReference;

	void Start()
	{
		cameraReference = this.camera;
	}

	void Update()
	{
		int height = Mathf.RoundToInt(targetWidth / (float)Screen.width * (float)Screen.height);
		cameraReference.orthographicSize = (height / pixelsToUnit * 0.5f) * 2f;

		if (handleDiffAspect == HandleDiffAspect.LetterBox)
		{
			cameraReference.aspect = (float)Screen.width / (float)Screen.height;

			letterboxHeight = ((float)Screen.height - ((float)Screen.width / (16f / 9f))) * 0.5f;
			letterboxHeightUnits = camera.orthographicSize * (letterboxHeight / (float)Screen.height) * 2f;
			if (SetBlackBarHeight(letterboxHeight) == false)
			{
				letterboxHeight = 0f;
				letterboxHeightUnits = 0f;
			}
		}
		else if (handleDiffAspect == HandleDiffAspect.Stretch)
		{
			cameraReference.aspect = 16f / 9f;

			letterboxHeight = 0f;
			letterboxHeightUnits = 0f;
			SetBlackBarHeight(letterboxHeight);
		}
	}

	bool SetBlackBarHeight(float height)
	{
		if (topBlackBar == null || bottomBlackBar == null)
		{
			return false;
		}

		topBlackBar.sizeDelta = new Vector2(topBlackBar.sizeDelta.x, height);
		topBlackBar.pivot = new Vector2(topBlackBar.pivot.x, 1f);
		topBlackBar.anchoredPosition = Vector2.zero;

		bottomBlackBar.sizeDelta = new Vector2(topBlackBar.sizeDelta.x, height);
		bottomBlackBar.pivot = new Vector2(topBlackBar.pivot.x, 0f);
		bottomBlackBar.anchoredPosition = Vector2.zero;

		return true;
	}
}
