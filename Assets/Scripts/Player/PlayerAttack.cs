using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerAttack : MonoBehaviour
	{

		public float firerate = 1.5f;
		private float nextFire;
		public float angle = 3f;

		public GameObject shotObject;
		public int maxActiveShots = 1;
		[HideInInspector] public int shotCount = 0;
		private bool waitForShot = false;

		int direction;

		public void Setup()
		{
			nextFire = firerate;
		}

		void Update()
		{
			setGlow();

			if (GameManager.allowInput)
				Shooting();
		}

		void setGlow()
		{
			if (shotCount < maxActiveShots)
			{
				if (this.transform.parent.FindChild("Ninja").FindChild("Glow").gameObject.activeSelf == false)
					this.transform.parent.FindChild("Ninja").FindChild("Glow").gameObject.SetActive(true);
			}
			else
			{
				if (this.transform.parent.FindChild("Ninja").FindChild("Glow").gameObject.activeSelf == true)
					this.transform.parent.FindChild("Ninja").FindChild("Glow").gameObject.SetActive(false);
			}
		}

		void Shooting()
		{
			nextFire += Time.deltaTime;
			if (nextFire >= firerate && shotCount < maxActiveShots && !waitForShot)
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

			nextFire = 0;
			waitForShot = true;
			direction = dir;			
		}

		public void Shoot()
		{
			// Create a new shot
			GameObject shotInstance = (GameObject)Instantiate(shotObject, this.transform.position, Quaternion.identity);
			shotInstance.GetComponent<Shuriken>().owner = this.transform.parent.gameObject;
			shotInstance.GetComponent<Shuriken>().setInitialMovement(this.GetComponentInParent<Player>().direction, angle * direction);
			this.audio.Play();

			waitForShot = false;
			direction = 0;
		}
	}
}
