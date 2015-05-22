using UnityEngine;
using System.Collections;

public class SetupRiver : SetupBase
{

	public GameObject riverPrefab;

	public override void build()
	{
		base.build();

		if (MultiplayerManager.isMasterClient)
		{
			GameObject instance = (GameObject)Instantiate(riverPrefab, new Vector2(0f, 0f), Quaternion.identity);
			instance.name = riverPrefab.name;

			Instances.Add(instance);
		}
	}

	void OnDrawGizmos()
	{
		if (riverPrefab != null)
		{
			River river = riverPrefab.GetComponent<River>();

			Gizmos.color = new Color(0.3f, 0.3f, 0.5f, 0.5f);

			if (river.snapToVertical)
			{
				river.height = Camera.main.orthographicSize * 2;
				river.snapToVertical = false;
			}

			Gizmos.DrawCube(new Vector2(0, 0), new Vector2(river.width, river.height));
		}
	}
}
