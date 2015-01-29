using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PongJutsu
{
	public class HelpScreen : UIScript
	{
		public override void UIpdate()
		{
			base.UIpdate();

			if (Input.anyKeyDown)
				ui.SetTrigger("Back");
		}
	}
}
