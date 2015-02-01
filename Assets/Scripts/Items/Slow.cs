using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Slow : Item
	{
		public float speedMuliplier = 0.4f;

		public bool slowDownWinningPlayerOnly = true;

		public override void OnActivation(Shuriken shuriken)
		{
			GameObject affectedPlayer = null;

			if (slowDownWinningPlayerOnly && GameVar.forts.leftCount != GameVar.forts.rightCount)
			{
				if (GameVar.forts.leftCount > GameVar.forts.rightCount)
					affectedPlayer = GameVar.players.left.reference;
				else if (GameVar.forts.rightCount > GameVar.forts.leftCount)
					affectedPlayer = GameVar.players.right.reference;
			}
			else
			{
				if (shuriken.lastHitOwner.tag == GameVar.players.left.reference.tag)
					affectedPlayer = GameVar.players.right.reference;
				else if (shuriken.lastHitOwner.tag == GameVar.players.right.reference.tag)
					affectedPlayer = GameVar.players.left.reference;
			}

			affectedPlayer.GetComponent<PlayerItemHandler>().Slow(this);
			placeFeedback(affectedPlayer);

			base.OnActivation(shuriken);
		}
	}
}
