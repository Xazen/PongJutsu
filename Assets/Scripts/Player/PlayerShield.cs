using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerShield : MonoBehaviour
	{

		public Sprite shieldLeftSprite;
		public Sprite shieldRightSprite;

		private Vector2 initScale;
		[HideInInspector] public bool isExpanded = false;
		[HideInInspector] public float expandDuration;

		void Start()
		{
			// Set different sprites for each player
			if (this.transform.parent.tag == "PlayerLeft")
				this.GetComponent<SpriteRenderer>().sprite = shieldLeftSprite;
			else if (this.transform.parent.tag == "PlayerRight")
				this.GetComponent<SpriteRenderer>().sprite = shieldRightSprite;

			initScale = this.transform.localScale;
		}

		void Update()
		{
			// Check expand
			if (isExpanded)
			{
				expandDuration -= Time.deltaTime;

				if (expandDuration <= 0)
				{
					transform.localScale = new Vector2(initScale.x, initScale.y);
					isExpanded = false;
				}
			}
		}
	}
}
