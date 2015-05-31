using UnityEngine;
using System.Collections;

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
	[HideInInspector]
	public int shotCount = 0;

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
		{
			if (photonView.isMine)
			{
				Shooting();
			}
		}
			
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
		if (nextFire >= firerate && PlayerInput.GetButton(Control.Shoot) && shotCount < maxActiveShots)
		{
			float initVerticalMovement = PlayerMovement.movementNormalized * maxAngle;
			object[] data = { Player.faction, Player.direction, initVerticalMovement, speedMultiplier, damageMultiplier };

			PhotonNetwork.Instantiate(shotPrefab.name, this.transform.position, Quaternion.identity, 0, data);

			photonView.RPC("OnShoot", PhotonTargets.All);
        }
	}

	[RPC]
	void OnShoot()
	{
		Player.Animator.SetTrigger("Shoot");

		attackAudioReference.Play();

		if (this.tag == "PlayerLeft" && GameFlow.instance.isDisadvantageBuffLeftPhase)
			rageAttackSoundpoolReference.PlayRandom();
		else if (this.tag == "PlayerRight" && GameFlow.instance.isDisadvantageBuffRightPhase)
			rageAttackSoundpoolReference.PlayRandom();

		GameObject sonicInstance = (GameObject)Instantiate(shotSonicPrefab, this.transform.position, this.transform.rotation);
		sonicInstance.GetComponent<ShurikenSonic>().Set(Player.faction);

		GameScore.GetByPlayer(this.gameObject).thrownshurikens += 1;
		nextFire = 0;
	}
}
