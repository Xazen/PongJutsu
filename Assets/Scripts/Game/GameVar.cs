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

			players.left.gameObject = GameObject.FindGameObjectWithTag("PlayerLeft");
			players.right.gameObject = GameObject.FindGameObjectWithTag("PlayerRight");
			river.gameObject = GameObject.FindGameObjectWithTag("River");
			shurikens.gameObject = Storage.shuriken;
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
				public GameObject gameObject;

				PlayerBase _References;
				private PlayerBase References
				{
					get
					{
						if (_References == null)
							_References = gameObject.GetComponent<PlayerBase>();

						return _References;
					}
				}

				public int comboCount
				{
					get { return References.Player.comboCount; }
				}

				public int maxActiveShots
				{
					get { return References.PlayerAttack.maxActiveShots; }
					set { References.PlayerAttack.maxActiveShots = value; }
				}

				public float firerate
				{
					get { return References.PlayerAttack.firerate; }
					set { References.PlayerAttack.firerate = value; }
				}

				public float angle
				{
					get { return References.PlayerAttack.maxAngle; }
					set { References.PlayerAttack.maxAngle = value; }
				}

				public float damageMultiplier
				{
					get { return References.PlayerAttack.damageMultiplier; }
					set { References.PlayerAttack.damageMultiplier = value; }
				}
								
				public float speedMultiplier
				{
					get { return References.PlayerAttack.speedMultiplier; }
					set { References.PlayerAttack.speedMultiplier = value; }
				}

				public float minMovementSpeed
				{
					get { return References.PlayerMovement.minMovementSpeed; }
					set { References.PlayerMovement.minMovementSpeed = value; }
				}
				public float accelerationSpeed
				{
					get { return References.PlayerMovement.accelerationSpeed; }
					set { References.PlayerMovement.accelerationSpeed = value; }
				}
				public float dashSpeed
				{
					get { return References.PlayerMovement.dashSpeed; }
					set { References.PlayerMovement.dashSpeed = value; }
				}
				public float dashCooldown
				{
					get { return References.PlayerMovement.dashCooldown; }
					set { References.PlayerMovement.dashCooldown = value; }
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
					if (!fort.isDestroyed && fort.tag == tag)
						fortsCount++;
				}

				return fortsCount;
			}
		}

		public class shurikens
		{
			public static GameObject gameObject;

			public static int damage
			{
				get { return gameObject.GetComponent<Shuriken>().damage; }
				set { gameObject.GetComponent<Shuriken>().damage = value; }
			}
			public static float speed
			{
				get { return gameObject.GetComponent<Shuriken>().speed; }
				set { gameObject.GetComponent<Shuriken>().speed = value; }
			}
		}

		public class river
		{
			public static GameObject gameObject;

			public static float spawnFrequency
			{
				get { return gameObject.GetComponent<River>().spawnFrequency; }
				set { gameObject.GetComponent<River>().spawnFrequency = value; }
			}
			public static float frequencyRandomizer
			{
				get { return gameObject.GetComponent<River>().frequencyRandomizer; }
				set { gameObject.GetComponent<River>().frequencyRandomizer = value; }
			}
			public static float flowSpeed
			{
				get { return gameObject.GetComponent<River>().flowSpeed; }
				set { gameObject.GetComponent<River>().flowSpeed = value; }
			}

			public static Dictionary<string, Item> itemList
			{
				get { return gameObject.GetComponent<River>().itemList; }
			}
		}
	}
}
