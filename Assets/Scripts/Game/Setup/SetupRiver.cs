using UnityEngine;
using System.Collections;

public class SetupRiver : GameSetup
{

	public GameObject riverPrefab;

	public override void build()
	{
		base.build();

		MainInstance = (GameObject)Instantiate(riverPrefab, new Vector2(0f, 0f), Quaternion.identity);
		MainInstance.name = riverPrefab.name;
	}

	public override void postbuild()
	{
		base.postbuild();

		MainInstance.GetComponent<River>().Setup();
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
