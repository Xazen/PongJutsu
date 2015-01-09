﻿using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PauseScreen : UIScript
	{
		public void click_Resume()
		{
			game.ResumeGame();
		}
		
		public void click_Help()
		{

		}

		public void click_Exit()
		{
			game.ExitGame();
		}
	}
}
