using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerAttack : MonoBehaviour
	{

		public float firerate = 1.5f;
		private float nextFire = 0f;

		public GameObject shotObject;


		void Update()
		{
			Fire();
		}

		void Fire()
		{
			nextFire += Time.deltaTime;
			if (Input.GetButton(this.transform.parent.tag + "_shoot"))
			{
				if (nextFire >= firerate)
				{
					Shoot();
					nextFire = 0;
				}
			}
		}

		private void Shoot()
		{
			// Create a new shot
			GameObject shotInstance = shotObject;
			shotInstance.GetComponent<Shot>().direction = this.GetComponentInParent<Player>().direction;
			shotInstance.GetComponent<Shot>().owner = this.transform.parent.gameObject;
			Instantiate(shotInstance, this.transform.position, new Quaternion());
		}
	}
}
