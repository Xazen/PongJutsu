using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerShield : MonoBehaviour
	{

		public float shieldAngleMultiplier = 5f;

		public AnimatorOverrideController shieldLeftController;
		public AnimatorOverrideController shieldRightController;

		public Sprite shieldLeftSprite;
		public Sprite shieldRightSprite;

		public void Setup()
		{
			// Set different sprites for each player
			if (this.transform.parent.tag == "PlayerLeft")
			{
				this.GetComponent<SpriteRenderer>().sprite = shieldLeftSprite;
				this.GetComponent<Animator>().runtimeAnimatorController = shieldLeftController;
			}
			else if (this.transform.parent.tag == "PlayerRight")
			{
				this.GetComponent<SpriteRenderer>().sprite = shieldRightSprite;
				this.GetComponent<Animator>().runtimeAnimatorController = shieldRightController;
			}
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
					this.GetComponent<Animator>().SetTrigger("Reflect");

					shuriken.lastHitOwner = this.transform.parent.gameObject;
					shuriken.bounceBack = true;

					this.transform.parent.GetComponent<Player>().addCombo();
				}
				// Catch
				else if (shuriken.owner == this.transform.parent.gameObject && shuriken.bounceBack)
				{
					this.GetComponent<Animator>().SetTrigger("Catch");

					this.transform.parent.GetComponent<Player>().addCombo();
					shurikenGameObject.GetComponent<Shuriken>().Remove();
				}
			}
		}
	}
}
