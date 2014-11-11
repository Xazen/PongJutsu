using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerAttack : MonoBehaviour
	{

		public float firerate = 1.5f;
		private float nextFire = 0f;

		public int maxActiveShots = 1;
		[HideInInspector] public int shotCount = 0;

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
				if (nextFire >= firerate && shotCount < maxActiveShots)
				{
					Shoot();
					nextFire = 0;
					shotCount++;
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
