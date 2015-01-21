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
			updateSlow();
		}

		// - - - - - - - - - - - - - - - - - - - - -

		[HideInInspector] public bool isSlow = false;
		private float slowTime;

		private float originalSpeed;

		public void Slow(Slow slow)
		{
			if (!isSlow)
			{
				originalSpeed = this.GetComponent<PlayerMovement>().maxMovementSpeed;
				this.GetComponent<PlayerMovement>().maxMovementSpeed *= slow.speedMuliplier;
				slowTime = slow.duration;
				isSlow = true;
			}
			else
			{
				slowTime = slow.duration;
			}
		}

		void updateSlow()
		{
			if (isSlow)
			{
				slowTime -= Time.deltaTime;

				if (slowTime < 0f)
				{
					this.GetComponent<PlayerMovement>().maxMovementSpeed = originalSpeed;
					isSlow = false;
				}
			}
		}

		// - - - - - - - - - - - - - - - - - - - - -

		[HideInInspector] public bool isInverted = false;
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

		[HideInInspector] public bool isExpanded = false;
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
