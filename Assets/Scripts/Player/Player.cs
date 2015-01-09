using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Player : MonoBehaviour
	{
		public AnimatorOverrideController ninjaLeftController;
		public AnimatorOverrideController ninjaRightController;

		public bool mirror = false;
		[HideInInspector] public int direction = 1;

		[HideInInspector] public int comboCount = 0;


		void Start()
		{
			if (mirror)
			{
				// Mirror the Player
				direction = -1;
				Vector3 scale = this.transform.localScale;
				this.transform.localScale = new Vector3(scale.x * -1, scale.y);
			}

			// Set different sprites for each player
			if (this.tag == "PlayerLeft")
				this.GetComponent<Animator>().runtimeAnimatorController = ninjaLeftController;
			else if (this.tag == "PlayerRight")
				this.GetComponent<Animator>().runtimeAnimatorController = ninjaRightController;
		}

		public void addCombo()
		{
			comboCount++;
		}
		public void resetCombo()
		{
			comboCount = 0;
		}

		// --- Forward animation event (ae) ---
		public void ae_Shoot()
		{
			this.GetComponentInChildren<PlayerAttack>().Shoot();
		}
	}
}

