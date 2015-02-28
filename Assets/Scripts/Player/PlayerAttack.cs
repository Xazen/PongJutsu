using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerAttack : PlayerBase
	{

		public float firerate = 1.5f;
		private float nextFire;

		public float maxAngle = 3.5f;

		[HideInInspector]
		public float damageMultiplier = 1.0f;
		[HideInInspector]
		public float speedMultiplier = 1.0f;

		public int maxActiveShots = 1;
		[HideInInspector] public int shotCount = 0;

		[SerializeField]
		private GameObject shotPrefab;
		[SerializeField]
		private GameObject shotSonicPrefab;

		[SerializeField]
		private GameObject glowReference;
		[SerializeField]
		private AudioSource attackAudioReference;
		[SerializeField]
		private SoundPool rageAttackSoundpoolReference;

		public void Start()
		{
			nextFire = firerate;
		}

		void FixedUpdate()
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
			nextFire += Time.fixedDeltaTime;
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
