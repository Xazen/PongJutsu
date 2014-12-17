using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class GameManager : MonoBehaviour
	{
		public bool allowPause = true;
		public bool allowRiverToggle = false;

		private bool pause = false;
		private GameObject river;

		void Awake()
		{
			river = GameObject.Find("River");
		}

		void Update()
		{
			if (allowPause)
				PauseToggle();
			if (allowRiverToggle)
				RiverToggle();
		}

		void PauseToggle()
		{
			// Toggle Pause
			if (Input.GetButtonDown("Pause"))
			{
				pause = !pause;
			}

			// Set TimeScale
			if (pause)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
		}

		void RiverToggle()
		{
			if (Input.GetButtonDown("River"))
			{
				river.SetActive(!river.active);
			}
		}
	}
}
