using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Slow : Item
	{

		public float duration = 8f;
		public float speedMuliplier = 0.4f;

		public bool slowDownWinningPlayerOnly = true;

		public override void content(Shuriken shuriken)
		{
			if (slowDownWinningPlayerOnly && GameVar.forts.leftCount != GameVar.forts.rightCount)
			{
				if (GameVar.forts.leftCount > GameVar.forts.rightCount)
					GameVar.players.left.reference.GetComponent<PlayerItemHandler>().Slow(this);
				else if (GameVar.forts.rightCount > GameVar.forts.leftCount)
					GameVar.players.right.reference.GetComponent<PlayerItemHandler>().Slow(this);
			}
			else
			{
				if (shuriken.lastHitOwner.tag == GameVar.players.left.reference.tag)
					GameVar.players.right.reference.GetComponent<PlayerItemHandler>().Slow(this);
				else if (shuriken.lastHitOwner.tag == GameVar.players.right.reference.tag)
					GameVar.players.left.reference.GetComponent<PlayerItemHandler>().Slow(this);
			}

			base.content(shuriken);
		}
	}
}
