using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Item : MonoBehaviour 
	{
		public int spawnProbability = 100;

		public virtual void content(Collider2D col)
		{
			
		}

		void OnTriggerEnter2D(Collider2D col)
		{
			if (col.GetComponent<Shuriken>() != null)
			{
				content(col);
			}
		}

		public void Hide()
		{
			this.GetComponent<CircleCollider2D>().enabled = false;
			this.GetComponentInChildren<SpriteRenderer>().enabled = false;
		}

		public void Remove()
		{
			Destroy(this.gameObject);
		}
	}
}

