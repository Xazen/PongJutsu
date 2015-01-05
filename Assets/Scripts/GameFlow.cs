using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class GameFlow : MonoBehaviour
	{
		// Parameters
		[HideInInspector] public float ingameTime;
		[HideInInspector] public int fortsAll;
		[HideInInspector] public int fortsLeft;
		[HideInInspector] public int fortsRight;

		// References
		private GameObject playerLeft;
		private GameObject playerRight;
		private River river;

		[HideInInspector] public static float shurikenSpeedMultiplier = 1f;

		public void run()
		{
			ingameTime = 0f;

			playerLeft = GameObject.FindGameObjectWithTag("PlayerLeft");
			playerRight = GameObject.FindGameObjectWithTag("PlayerRight");
			river = GameObject.FindGameObjectWithTag("River").GetComponent<River>();

			UpdateParamters();
		}

		void Update()
		{
			if (GameManager.isIngame)
			{
				UpdateParamters();
				UpdateFlow();
			}
		}

		void UpdateParamters()
		{
			// Time
			ingameTime += Time.deltaTime;

			// Forts
			int fortsLeftCount = 0;
			int fortsRightCount = 0;
			foreach (Fort fort in GameObject.FindObjectsOfType<Fort>())
			{
				if (fort.health > 0)
				{
					switch (fort.tag)
					{
						case "FortLeft":
							fortsLeftCount++;
							break;
						case "FortRight":
							fortsRightCount++;
							break;
					}
				}
			}
			fortsAll = fortsLeftCount + fortsRightCount;
			fortsLeft = fortsLeftCount;
			fortsRight = fortsRightCount;
		}

		void UpdateFlow()
		{
			// Hard coded GameFlow here...
		}
	}
}
