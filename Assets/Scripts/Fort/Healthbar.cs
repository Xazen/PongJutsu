using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Healthbar : MonoBehaviour
	{
		public Color colorFullHealth = Color.green;
		public Color colorLowHealth = Color.red;

		public bool removeAtDestroy = true;

		public void updateHealthbar(int health)
		{
			this.transform.localScale = new Vector2(1, health / 100f);
			this.GetComponent<SpriteRenderer>().color = Color.Lerp(colorFullHealth, colorLowHealth, 1 - health / 100f);

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