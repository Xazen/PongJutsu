﻿using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Bomb : Item
	{

		public float damageMultiplier = 0.5f;

		public override void OnActivation(Shuriken shuriken)
		{
			if (!shuriken.isBomb)
			{
				shuriken.isBomb = true;
				shuriken.damage = Mathf.RoundToInt(shuriken.damage * damageMultiplier);
			}

			base.OnActivation(shuriken);
		}
	}
}
