using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class ItemCarrier : MonoBehaviour
	{
		private float vSpeed;
		private float bound;

		public void setVerticalSpeed(float verticalSpeed)
		{
			vSpeed = verticalSpeed;
		}

		public void instantiateItem(GameObject itemPrefab)
		{
			GameObject item = (GameObject)Instantiate(itemPrefab, this.transform.position, new Quaternion());
			item.transform.parent = this.transform;
			this.name = "Carrier (" + itemPrefab.name + ")";
		}

		void Update()
		{
			this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + vSpeed * Time.deltaTime);
		}
	}
}
