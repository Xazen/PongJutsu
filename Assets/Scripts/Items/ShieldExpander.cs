using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class ShieldExpander : Item
	{
		public float scaleMultiplier = 1.5f;
		public float duration = 5f;

		private PlayerShield shield;

		public override void content(Collider2D col)
		{
			base.content(col);

			shield = col.GetComponent<Shuriken>().lastHitOwner.GetComponentInChildren<PlayerShield>();

			if (!shield.isExpanded)
			{
				// Expand shield
				shield.transform.localScale = new Vector2(shield.transform.localScale.x, shield.transform.localScale.y * scaleMultiplier);
				shield.isExpanded = true;
				shield.expandDuration = duration;

				this.Remove();
			}
			else
			{
				// Set expand duration
				shield.expandDuration = duration;

				this.Remove();
			}
		}

	}
}
