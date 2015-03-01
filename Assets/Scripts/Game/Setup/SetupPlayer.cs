using UnityEngine;
using System.Collections;

public class SetupPlayer : GameSetup
{

	public GameObject playerPrefab;
	public float offset = 0.5f;

	private float internOffset = 0.5f;

	public override void build()
	{
		base.build();

		MainInstance = new GameObject("Players");

		float width = GetComponent<SetupStage>().width;
		float fortOffset = GetComponent<SetupForts>().width + GetComponent<SetupForts>().offset;

		// Player 1
		spawnPlayer(new Vector2(-width + fortOffset + offset + internOffset, 0), PlayerSide.Left, MainInstance, "PlayerLeft");

		// Player 2
		spawnPlayer(new Vector2(width - fortOffset - offset - internOffset, 0), PlayerSide.Right, MainInstance, "PlayerRight");
	}

	private GameObject spawnPlayer(Vector2 position, PlayerSide playerSide, GameObject parent, string name)
	{
		GameObject instance = (GameObject)Instantiate(playerPrefab);
		instance.name = name;
		instance.tag = name;
		instance.transform.parent = parent.transform;
		instance.transform.position = position;
		instance.GetComponent<PlayerBase>().Player.playerSide = playerSide;

		Instances.Add(instance);

		return instance;
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
