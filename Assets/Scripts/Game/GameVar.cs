using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class GameVar
	{
		public static float ingameTime { get; private set; }

		public static void Refresh()
		{
			ingameTime = 0f;

			players.left.reference = GameObject.FindGameObjectWithTag("PlayerLeft");
			players.right.reference = GameObject.FindGameObjectWithTag("PlayerRight");
			river.reference = GameObject.FindGameObjectWithTag("River");
			shuriken.reference = Storage.shuriken;
		}

		public static void Update()
		{
			ingameTime += Time.deltaTime;
		}

		public class players
		{
			private static player _left = new player();
			private static player _right = new player();

			public static player left { get { return _left; } }
			public static player right { get { return _right; } }

			public class player
			{
				public GameObject reference;

				public int comboCount
				{
					get { return reference.GetComponent<Player>().comboCount; }
				}

				public int maxActiveShots
				{
					get { return reference.GetComponentInChildren<PlayerAttack>().maxActiveShots; }
					set { reference.GetComponentInChildren<PlayerAttack>().maxActiveShots = value; }
				}
				public float firerate
				{
					get { return reference.GetComponentInChildren<PlayerAttack>().firerate; }
					set { reference.GetComponentInChildren<PlayerAttack>().firerate = value; }
				}

				public float minMovementSpeed
				{
					get { return reference.GetComponentInChildren<PlayerMovement>().minMovementSpeed; }
					set { reference.GetComponentInChildren<PlayerMovement>().minMovementSpeed = value; }
				}
				public float accelerationSpeed
				{
					get { return reference.GetComponentInChildren<PlayerMovement>().accelerationSpeed; }
					set { reference.GetComponentInChildren<PlayerMovement>().accelerationSpeed = value; }
				}
				public float dashSpeed
				{
					get { return reference.GetComponentInChildren<PlayerMovement>().dashSpeed; }
					set { reference.GetComponentInChildren<PlayerMovement>().dashSpeed = value; }
				}
				public float dashCooldown
				{
					get { return reference.GetComponentInChildren<PlayerMovement>().dashCooldown; }
					set { reference.GetComponentInChildren<PlayerMovement>().dashCooldown = value; }
				}
			}
		}

		public class forts
		{
			public static int leftCount
			{
				get { return count("FortLeft"); }
			}
			public static int rightCount
			{
				get { return count("FortRight"); }
			}
			public static int allCount
			{
				get { return leftCount + rightCount; }
			}

			private static int count(string tag)
			{
				int fortsCount = 0;
				foreach (Fort fort in GameObject.FindObjectsOfType<Fort>())
				{
					if (fort.health > 0 && fort.tag == tag)
						fortsCount++;
				}

				return fortsCount;
			}
		}

		public class shuriken
		{
			public static GameObject reference;

			public static int damage
			{
				get { return reference.GetComponent<Shuriken>().damage; }
				set { reference.GetComponent<Shuriken>().damage = value; }
			}
			public static float speed
			{
				get { return reference.GetComponent<Shuriken>().speed; }
				set { reference.GetComponent<Shuriken>().speed = value; }
			}
		}

		public class river
		{
			public static GameObject reference;

			public static float spawnFrequency
			{
				get { return reference.GetComponent<River>().spawnFrequency; }
				set { reference.GetComponent<River>().spawnFrequency = value; }
			}
			public static float frequencyRandomizer
			{
				get { return reference.GetComponent<River>().frequencyRandomizer; }
				set { reference.GetComponent<River>().frequencyRandomizer = value; }
			}
			public static float flowSpeed
			{
				get { return reference.GetComponent<River>().flowSpeed; }
				set { reference.GetComponent<River>().flowSpeed = value; }
			}

			public static Dictionary<string, Item> itemList
			{
				get { return reference.GetComponent<River>().itemList; }
			}
		}
	}
}
