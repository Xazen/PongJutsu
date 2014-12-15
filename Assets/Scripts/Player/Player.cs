using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Player : MonoBehaviour
	{
		public AnimatorOverrideController ninjaLeftController;
		public AnimatorOverrideController ninjaRightController;

		public Sprite shieldLeftSprite;
		public Sprite shieldRightSprite;

		public bool mirror = false;
		[HideInInspector] public int direction = 1;


		void Start()
		{
			if (mirror)
			{
				// Mirror the Player
				direction = -1;
				Vector3 scale = this.transform.localScale;
				this.transform.localScale = new Vector3(scale.x * -1, scale.y);
			}

			// set different sprites for each player
			if (this.tag == "PlayerLeft")
			{
				this.GetComponent<Animator>().runtimeAnimatorController = ninjaLeftController;
				this.transform.FindChild("Shield").GetComponent<SpriteRenderer>().sprite = shieldLeftSprite;
			}
			else if (this.tag == "PlayerRight")
			{
				this.GetComponent<Animator>().runtimeAnimatorController = ninjaRightController;
				this.transform.FindChild("Shield").GetComponent<SpriteRenderer>().sprite = shieldRightSprite;
			}
		}

		// --- Forward animation event (ae) ---
		public void ae_Shoot()
		{
			this.GetComponentInChildren<PlayerAttack>().Shoot();
		}
	}
}

