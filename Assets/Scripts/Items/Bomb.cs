using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Bomb : Item
	{

		public float damageMultiplier = 0.5f;

		public override void content(Shuriken shuriken)
		{
			if (shuriken.lastHitOwner.tag == GameVar.players.left.reference.tag)
				damageForts(GameVar.players.right.reference.GetComponent<Player>().forts, shuriken);
			else if (shuriken.lastHitOwner.tag == GameVar.players.right.reference.tag)
				damageForts(GameVar.players.left.reference.GetComponent<Player>().forts, shuriken);

			base.content(shuriken);
		}

		private void damageForts(GameObject[] forts, Shuriken shuriken)
		{
			int damage = (int)(shuriken.damage * damageMultiplier);

			foreach (GameObject fort in forts)
			{
				fort.GetComponent<Fort>().TakeDamage(damage);
			}
		}
	}
}
