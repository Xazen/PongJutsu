using UnityEngine;
using System.Collections;

public class SetupStage : SetupBase
{

	public float width = 9f;
	public float height = 5f;

	public bool snapToVertical = false;

	void OnDrawGizmos()
	{
		float aspect = Camera.main.aspect;
		float size = Camera.main.orthographicSize;

		if (snapToVertical)
		{
			width = size * aspect;
			snapToVertical = false;
		}

		// Draw Camera Size
		Gizmos.color = new Color(0f, 0f, 0f, 0.75f);
		Gizmos.DrawLine(new Vector2(-size * aspect, size), new Vector2(size * aspect, size));
		Gizmos.DrawLine(new Vector2(-size * aspect, size), new Vector2(-size * aspect, -size));
		Gizmos.DrawLine(new Vector2(size * aspect, -size), new Vector2(-size * aspect, -size));
		Gizmos.DrawLine(new Vector2(size * aspect, -size), new Vector2(size * aspect, size));

		// Draw Stage Size
		Gizmos.color = new Color(1f, 1f, 0f);
		Gizmos.DrawLine(new Vector2(-width, height), new Vector2(width, height));
		Gizmos.DrawLine(new Vector2(-width, height), new Vector2(-width, -height));
		Gizmos.DrawLine(new Vector2(width, -height), new Vector2(-width, -height));
		Gizmos.DrawLine(new Vector2(width, -height), new Vector2(width, height));

		// Fill Stage
		//Gizmos.color = new Color(0f, 0f, 0f, 0.3f);
		//Gizmos.DrawCube(new Vector2(0, 0), new Vector2(width * 2, height * 2));
	}
}
