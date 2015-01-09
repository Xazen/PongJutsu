using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Item : MonoBehaviour 
	{
		public int spawnProbability = 100;
		[System.NonSerialized] private int _spawnProbability = -1;

		public void resetProbability()
		{
			if (_spawnProbability == -1)
				_spawnProbability = spawnProbability;
			else
				spawnProbability = _spawnProbability;
		}

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

