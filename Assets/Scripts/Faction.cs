using UnityEngine;
using System.Collections;

public enum Faction
{
	Left,
	Right
}

public static class FactionExtensions
{
	public static int Direction(this Faction faction)
	{
		int i = 0;

		if (faction == Faction.Left)
			i = 1;
		else if (faction == Faction.Right)
			i = -1;

		return i;
	}

	public static Quaternion Rotation2D(this Faction faction, int eulerOffset = 0)
	{
		int i = 0;

		if (faction == Faction.Left)
			i = 0;
		else if (faction == Faction.Right)
			i = 180;

		return Quaternion.Euler(0f, i + eulerOffset, 0f);
	}
}
