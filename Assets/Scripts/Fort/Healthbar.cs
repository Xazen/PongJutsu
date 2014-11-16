using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Healthbar : MonoBehaviour
	{
		public void updateHealthbar(int health)
		{
			this.transform.localScale = new Vector2(1, health / 100f);
			this.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.green, Color.red, 1 - health / 100f);
		}
	}
}