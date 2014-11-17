using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Fort : MonoBehaviour
	{
		public int health = 100;
		public bool flip = false;

		void Start()
		{
			if (flip)
			{
				Vector3 scale = this.transform.localScale;
				this.transform.localScale = new Vector3(scale.x * -1, scale.y);
			}

			this.GetComponentInChildren<Healthbar>().updateHealthbar(health);
		}

		public void TakeDamage(int damage)
		{
			health -= damage;
			this.GetComponentInChildren<Healthbar>().updateHealthbar(health);

			// Destroy Fort if health is 0 or lower
			if (health <= 0)
			{
				Destroy(this.gameObject);
			}
		}
	}
}