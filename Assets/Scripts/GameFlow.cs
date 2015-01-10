using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class GameFlow : MonoBehaviour
	{
		// Parameters
		public static float ingameTime;
		public static int fortsAll;
		public static int fortsLeft;
		public static int fortsRight;
		public static int combosPlayerLeft;
		public static int combosPlayerRight;

		// References
		private GameObject playerLeft;
		private GameObject playerRight;
		private River river;
		private Shuriken shuriken;

		public void run()
		{
			ingameTime = 0f;

			playerLeft = GameObject.FindGameObjectWithTag("PlayerLeft");
			playerRight = GameObject.FindGameObjectWithTag("PlayerRight");
			river = GameObject.FindGameObjectWithTag("River").GetComponent<River>();
			shuriken = Resources.LoadAssetAtPath<GameObject>("Assets/Prefabs/Shuriken.prefab").GetComponent<Shuriken>();

			UpdateParamters();
			StartFlow();
		}

		void Update()
		{
			if (GameManager.allowInput)
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

			combosPlayerLeft = playerLeft.GetComponent<Player>().comboCount;
			combosPlayerRight = playerRight.GetComponent<Player>().comboCount;
		}


		// - - - - - - - - - - - - GameFlow Scripting - - - - - - - - - - - - 

		void StartFlow()
		{
			
		}

		void UpdateFlow()
		{
			
		}
	}
}
