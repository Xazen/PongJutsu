using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Healthbar : MonoBehaviour
	{
		public Color colorFullHealth = Color.green;
		public Color colorLowHealth = Color.red;

		public void updateHealthbar(int health)
		{
			this.transform.localScale = new Vector2(1, health / 100f);
			this.GetComponent<SpriteRenderer>().color = Color.Lerp(colorFullHealth, colorLowHealth, 1 - health / 100f);
		}
	}
}