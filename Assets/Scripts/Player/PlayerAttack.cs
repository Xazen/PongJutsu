using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerAttack : MonoBehaviour
	{

		public float firerate = 1.5f;
		private float nextFire;

		public float maxAngle = 3.5f;

		public GameObject shotObject;
		public int maxActiveShots = 1;
		[HideInInspector] public int shotCount = 0;

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
			if (nextFire >= firerate && shotCount < maxActiveShots && Input.GetButton(this.transform.parent.tag + " shoot"))
			{
				triggerShoot();
			}
		}

		void triggerShoot()
		{
			// Trigger Animation... wait for throw
			this.transform.parent.GetComponentInChildren<Animator>().SetTrigger("Shoot");
			nextFire = 0;
		}

		public void Shoot()
		{
			// Create a new shot
			GameObject shotInstance = (GameObject)Instantiate(shotObject, this.transform.position, Quaternion.identity);
			shotInstance.GetComponent<Shuriken>().owner = this.transform.parent.gameObject;
			shotInstance.GetComponent<Shuriken>().setInitialMovement(this.GetComponentInParent<Player>().direction, this.GetComponentInParent<PlayerMovement>().movementNormalized * maxAngle);
			this.audio.Play();
		}
	}
}
