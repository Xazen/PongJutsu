using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerItemHandler : MonoBehaviour
	{
		void Update()
		{
			updateInverter();
			updateExpand();
		}

		// - - - - - - - - - - - - - - - - - - - - -

		private bool isInverted = false;
		private float invertTime;

		public void Inverter(Inverter inverter)
		{
			if (!isInverted)
			{
				this.GetComponent<PlayerMovement>().invertDirection = true;
				invertTime = inverter.duration;
				isInverted = true;
			}
			else
			{
				invertTime = inverter.duration;
			}
		}

		void updateInverter()
		{
			if (isInverted)
			{
				invertTime -= Time.deltaTime;

				if (invertTime < 0f)
				{
					this.GetComponent<PlayerMovement>().invertDirection = false;
					isInverted = false;
				}
			}
		}

		// - - - - - - - - - - - - - - - - - - - - -

		private bool isExpanded = false;
		private float expandTime;
		private Vector2 initScale;

		public void ShieldExpander(ShieldExpander shieldExpander)
		{
			PlayerShield shield = this.GetComponentInChildren<PlayerShield>();

			if (!isExpanded)
			{
				// Expand shield
				initScale = shield.transform.localScale;
				shield.transform.localScale = new Vector2(shield.transform.localScale.x, shield.transform.localScale.y * shieldExpander.scaleMultiplier);
				expandTime = shieldExpander.duration;
				isExpanded = true;
			}
			else
			{
				// Set expand duration
				expandTime = shieldExpander.duration;
			}
		}

		void updateExpand()
		{
			if (isExpanded)
			{
				expandTime -= Time.deltaTime;

				if (expandTime < 0f)
				{
					this.GetComponentInChildren<PlayerShield>().transform.localScale = new Vector2(initScale.x, initScale.y);
					isExpanded = false;
				}
			}
		}
	}
}
