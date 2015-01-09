using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class River : MonoBehaviour
	{
		public float width = 1.7f;
		public float height = 11.6f;
		public bool snapToVertical = false;

		public float spawnFrequency = 5f;
		public float frequencyRandomizer = 2f;
		private float nextSpawn;

		public float flowSpeed = -1f;

		[SerializeField] private GameObject itemCarrier;

		[SerializeField] private GameObject[] items = new GameObject[0];
		public Dictionary<string, Item> itemList = new Dictionary<string, Item>();

		private List<GameObject> spawnedItems = new List<GameObject>();

		void Awake()
		{
			foreach (GameObject item in items)
				itemList.Add(item.name, item.GetComponent<Item>());

			this.GetComponent<BoxCollider2D>().size = new Vector2(width, height);

			setNextSpawn();
		}

		void Update()
		{
			if (!GameManager.isPause)
			{
				checkSpawn();
			}
		}

		void setNextSpawn()
		{
			nextSpawn = spawnFrequency + Random.Range(-frequencyRandomizer, frequencyRandomizer);
		}

		void checkSpawn()
		{
			nextSpawn -= Time.deltaTime;
			if (nextSpawn <= 0)
			{
				SpawnItem();
				setNextSpawn();
			}
		}

		void SpawnItem()
		{
			// Set random item
			GameObject item = randomItem();

			// Set random position
			float xRange = width - itemCarrier.GetComponent<BoxCollider2D>().size.x;
			float x = Random.Range(-xRange / 2, xRange / 2);
			float y = (Mathf.Sign(flowSpeed) * -1 * height + itemCarrier.GetComponent<BoxCollider2D>().size.y) / 2;

			// Create carrier
			GameObject carrier = (GameObject)Instantiate(itemCarrier, new Vector2(x, y), Quaternion.identity);
			carrier.GetComponent<ItemCarrier>().instantiateItem(item);
			carrier.GetComponent<ItemCarrier>().setVerticalSpeed(flowSpeed);
			carrier.transform.parent = this.transform;

			spawnedItems.Add(carrier);
		}

		// Return a random item based on spawn probabilities
		GameObject randomItem()
		{
			int probabilitySum = 0;
			foreach (GameObject item in items)
			{
				probabilitySum += item.GetComponent<Item>().spawnProbability;
			}

			int r = Random.Range(0, probabilitySum);

			int cumulative = 0;
			foreach (GameObject item in items)
			{
				cumulative += item.GetComponent<Item>().spawnProbability;
				if (r < cumulative)
				{
					return item;
				}
			}

			return null;
		}

		void OnTriggerExit2D(Collider2D col)
		{
			// Destroy items when they leave the river
			if (col.tag == "Carrier")
			{
				Destroy(col.gameObject);
			}
		}
	}
}
