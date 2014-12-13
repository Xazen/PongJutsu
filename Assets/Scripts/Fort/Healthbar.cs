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

		private Vector2 initScale;

		void Awake()
		{
			initScale = this.transform.localScale;
		}

		public void updateHealthbar(int health)
		{
			// Set new healtbar scale 
			this.transform.localScale = new Vector2(initScale.x, initScale.y * (health / (float)this.GetComponentInParent<Fort>().maxHealth));

			// Lerp three colors
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