using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class SetupPlayer : GameSetup
	{

		public GameObject playerPrefab;
		public float offset = 0.5f;
		public bool autoMirror = true;

		private float internOffset = 0.5f;

		public override void build()
		{
			base.build();

			MainInstance = new GameObject("Players");

			float width = GetComponent<SetupStage>().width;
			float fortOffset = GetComponent<SetupForts>().width + GetComponent<SetupForts>().offset;

			// Player 1
			spawnPlayer(new Vector2(-width + fortOffset + offset + internOffset, 0), false, MainInstance, "PlayerLeft", "PlayerLeft");

			// Player 2
			spawnPlayer(new Vector2(width - fortOffset - offset - internOffset, 0), autoMirror, MainInstance, "PlayerRight", "PlayerRight");
		}

		private GameObject spawnPlayer(Vector2 position, bool mirror, GameObject parent, string name, string tag)
		{
			GameObject instance = (GameObject)Instantiate(playerPrefab);
			instance.name = name;
			instance.tag = tag;
			instance.transform.parent = parent.transform;
			instance.transform.position = position;
			instance.GetComponent<Player>().mirror = mirror;

			Instances.Add(instance);

			return instance;
		}

		public override void postbuild()
		{
			base.postbuild();

			foreach (GameObject instance in Instances)
			{
				instance.GetComponent<Player>().Setup();
				instance.GetComponentInChildren<PlayerAttack>().Setup();
				instance.GetComponentInChildren<PlayerShield>().Setup();
			}
		}

		void OnDrawGizmos()
		{
			float width = GetComponent<SetupStage>().width;
			float fortOffset = GetComponent<SetupForts>().width + GetComponent<SetupForts>().offset;

			Gizmos.color = new Color(0.1f, 0.9f, 0.1f, 0.5f);

			Gizmos.DrawCube(new Vector3(-width + fortOffset + offset + internOffset, 0), (Vector2)playerPrefab.transform.localScale);
			Gizmos.DrawCube(new Vector3(width - fortOffset - offset - internOffset, 0), (Vector2)playerPrefab.transform.localScale);
		}
	}
}
