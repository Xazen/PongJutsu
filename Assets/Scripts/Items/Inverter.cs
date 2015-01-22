using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Inverter : Item
	{
		public float duration = 8f;

		public override void content(Shuriken shuriken)
		{
			if (shuriken.lastHitOwner.tag == GameVar.players.left.reference.tag)
				GameVar.players.right.reference.GetComponent<PlayerItemHandler>().Inverter(this);
			else if (shuriken.lastHitOwner.tag == GameVar.players.right.reference.tag)
				GameVar.players.left.reference.GetComponent<PlayerItemHandler>().Inverter(this);

			base.content(shuriken);
		}
	}
}
