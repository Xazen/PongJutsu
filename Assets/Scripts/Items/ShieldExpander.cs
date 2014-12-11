using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class ShieldExpander : Item
	{
		public float sizeMultiplier = 1.5f;
		public float duration = 5f;

		private GameObject shield;
		private bool isActivated = false;

		public override void content(Collider2D col)
		{
			base.content(col);

			// Scale shield
			shield = col.GetComponent<Shuriken>().lastHitOwner.transform.FindChild("Shield").gameObject;
			shield.transform.localScale = new Vector2(shield.transform.localScale.x, shield.transform.localScale.y * sizeMultiplier);
			isActivated = true;

			this.Hide();
		}

		void Update()
		{
			// Shield scale timer
			if (isActivated)
			{
				duration -= Time.deltaTime;
			}
			if (duration <= 0)
			{
				shield.transform.localScale = new Vector2(shield.transform.localScale.x, shield.transform.localScale.y / sizeMultiplier);
				this.Remove();
			}
		}
	}
}
