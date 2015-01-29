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

		public float duration = 0f;
		[SerializeField] private GameObject feedback;

		public virtual void OnActivation(Shuriken shuriken)
		{
			Destroy(this.gameObject);
		}

		public void placeFeedback(GameObject gameObject)
		{
			if (feedback != null)
			{
				GameObject f = (GameObject)Instantiate(feedback, gameObject.transform.position, Quaternion.identity);
				f.name = this.gameObject.name + "(Feedback)";

				if (duration > 0f)
					f.transform.GetComponent<ItemFeedback>().Setup(gameObject, duration);
				else
					f.transform.GetComponent<ItemFeedback>().Setup(gameObject);
			}
		}

		void OnTriggerEnter2D(Collider2D col)
		{
			if (col.GetComponent<Shuriken>() != null)
			{
				OnActivation(col.GetComponent<Shuriken>());
			}
		}
	}
}

