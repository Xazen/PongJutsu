using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Player : MonoBehaviour
	{
		public AnimatorOverrideController ninjaLeftController;
		public AnimatorOverrideController ninjaRightController;

		[HideInInspector] public GameObject[] forts;

		public bool mirror = false;
		[HideInInspector] public int direction = 1;

		[HideInInspector] public int comboCount = 0;


		public void Setup()
		{
			if (mirror)
			{
				// Mirror the Player
				direction = -1;
				this.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
			}

			// Setup different players
			if (this.tag == "PlayerLeft")
			{
				this.GetComponent<Animator>().runtimeAnimatorController = ninjaLeftController;
				forts = GameObject.FindGameObjectsWithTag("FortLeft");
			}
			else if (this.tag == "PlayerRight")
			{
				this.GetComponent<Animator>().runtimeAnimatorController = ninjaRightController;
				forts = GameObject.FindGameObjectsWithTag("FortRight");
			}
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

