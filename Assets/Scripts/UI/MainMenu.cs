using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class MainMenu : UIScript
	{
		public void click_Start()
		{
			ui.SetTrigger("StartGame");
			game.LoadGame();
		}
		public void click_Options()
		{

		}
		public void click_Credits()
		{

		}
		public void click_Help()
		{

		}
		public void click_Exit()
		{
			Application.Quit();
		}
	}
}
