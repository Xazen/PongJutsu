using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PauseScreen : UIScript
	{
		public void click_Continue()
		{
			ui.SetTrigger("ContinueGame");
			game.ContinueGame();
		}
	}
}
