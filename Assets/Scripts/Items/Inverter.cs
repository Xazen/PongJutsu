using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Inverter : Item
	{
		public override void OnActivation(Shuriken shuriken)
		{
			GameObject affectedPlayer = null;

			if (shuriken.lastHitOwner.tag == GameVar.players.left.reference.tag)
				affectedPlayer = GameVar.players.right.reference;
			else if (shuriken.lastHitOwner.tag == GameVar.players.right.reference.tag)
				affectedPlayer = GameVar.players.left.reference;

			affectedPlayer.GetComponent<PlayerItemHandler>().Inverter(this);
			placeFeedback(affectedPlayer);

			base.OnActivation(shuriken);
		}
	}
}
