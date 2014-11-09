using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Fort : MonoBehaviour
	{
		public int health = 100;

		void Start()
		{
			setHealthText(health);
		}

		public void TakeDamage(int damage)
		{
			health -= damage;
			setHealthText(health);

			// Destroy Fort if health is 0 or lower
			if (health <= 0)
			{
				Destroy(this.gameObject);
			}
		}

		void setHealthText(int health)
		{
			// Print health on Fort
			this.GetComponent<TextMesh>().text = health.ToString();
		}
	}
}