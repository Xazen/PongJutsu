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
			if (Input.GetButton(this.transform.parent.tag + " shoot"))
			{
				if (nextFire >= firerate && shotCount < maxActiveShots)
				{
					Shoot();
					nextFire = 0;
				}
			}
		}

		private void Shoot()
		{
			// Create a new shot
			GameObject shotInstance = (GameObject) Instantiate(shotObject, this.transform.position, new Quaternion());
			shotInstance.GetComponent<Shot>().owner = this.transform.parent.gameObject;
			shotInstance.GetComponent<Shot>().setInitialMovement(this.GetComponentInParent<Player>().direction, 0);
		}
	}
}
