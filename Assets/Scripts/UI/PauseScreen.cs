using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PauseScreen : UIScript
	{
		public void click_Resume()
		{
			GameManager.ResumeGame();
		}
		
		public void click_Help()
		{

		}

		public void click_Exit()
		{
			GameManager.ExitGame();
		}
	}
}
