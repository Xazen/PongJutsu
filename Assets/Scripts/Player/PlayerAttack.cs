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
		private bool waitForShot = false;

		public GameObject shotObject;
		public GameObject shotSonic;

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
			Shuriken shuriken = shotInstance.GetComponent<Shuriken>();
			shuriken.owner = this.transform.parent.gameObject;
			shuriken.speed *= speedMultiplier;
			shuriken.GetComponent<Shuriken>().setInitialMovement(this.GetComponentInParent<Player>().direction, angle * direction);
			shuriken.damage = Mathf.RoundToInt((float)shuriken.damage*damageMultiplier);
			this.audio.Play();

			GameObject sonicInstance = (GameObject)Instantiate(shotSonic, this.transform.position, this.transform.rotation);

			if (this.transform.parent.tag == "PlayerLeft")
				sonicInstance.GetComponent<ShurikenSonic>().setColor(shotInstance.GetComponentInChildren<Shuriken>().shurikenLeftColor);
			else if (this.transform.parent.tag == "PlayerRight")
				sonicInstance.GetComponent<ShurikenSonic>().setColor(shotInstance.GetComponentInChildren<Shuriken>().shurikenRightColor);

			waitForShot = false;
			direction = 0;
		}
	}
}
