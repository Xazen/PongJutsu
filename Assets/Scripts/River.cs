using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class River : MonoBehaviour
	{
		public float width = 1.7f;
		public float height = 11.6f;
		public bool snap = false;

		public float spawnFrequency = 5f;
		public float frequencyRandomizer = 2f;
		private float nextSpawn = 0f;

		public float flowSpeed = -1f;

		public GameObject itemCarrier;
		public GameObject[] items = new GameObject[0];

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
			int r = Random.Range(0, items.Length);
			float x = Random.Range(-width / 2, width / 2);
			float y = (Mathf.Sign(flowSpeed) * -1 * height) / 2;

			GameObject carrier = (GameObject)Instantiate(itemCarrier, new Vector2(x, y), new Quaternion());
			carrier.GetComponent<ItemCarrier>().instantiateItem(items[r]);
			carrier.GetComponent<ItemCarrier>().SetVerticalSpeed(flowSpeed);
			carrier.transform.parent = this.transform;
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.gray;

			if (snap)
			{
				height = Camera.main.orthographicSize * 2;
				snap = false;
			}

			Gizmos.DrawCube(new Vector2(0, 0), new Vector2(width, height));
		}
	}
}
