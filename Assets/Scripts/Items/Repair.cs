﻿using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Repair : Item
	{
		public int heal = 100;

		public override void content(Shuriken shuriken)
		{
			GameObject weakestFort = null;
			foreach (GameObject fort in shuriken.lastHitOwner.GetComponent<Player>().forts)
			{
				int health = fort.GetComponent<Fort>().health;

				if (weakestFort != null)
				{
					if (health < weakestFort.GetComponent<Fort>().health && health > 0)
						weakestFort = fort;
				}
				else
				{
					if (health > 0)
						weakestFort = fort;
				}
			}

			if (weakestFort != null)
				weakestFort.GetComponent<Fort>().TakeHeal(heal);

			base.content(shuriken);
		}
	}
}