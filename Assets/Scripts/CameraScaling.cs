using UnityEngine;
using System.Collections;

// Script from Rezan Kezilgün (BS II 2014)
// Modified by Jannik Szwaczk (BS II 2014)

namespace PongJutsu
{
	public class CameraScaling : MonoBehaviour
	{

		public ScalingType scalingType = ScalingType.Width;

		// Die Hauptbildschirmbreite von dem aus die anderen berechnet werden (für ScalingTyp Width)
		public int targetWidth = 1920;

		// (für ScalingTyp Width UND Pixels)
		public float pixelsToUnit = 100f;

		// Referenz zu der Kamera
		Camera cameraReference;

		public enum ScalingType
		{
			Height, Width, Pixels
		}


		void Awake()
		{
			cameraReference = camera;
		}

		void Update()
		{
			int height = 0;
			if (scalingType == ScalingType.Width)
			{
				height = Mathf.RoundToInt(targetWidth / (float)Screen.width * Screen.height);
			}
			else if (scalingType == ScalingType.Pixels)
			{
				height = Screen.height;
			}
			cameraReference.orthographicSize = height / pixelsToUnit * 0.5f;
		}
	}
}