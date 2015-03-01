using UnityEngine;
using System.Collections;

public class Credits : UIScript
{
	public override void uiUpdate()
	{
		base.uiUpdate();

		if (Input.anyKeyDown)
			ui.SetTrigger("Back");
	}
}
