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
					this.transform.parent.GetComponentInChildren<Animator>().SetTrigger("Shoot");
					nextFire = 0;
				}
			}
		}

		public void Shoot()
		{
			// Create a new shot
			GameObject shotInstance = (GameObject) Instantiate(shotObject, this.transform.position, new Quaternion());
			shotInstance.GetComponent<Shuriken>().owner = this.transform.parent.gameObject;
			shotInstance.GetComponent<Shuriken>().lastHitOwner = this.transform.parent.gameObject;
			shotInstance.GetComponent<Shuriken>().setInitialMovement(this.GetComponentInParent<Player>().direction, 0);

			this.audio.Play();
		}
	}
}
