using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class ShieldExpander : Item
	{
		public float scaleMultiplier = 1.5f;
		public float duration = 5f;

		private PlayerShield shield;

		public override void content(Shuriken shuriken)
		{
			shuriken.lastHitOwner.GetComponent<PlayerItemHandler>().ShieldExpander(this);

			base.content(shuriken);
		}
	}
}
