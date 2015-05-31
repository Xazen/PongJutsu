using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class Player : PlayerBase
{
	Faction _faction;
	public Faction faction
	{
		get
		{
			return _faction;
		}
		set
		{
			_faction = value;

			direction = _faction.Direction();
			transform.rotation = _faction.Rotation2D();
			tag = _faction.ToTag("Player");
			name = tag;

			if (_faction == Faction.Left)
			{
				Animator.runtimeAnimatorController = ninjaLeftController;
				PlayerShield.shieldReference.GetComponent<SpriteRenderer>().sprite = shieldLeftSprite;
				PlayerShield.Animator.runtimeAnimatorController = shieldLeftController;
			}
			else if (_faction == Faction.Right)
			{
				Animator.runtimeAnimatorController = ninjaRightController;
				PlayerShield.shieldReference.GetComponent<SpriteRenderer>().sprite = shieldRightSprite;
				PlayerShield.Animator.runtimeAnimatorController = shieldRightController;
			}
		}
	}

	Animator _Animator;
	public Animator Animator
	{
		get
		{
			if (_Animator == null)
				_Animator = this.GetComponent<Animator>();

			return _Animator;
		}
	}

	[SerializeField]
	private AnimatorOverrideController ninjaLeftController;
	[SerializeField]
	private AnimatorOverrideController ninjaRightController;

	[SerializeField]
	private AnimatorOverrideController shieldLeftController;
	[SerializeField]
	private AnimatorOverrideController shieldRightController;

	[SerializeField]
	private Sprite shieldLeftSprite;
	[SerializeField]
	private Sprite shieldRightSprite;

	private GameObject[] _forts;
	[HideInInspector]
	public GameObject[] forts
	{
		get
		{
			if (_forts == null)
			{
				if (faction == Faction.Left)
					_forts = GameObject.FindGameObjectsWithTag("FortLeft");
				else if (faction == Faction.Right)
					_forts = GameObject.FindGameObjectsWithTag("FortRight");
			}

			return _forts;
		}
	}

	[HideInInspector]
	public int direction = 1;

	[HideInInspector]
	public int comboCount = 0;
	[SerializeField]
	private float resetComboTime = 4.0f;
	private float passedTimeSinceCombo = 0.0f;

	void Awake()
	{
		object[] initData = GetComponent<PhotonView>().instantiationData;
		faction = (Faction)	initData[0];

		if (MultiplayerManager.onlineMode && !MultiplayerManager.CanControlFaction(faction))
		{
			Player.enabled = false;
			PlayerMovement.enabled = true;
			PlayerAttack.enabled = true;
			PlayerShield.enabled = false;
			PlayerInput.enabled = false;
			PlayerItemHandler.enabled = false;
		}
	}

	void FixedUpdate()
	{
		if (comboCount > 0)
		{
			passedTimeSinceCombo += Time.fixedDeltaTime;

			if (passedTimeSinceCombo >= resetComboTime)
			{
				resetCombo();
			}
		}
	}

	public void addCombo()
	{
		passedTimeSinceCombo = 0.0f;

		if (GameManager.allowInput)
			comboCount++;
	}
	public void resetCombo()
	{
		comboCount = 0;
	}
}
