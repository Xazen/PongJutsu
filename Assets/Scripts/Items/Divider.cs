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

			// Split shuriken
			for (int i = 0; splits > i; i++)
			{
				GameObject shotInstance = (GameObject) Instantiate(shotObject, this.transform.position, new Quaternion());

				// Calculate y-movement
				float movementY;
				if (splits % 2 == 0)
					 movementY = i * angularDistance - angularDistance * (splits / 2f) + angularDistance / 2f;
				else
					movementY = i * angularDistance - angularDistance * ((splits -1f) / 2f);

				// Init values
				shotInstance.GetComponent<Shuriken>().owner = col.GetComponent<Shuriken>().owner;
				shotInstance.GetComponent<Shuriken>().damage = (int)(col.GetComponent<Shuriken>().damage * damagePercentage);
				shotInstance.GetComponent<Shuriken>().setInitialMovement((int)Mathf.Sign(col.GetComponent<Shuriken>().movement.x), col.GetComponent<Shuriken>().movement.y + movementY);
				shotInstance.GetComponent<Shuriken>().bounceBack = col.GetComponent<Shuriken>().bounceBack;
			}

			Destroy(col.gameObject);
			this.Remove();
		}
	}
}
