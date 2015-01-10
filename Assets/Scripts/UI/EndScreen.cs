using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class EndScreen : UIScript
	{
		public void click_Exit()
		{
			GameManager.ExitGame();
		}
	}
}
