using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Fort : MonoBehaviour
	{

		public int maxHealth = 100;
		public bool mirror = false;

		[HideInInspector] public int health;

		public AnimatorOverrideController FortLeftController;
		public AnimatorOverrideController FortRightController;

		public bool removeAtDestroy = false;

		void Start()
		{
			if (mirror)
			{
				// Mirror the Fort
				Vector3 scale = this.transform.localScale;
				this.transform.localScale = new Vector3(scale.x * -1, scale.y);
			}

			// Set different AnimationControllers
			if (this.tag == "FortLeft")
			{
				this.GetComponent<Animator>().runtimeAnimatorController = FortLeftController;
			}
			else if (this.tag == "FortRight")
			{
				this.GetComponent<Animator>().runtimeAnimatorController = FortRightController;
			}

			// Intit values
			health = maxHealth;
			this.GetComponentInChildren<Healthbar>().updateHealthbar(health);
			this.GetComponent<Animator>().SetInteger("Health", health);
		}

		public void TakeDamage(int damage)
		{
			health -= damage;
			health = Mathf.Clamp(health, 0, maxHealth);

			// Update values
			this.GetComponentInChildren<Healthbar>().updateHealthbar(health);
			this.GetComponent<Animator>().SetInteger("Health", health);

			if (health <= 0 && removeAtDestroy)
			{
				// Destroy Fort
				Destroy(this.gameObject);
			}
			else if (health <= 0)
			{
				// Disable Fort
				this.collider2D.enabled = false;
			}
		}
	}
}