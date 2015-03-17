using UnityEngine;
using System.Collections;

public class Credits : UIBase
{
	public override void uiUpdate()
	{
		base.uiUpdate();

		if (Input.anyKeyDown)
			ui.SetTrigger("Back");
	}
}
