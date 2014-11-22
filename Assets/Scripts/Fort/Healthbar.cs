using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Healthbar : MonoBehaviour
	{
		public Color colorFullHealth = Color.green;
		public Color colorMediumHealth = Color.Lerp(Color.red, Color.yellow, 0.5f);
		public Color colorLowHealth = Color.red;

		public bool removeAtDestroy = true;

		public void updateHealthbar(int health)
		{
			this.transform.localScale = new Vector2(1, health / (float)this.GetComponentInParent<Fort>().maxHealth);

			if (health > 50)
				this.GetComponent<SpriteRenderer>().color = Color.Lerp(colorMediumHealth, colorFullHealth, (float)health / (this.GetComponentInParent<Fort>().maxHealth / 2f) - 1f);
			else if (health <= 50)
				this.GetComponent<SpriteRenderer>().color = Color.Lerp(colorLowHealth, colorMediumHealth, (float)health / (this.GetComponentInParent<Fort>().maxHealth / 2f));


			if (health <= 0 && removeAtDestroy)
			{
				// remove healthbar
				Destroy(this.gameObject);
				Destroy(this.transform.parent.transform.FindChild("HealthbarFrame").gameObject);
				Destroy(this.transform.parent.transform.FindChild("HealthbarBackground").gameObject);
			}
		}
	}
}