using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class GameFlow : MonoBehaviour
	{
		// Parameters
		private float ingameTime;
		private int fortsAll;
		private int fortsLeft;
		private int fortsRight;

		// References
		private GameObject playerLeft;
		private GameObject playerRight;
		private River river;

		public void run()
		{
			ingameTime = 0f;

			playerLeft = GameObject.FindGameObjectWithTag("PlayerLeft");
			playerRight = GameObject.FindGameObjectWithTag("PlayerRight");
			river = GameObject.FindGameObjectWithTag("River").GetComponent<River>();
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
			Debug.Log("Ingame Time: "  + (int)ingameTime + ";\n"
					+ "Number of Forts... " + "All: " + fortsAll + ";" + "Left: " + fortsLeft + "; " + "Right: " + fortsRight + ";\n");

			// Hard coded GameFlow here...
		}
	}
}
