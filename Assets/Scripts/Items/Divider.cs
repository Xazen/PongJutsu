using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Divider : Item
	{
		public GameObject shotObject;
		public int splits = 2;
		public float angularDistance = 1f;
		public float damagePercentage = 0.5f;

		public override void content(Collider2D col)
		{
			base.content(col);

			for (int i = 0; splits > i; i++)
			{
				GameObject shotInstance = (GameObject) Instantiate(shotObject, this.transform.position, new Quaternion());
				shotInstance.GetComponent<Shot>().owner = col.GetComponent<Shot>().owner;
				shotInstance.GetComponent<Shot>().damage = (int)(col.GetComponent<Shot>().damage * damagePercentage);

				float movementY;
				if (splits % 2 == 0)
					 movementY = i * angularDistance - angularDistance * (splits / 2f) + angularDistance / 2f;
				else
					movementY = i * angularDistance - angularDistance * ((splits -1f) / 2f);
				shotInstance.GetComponent<Shot>().setInitialMovement(col.GetComponent<Shot>().owner.GetComponent<Player>().direction, movementY);
			}

			Destroy(col.gameObject);
		}
	}
}
