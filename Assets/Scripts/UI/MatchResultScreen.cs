using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class MatchResultScreen : UIScript
	{
		public void click_Exit()
		{
			GameManager.ExitGame();
		}

		public void click_Restart()
		{
			GameManager.RestartGame();
		}
	}
}
