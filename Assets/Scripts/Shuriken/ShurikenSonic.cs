using UnityEngine;
using System.Collections;

public class ShurikenSonic : Destructor
{
	[SerializeField]
	private AnimatorOverrideController sonicLeftController;
	[SerializeField]
	private AnimatorOverrideController sonicRightController;

	public void setOwner(GameObject owner)
	{
		if (owner.tag == "PlayerLeft")
			this.GetComponent<Animator>().runtimeAnimatorController = sonicLeftController;
		if (owner.tag == "PlayerRight")
			this.GetComponent<Animator>().runtimeAnimatorController = sonicRightController;
	}
}
