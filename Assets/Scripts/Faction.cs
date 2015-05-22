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
		int d = 0;

		if (faction == Faction.Left)
			d = 1;
		else if (faction == Faction.Right)
			d = -1;

		return d;
	}

	public static Quaternion Rotation2D(this Faction faction, int additionalRotation = 0)
	{
		int r = 0;

		if (faction == Faction.Left)
			r = 0;
		else if (faction == Faction.Right)
			r = 180;

		return Quaternion.Euler(0f, r + additionalRotation, 0f);
	}

	public static string ToTag(this Faction faction, string name)
	{
		string tag = "";

		if (faction == Faction.Left)
			tag = name + "Left";
		else if (faction == Faction.Right)
			tag = name + "Right";

		return tag;
	}
}
