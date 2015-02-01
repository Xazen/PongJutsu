using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PauseScreen : UIScript
	{
		public override void uiEnable()
		{
			base.uiEnable();

			GameManager.allowPauseSwitch = true;
		}

		public void click_Resume()
		{
			GameManager.ResumeGame();
		}
		
		public void click_Help()
		{
			ui.SetTrigger("Help");
			GameManager.allowPauseSwitch = false;
		}

		public void click_Exit()
		{
			GameManager.ExitGame();
		}
	}
}
