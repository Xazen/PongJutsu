using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Item : MonoBehaviour 
	{
		[SerializeField] private int _spawnProbability = 100;
		public int spawnProbability 
		{
			get { return Mathf.Clamp(_spawnProbability, 0, 100); }
			set { _spawnProbability = Mathf.Clamp(value, 0, 100); }
		}

		[System.NonSerialized] private int defaultProbability = -1;
		public void resetProbability()
		{
			if (defaultProbability == -1)
				defaultProbability = spawnProbability;
			else
				spawnProbability = defaultProbability;
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

