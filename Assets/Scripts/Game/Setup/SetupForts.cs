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

		MainInstance = new GameObject("Forts");

		float offsetX = GetComponent<SetupStage>().width - offset + fortPrefab.GetComponent<BoxCollider2D>().center.x;
		float offsetY = GetComponent<SetupStage>().height;

		// Forts Player Left
		for (int i = 0; i < numberOfForts; i++)
		{
			Vector2 size = new Vector2(width, (offsetY * 2) / numberOfForts);
			Vector2 position = new Vector2(-offsetX - -width / 2, size.y * i - offsetY + size.y / 2);

			spawnFort(position, size, Faction.Left, MainInstance, "FortLeft");
		}

		// Forts Player Right
		for (int i = 0; i < numberOfForts; i++)
		{
			Vector2 size = new Vector2(width, (offsetY * 2) / numberOfForts);
			Vector2 position = new Vector2(offsetX - width / 2, size.y * i - offsetY + size.y / 2);

			spawnFort(position, size, Faction.Right, MainInstance, "FortRight");
		}
	}

	private GameObject spawnFort(Vector2 position, Vector2 size, Faction faction, GameObject parent, string tag)
	{
		GameObject instance = (GameObject)Instantiate(fortPrefab);

		instance.name = tag;
		instance.tag = tag;
		instance.transform.parent = parent.transform;
		instance.GetComponent<BoxCollider2D>().size = size;
		instance.transform.position = position;
		instance.GetComponent<Fort>().faction = faction;

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
