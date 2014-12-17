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

		public GameObject itemCarrier;
		public GameObject[] items = new GameObject[0];

		private List<GameObject> spawnedItems = new List<GameObject>();

		void Start()
		{
			nextSpawn = spawnFrequency + Random.Range(-frequencyRandomizer, frequencyRandomizer);
		}

		void Update()
		{
			nextSpawn -= Time.deltaTime;
			if (nextSpawn <= 0)
			{
				spawnItem();
				nextSpawn = spawnFrequency + Random.Range(-frequencyRandomizer, frequencyRandomizer);
			}
		}

		void spawnItem()
		{
			// Set random position and item
			int r = Random.Range(0, items.Length);
			float xRange = width - itemCarrier.GetComponent<BoxCollider2D>().size.x;
			float x = Random.Range(-xRange / 2, xRange / 2);
			float y = (Mathf.Sign(flowSpeed) * -1 * height + itemCarrier.GetComponent<BoxCollider2D>().size.y) / 2;

			// Create carrier
			GameObject carrier = (GameObject)Instantiate(itemCarrier, new Vector2(x, y), new Quaternion());
			carrier.GetComponent<ItemCarrier>().instantiateItem(items[r]);
			carrier.GetComponent<ItemCarrier>().setVerticalSpeed(flowSpeed);
			carrier.transform.parent = this.transform;

			spawnedItems.Add(carrier);
		}

		void OnTriggerExit2D(Collider2D col)
		{
			// Destroy items when they leave the river
			if (col.tag == "Carrier")
			{
				Destroy(col.gameObject);
			}
		}

		void OnDrawGizmos()
		{
			Gizmos.color = new Color(0.4f, 0.4f, 0.7f, 0.4f);

			if (snapToVertical)
			{
				height = Camera.main.orthographicSize * 2;
				snapToVertical = false;
			}

			Gizmos.DrawCube(new Vector2(0, 0), new Vector2(width, height));
		}
	}
}
