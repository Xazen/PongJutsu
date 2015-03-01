using UnityEngine;
using System.Collections;

public class ShurikenExplosion : Destructor
{
	[HideInInspector]
	public float explosionRadius;
	[HideInInspector]
	public float direction;

	void Start()
	{
		this.transform.localScale = new Vector2(-direction * explosionRadius / 2f, explosionRadius / 2f);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0.95f, 0.1f, 0f, 0.8f);

		Gizmos.DrawCube(this.transform.position, new Vector2(1f, explosionRadius * 2f));
	}
}
