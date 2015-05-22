using UnityEngine;
using System.Collections;

public class SetupForts : SetupBase
{

	public GameObject fortPrefab;

	public int numberOfForts = 5;
	[HideInInspector]
	public float width = 1f;
	public float offset = 1f;

	public bool autoMirror = true;

	public override void build()
	{
		base.build();

		float offsetX = GetComponent<SetupStage>().width - offset + fortPrefab.GetComponent<BoxCollider2D>().offset.x;
		float offsetY = GetComponent<SetupStage>().height;

		// Forts Player Left
		if (MultiplayerManager.CanControlFaction(Faction.Left))
			for (int i = 0; i < numberOfForts; i++)
			{
				Vector2 size = new Vector2(width, (offsetY * 2) / numberOfForts);
				Vector2 position = new Vector2(-offsetX - -width / 2, size.y * i - offsetY + size.y / 2);

				spawnFort(position, size, Faction.Left);
			}

		// Forts Player Right
		if (MultiplayerManager.CanControlFaction(Faction.Right))
			for (int i = 0; i < numberOfForts; i++)
			{
				Vector2 size = new Vector2(width, (offsetY * 2) / numberOfForts);
				Vector2 position = new Vector2(offsetX - width / 2, size.y * i - offsetY + size.y / 2);

				spawnFort(position, size, Faction.Right);
			}
	}

	private GameObject spawnFort(Vector2 position, Vector2 size, Faction faction)
	{
		GameObject instance = PhotonNetwork.Instantiate(fortPrefab.name, position, Quaternion.identity, 0, new object[] { faction, size });
		Instances.Add(instance);

		return instance;
	}

	void OnDrawGizmos()
	{
		float offsetX = GetComponent<SetupStage>().width - offset;
		float offsetY = GetComponent<SetupStage>().height;

		for (int i = 0; i < numberOfForts; i++)
		{
			if (i % 2 == 0)
				Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
			else
				Gizmos.color = new Color(0.15f, 0.15f, 0.8f, 0.3f);

			Vector2 size = new Vector2(width, (offsetY * 2) / numberOfForts);
			Vector2 position = new Vector2(-offsetX - -width / 2, size.y * i - offsetY + size.y / 2);
			Gizmos.DrawCube(position, size);
		}

		Gizmos.color = new Color(0.1f, 0.1f, 0.85f, 0.75f);
		Gizmos.DrawLine(new Vector2(-offsetX - -width, -offsetY), new Vector2(-offsetX - -width, offsetY));

		for (int i = 0; i < numberOfForts; i++)
		{
			if (i % 2 == 0)
				Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
			else
				Gizmos.color = new Color(0.15f, 0.15f, 0.8f, 0.3f);

			Vector2 size = new Vector2(width, (offsetY * 2) / numberOfForts);
			Vector2 position = new Vector2(offsetX - width / 2, size.y * i - offsetY + size.y / 2);
			Gizmos.DrawCube(position, size);
		}

		Gizmos.color = new Color(0.1f, 0.1f, 0.85f, 0.75f);
		if (autoMirror)
			Gizmos.DrawLine(new Vector2(offsetX - width, -offsetY), new Vector2(offsetX - width, offsetY));
		else
			Gizmos.DrawLine(new Vector2(offsetX, -offsetY), new Vector2(offsetX, offsetY));
	}
}
