﻿using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Item : MonoBehaviour 
	{
		[SerializeField] private int _spawnProbability = 100;
		public int spawnProbability 
		{
			get { return Mathf.Clamp(_spawnProbability, 0, 200); }
			set { _spawnProbability = Mathf.Clamp(value, 0, 200); }
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

		[SerializeField] private GameObject activationEffect;

		[SerializeField] private GameObject feedbackEffect;

		public virtual void OnActivation(Shuriken shuriken)
		{
			if (activationEffect != null)
			{
				GameObject instance = (GameObject) Instantiate(activationEffect, this.transform.position, Quaternion.identity);
				instance.transform.parent = this.transform.parent;
			}

			GameScore.GetByPlayer(shuriken.lastHitOwner).plusFortHit();

			Destroy(this.gameObject);
		}

		public void placeFeedback(GameObject gameObject)
		{
			if (feedbackEffect != null)
			{
				GameObject f = (GameObject)Instantiate(feedbackEffect, gameObject.transform.position, Quaternion.identity);
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

