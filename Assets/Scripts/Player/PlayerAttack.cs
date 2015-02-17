using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerAttack : PlayerBase
	{

		public float firerate = 1.5f;
		private float nextFire;

		public float maxAngle = 3.5f;

		public float damageMultiplier = 1.0f;
		public float speedMultiplier = 1.0f;

		public int maxActiveShots = 1;
		[HideInInspector] public int shotCount = 0;

		public GameObject shotPrefab;
		public GameObject shotSonicPrefab;

		public GameObject glowReference;
		public AudioSource attackAudioReference;
		public SoundPool rageAttackSoundpoolReference;

		int direction;

		public void Start()
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
				if (glowReference.activeSelf == false)
					glowReference.SetActive(true);
			}
			else
			{
				if (glowReference.activeSelf == true)
					glowReference.SetActive(false);
			}
		}

		void Shooting()
		{
			nextFire += Time.deltaTime;
			if (nextFire >= firerate && Input.GetButton(this.tag + " shoot") && shotCount < maxActiveShots)
			{
				Shoot();
			}
		}

		void Shoot()
		{
			Player.Animator.SetTrigger("Shoot");

			GameObject shotInstance = (GameObject)Instantiate(shotPrefab, this.transform.position, Quaternion.identity);
			Shuriken shuriken = shotInstance.GetComponent<Shuriken>();
			shuriken.owner = this.gameObject;
			shuriken.speed *= speedMultiplier;
			shuriken.damage = Mathf.RoundToInt((float)shuriken.damage * damageMultiplier);
			shuriken.setInitialMovement(Player.direction, PlayerMovement.movementNormalized * maxAngle);

			attackAudioReference.Play();

			if (this.tag == "PlayerLeft" && GameFlow.instance.isDisadvantageBuffLeftPhase)
				rageAttackSoundpoolReference.PlayRandom();
			else if (this.tag == "PlayerRight" && GameFlow.instance.isDisadvantageBuffRightPhase)
				rageAttackSoundpoolReference.PlayRandom();

			GameObject sonicInstance = (GameObject)Instantiate(shotSonicPrefab, this.transform.position, this.transform.rotation);
			sonicInstance.GetComponent<ShurikenSonic>().setOwner(this.gameObject);

			GameScore.GetByPlayer(this.gameObject).thrownshurikens += 1;
			nextFire = 0;
		}
	}
}
