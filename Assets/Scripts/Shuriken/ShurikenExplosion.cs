using UnityEngine;
using System.Collections;

public class ShurikenExplosion : Destructor
{
	private float radius;

	public void Set(float explosionRadius)
	{
		radius = explosionRadius;
		transform.localScale = new Vector2(Mathf.Sign(transform.position.x) * radius / 2f, radius / 2f);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0.95f, 0.1f, 0f, 0.8f);
		Gizmos.DrawCube(transform.position, new Vector2(1f, radius * 2f));
	}
}
