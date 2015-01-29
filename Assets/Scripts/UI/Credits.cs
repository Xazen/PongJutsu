using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Credits : UIScript
	{
		public override void uiUpdate()
		{
			base.uiUpdate();

			if (Input.anyKeyDown)
				ui.SetTrigger("Back");			
		}
	}

}
