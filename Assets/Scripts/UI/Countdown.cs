﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PongJutsu
{
	public class Countdown : UIScript
	{
		private string defaultRoundText = "Round";

		void OnEnable()
		{
			this.GetComponent<SoundPool>().PlayElement(GameMatch.getWinnerList().Count);
			this.transform.FindChild("Round").GetComponent<Text>().text = defaultRoundText + " " + (GameMatch.getWinnerList().Count + 1);
		}
	}
}
