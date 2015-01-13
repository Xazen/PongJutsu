using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerItemHandler : MonoBehaviour
	{
		void Update()
		{
			updateExpand();
		}

		// - - - - - - - - - - - - - - - - - - - - -

		private bool isExpanded = false;
		private Vector2 initScale;
		private float expandDuration;

		public void ShieldExpander(ShieldExpander shieldExpander)
		{
			PlayerShield shield = this.GetComponentInChildren<PlayerShield>();

			if (!isExpanded)
			{
				// Expand shield
				initScale = shield.transform.localScale;
				shield.transform.localScale = new Vector2(shield.transform.localScale.x, shield.transform.localScale.y * shieldExpander.scaleMultiplier);
				isExpanded = true;
				expandDuration = shieldExpander.duration;
			}
			else
			{
				// Set expand duration
				expandDuration = shieldExpander.duration;
			}
		}

		void updateExpand()
		{
			if (isExpanded)
			{
				expandDuration -= Time.deltaTime;

				if (expandDuration <= 0)
				{
					this.GetComponentInChildren<PlayerShield>().transform.localScale = new Vector2(initScale.x, initScale.y);
					isExpanded = false;
				}
			}
		}
	}
}
