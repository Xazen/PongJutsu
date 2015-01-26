using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Credits : UIScript
	{
		public override void UIpdate()
		{
			base.UIpdate();

			if (Input.anyKeyDown)
				ui.SetTrigger("MainMenu");			
		}
	}

}
