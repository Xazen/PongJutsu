using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class SetupPlayer : MonoBehaviour
	{

		public GameObject playerPrefab;
		public float offset = 0.5f;
		public bool autoFlip = true;

		private float internOffset = 0.5f;

		// Player Boundries for optional offset


		void Awake()
		{
			float width = GetComponent<SetupStage>().width;
			float fortOffset = GetComponent<SetupForts>().width + GetComponent<SetupForts>().offset;

			GameObject Container = new GameObject("Players");

			// Player 1
			spawnPlayer(new Vector2(-width + fortOffset + offset + internOffset, 0), Container, "PlayerLeft", "PlayerLeft").GetComponent<Player>().flip = false;

			// Player 2
			spawnPlayer(new Vector2(width - fortOffset - offset - internOffset, 0), Container, "PlayerRight", "PlayerRight").GetComponent<Player>().flip = autoFlip;
		}

		private GameObject spawnPlayer(Vector2 position, GameObject parent, string name, string tag)
		{
			GameObject instance = (GameObject)Instantiate(playerPrefab);
			instance.name = name;
			instance.tag = tag;
			instance.transform.parent = parent.transform;
			instance.transform.position = position;

			return instance;
		}

		void OnDrawGizmos()
		{
			float width = GetComponent<SetupStage>().width;
			float fortOffset = GetComponent<SetupForts>().width + GetComponent<SetupForts>().offset;

			Gizmos.color = new Color(0.1f, 0.9f, 0.1f, 0.5f);

			Gizmos.DrawCube(new Vector3(-width + fortOffset + offset + internOffset * 2, 0), (Vector2)playerPrefab.transform.localScale);
			Gizmos.DrawCube(new Vector3(width - fortOffset - offset - internOffset * 2, 0), (Vector2)playerPrefab.transform.localScale);
		}
	}
}
