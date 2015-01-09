using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerShield : MonoBehaviour
	{

		public float shieldAngleMultiplier = 5f;

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

		void OnTriggerEnter2D(Collider2D col)
		{
			if (col.gameObject.tag == "Shuriken")
			{
				// Get Shuriken Script and GameObject
				Shuriken shuriken = col.GetComponent<Shuriken>();
				GameObject shurikenGameObject = col.gameObject;

				// Reflect
				if (shuriken.owner != this.transform.parent.gameObject)
				{
					shuriken.movement.x = -shuriken.movement.x;

					float a = shurikenGameObject.transform.position.y - this.transform.parent.transform.position.y;
					float b = this.transform.localScale.y * this.GetComponent<BoxCollider2D>().size.y;
					float c = a / (b * 0.5f);

					shuriken.movement.y = c * shieldAngleMultiplier;
					shuriken.adjustSpeed();

					this.audio.Play();

					shuriken.lastHitOwner = this.transform.parent.gameObject;
					shuriken.bounceBack = true;

					this.transform.parent.GetComponent<Player>().addCombo();
				}
				// Catch
				else if (shuriken.owner == this.transform.parent.gameObject && shuriken.bounceBack)
				{
					this.transform.parent.GetComponent<Player>().addCombo();
					shurikenGameObject.GetComponent<Shuriken>().Remove();
				}
			}
		}

		void Update()
		{
			updateExpand();
		}

		void updateExpand()
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
