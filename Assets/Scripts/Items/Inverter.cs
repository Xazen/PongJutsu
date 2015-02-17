using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Inverter : Item
	{
		public override void OnActivation(Shuriken shuriken)
		{
			GameObject affectedPlayer = null;

			if (shuriken.lastHitOwner.tag == GameVar.players.left.gameObject.tag)
				affectedPlayer = GameVar.players.right.gameObject;
			else if (shuriken.lastHitOwner.tag == GameVar.players.right.gameObject.tag)
				affectedPlayer = GameVar.players.left.gameObject;

			affectedPlayer.GetComponent<PlayerItemHandler>().Inverter(this);
			placeFeedback(affectedPlayer);

			base.OnActivation(shuriken);
		}
	}
}
