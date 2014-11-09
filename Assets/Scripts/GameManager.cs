using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class GameManager : MonoBehaviour
	{

		private bool pause = false;


		void Update()
		{
			CheckPause();
		}

		void CheckPause()
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
	}
}
