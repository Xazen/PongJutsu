﻿using UnityEngine;
using System.Collections;

public class Bomb : Item
{

	public float damageMultiplier = 0.5f;

	public override void OnActivation(Shuriken shuriken)
	{
		if (!shuriken.isBomb)
			shuriken.activateBomb(damageMultiplier);

		base.OnActivation(shuriken);
	}
}
