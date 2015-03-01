using UnityEngine;
using System.Collections;

public class ShieldExpander : Item
{
	public float scaleMultiplier = 1.5f;

	private PlayerShield shield;

	public override void OnActivation(Shuriken shuriken)
	{
		shuriken.lastHitOwner.GetComponent<PlayerItemHandler>().ShieldExpander(this);
		placeFeedback(shuriken.lastHitOwner);

		base.OnActivation(shuriken);
	}
}
