using UnityEngine;
using System.Collections;

public class SetupPlayer : SetupBase
{

	public GameObject playerPrefab;
	public float offset = 0.5f;

	private float internOffset = 0.5f;

	public override void build()
	{
		base.build();

		float width = GetComponent<SetupStage>().width;
		float fortOffset = GetComponent<SetupForts>().width + GetComponent<SetupForts>().offset;

		// Player 1
		if (MultiplayerManager.CanControlFaction(Faction.Left))
			spawnPlayer(new Vector2(-width + fortOffset + offset + internOffset, 0), Faction.Left);

		// Player 2
		if (MultiplayerManager.CanControlFaction(Faction.Right))
			spawnPlayer(new Vector2(width - fortOffset - offset - internOffset, 0), Faction.Right);
	}

	private GameObject spawnPlayer(Vector2 position, Faction faction)
	{
		GameObject instance = PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity, 0, new object[] { faction });
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
