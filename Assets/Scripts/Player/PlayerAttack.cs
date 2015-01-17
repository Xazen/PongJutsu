using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerAttack : MonoBehaviour
	{

		public float firerate = 1.5f;
		private float nextFire;
		public float angle = 3f;

		public float damageMultiplier = 1.0f;
		public float speedMultiplier = 1.0f;

		public int maxActiveShots = 1;
		[HideInInspector] public int shotCount = 0;

		public GameObject shotObject;

		int direction;

		void Start()
		{
			nextFire = firerate;
		}

		void Update()
		{
			if (GameManager.allowInput)
			{
				Shooting();
			}
		}

		void Shooting()
		{
			nextFire += Time.deltaTime;
			if (nextFire >= firerate && shotCount < maxActiveShots)
			{
				if (Input.GetButton(this.transform.parent.tag + " shoot straight"))
				{
					triggerShoot(0);
				}
				else if (Input.GetButton(this.transform.parent.tag + " shoot up"))
				{
					triggerShoot(1);
				}
				else if (Input.GetButton(this.transform.parent.tag + " shoot down"))
				{
					triggerShoot(-1);
				}
			}
		}

		void triggerShoot(int dir)
		{
			// Trigger Animation... wait for throw
			this.transform.parent.GetComponentInChildren<Animator>().SetTrigger("Shoot");
			direction = dir;
			nextFire = 0;
		}

		public void Shoot()
		{
			// Create a new shot
			GameObject shotInstance = (GameObject)Instantiate(shotObject, this.transform.position, Quaternion.identity);
			Shuriken shuriken = shotInstance.GetComponent<Shuriken> ();
			shuriken.owner = this.transform.parent.gameObject;
			shuriken.GetComponent<Shuriken>().setInitialMovement(this.GetComponentInParent<Player>().direction, angle * direction);
			shuriken.damage = Mathf.RoundToInt((float)shuriken.damage*damageMultiplier);
			shuriken.speed *= speedMultiplier;
			this.audio.Play();

			direction = 0;
		}
	}
}
