using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public enum PlayerSide
	{
		Left,
		Right
	}

	public class Player : PlayerBase
	{
		PlayerSide _playerSide;
		public PlayerSide playerSide
		{
			get
			{
				return _playerSide;
			}
			set
			{
				_playerSide = value;

				if (_playerSide == PlayerSide.Left)
				{
					direction = 1;
					this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

					this.GetComponent<Animator>().runtimeAnimatorController = ninjaLeftController;
					PlayerShield.shieldReference.GetComponent<SpriteRenderer>().sprite = shieldLeftSprite;
					PlayerShield.shieldReference.GetComponent<Animator>().runtimeAnimatorController = shieldLeftController;

					forts = GameObject.FindGameObjectsWithTag("FortLeft");
				}
				else if (_playerSide == PlayerSide.Right)
				{
					direction = -1;
					this.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

					this.GetComponent<Animator>().runtimeAnimatorController = ninjaRightController;
					PlayerShield.shieldReference.GetComponent<SpriteRenderer>().sprite = shieldRightSprite;
					PlayerShield.shieldReference.GetComponent<Animator>().runtimeAnimatorController = shieldRightController;

					forts = GameObject.FindGameObjectsWithTag("FortRight");
				}
			}
		}

		public AnimatorOverrideController ninjaLeftController;
		public AnimatorOverrideController ninjaRightController;

		public AnimatorOverrideController shieldLeftController;
		public AnimatorOverrideController shieldRightController;

		public Sprite shieldLeftSprite;
		public Sprite shieldRightSprite;

		[HideInInspector] public GameObject[] forts;

		[HideInInspector] public int direction = 1;

		[HideInInspector] public int comboCount = 0;
		[SerializeField] private float resetComboTime = 4.0f;
		private float passedTimeSinceCombo = 0.0f;

		public void Update()
		{
			if (comboCount > 0) 
			{
				passedTimeSinceCombo += Time.deltaTime;

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
}

