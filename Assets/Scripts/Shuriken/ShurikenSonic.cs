using UnityEngine;
using System.Collections;

public class ShurikenSonic : Destructor
{
	[SerializeField]
	private AnimatorOverrideController sonicLeftController;
	[SerializeField]
	private AnimatorOverrideController sonicRightController;

	public void Set(Faction ownerFaction)
	{
		if (ownerFaction == Faction.Left)
			this.GetComponent<Animator>().runtimeAnimatorController = sonicLeftController;
		else if (ownerFaction == Faction.Right)
			this.GetComponent<Animator>().runtimeAnimatorController = sonicRightController;
	}
}
