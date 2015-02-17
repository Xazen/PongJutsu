using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerShield : PlayerBase
	{
		Animator _Animator;
		public Animator Animator
		{
			get
			{
				if (_Animator == null)
					_Animator = shieldReference.GetComponent<Animator>();

				return _Animator;
			}
		}

		[SerializeField]
		private float shieldAngleMultiplier = 5f;

		public GameObject shieldReference;
		public GameObject expanderReference;

		void OnTriggerEnter2D(Collider2D col)
		{
			if (col.gameObject.tag == "Shuriken")
			{
				// Get Shuriken Script and GameObject
				Shuriken shuriken = col.GetComponent<Shuriken>();
				GameObject shurikenGameObject = col.gameObject;

				// Reflect
				if (shuriken.owner != this.gameObject)
				{
					shuriken.movement.x = -shuriken.movement.x;

					float a = shurikenGameObject.transform.position.y - this.transform.position.y;
					float b = this.transform.localScale.y * this.GetComponent<BoxCollider2D>().size.y;
					float c = a / (b * 0.5f);

					shuriken.movement.y = c * shieldAngleMultiplier;
					shuriken.adjustSpeed();

					shieldReference.GetComponent<SoundPool>().PlayElement(0);
					Animator.SetTrigger("Reflect");

					shuriken.lastHitOwner = this.gameObject;
					shuriken.bounceBack = true;

					Player.addCombo();
					GameScore.GetByPlayer(this.gameObject).reflections += 1;
				}
				// Catch
				else if (shuriken.owner == this.gameObject && shuriken.bounceBack)
				{
					shieldReference.GetComponent<SoundPool>().PlayElement(1);
					Animator.SetTrigger("Catch");

					Player.addCombo();
					GameScore.GetByPlayer(this.gameObject).catches += 1;

					shurikenGameObject.GetComponent<Shuriken>().Remove();
				}
			}
		}
	}
}
