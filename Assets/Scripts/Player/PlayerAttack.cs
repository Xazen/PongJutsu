using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerAttack : MonoBehaviour
	{

		public float firerate = 1.5f;
		private float nextFire = 0f;
		public float angle = 3f;

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
			if (nextFire >= firerate && shotCount < maxActiveShots)
			{
				if (Input.GetButton(this.transform.parent.tag + " shoot forward"))
				{
					Shoot(0);
				}
				else if (Input.GetButton(this.transform.parent.tag + " shoot up"))
				{
					Shoot(1);
				}
				else if (Input.GetButton(this.transform.parent.tag + " shoot down"))
				{
					Shoot(-1);
				}
			}
		}

		private void Shoot(int dir)
		{
			// Create a new shot
			GameObject shotInstance = (GameObject) Instantiate(shotObject, this.transform.position, new Quaternion());
			shotInstance.GetComponent<Shuriken>().owner = this.transform.parent.gameObject;
			shotInstance.GetComponent<Shuriken>().setInitialMovement(this.GetComponentInParent<Player>().direction, angle * dir);

			nextFire = 0;

			this.transform.parent.GetComponentInChildren<Animator>().SetTrigger("Shoot");
			this.audio.Play();
		}
	}
}
