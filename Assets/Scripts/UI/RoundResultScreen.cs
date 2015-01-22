using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class RoundResultScreen : UIScript
	{
		public GameObject RoundEndObjectContainer;
		public GameObject MatchEndObjectContainer;

		void OnEnable()
		{
			if (GameMatch.getMatchWinner() != null)
			{
				RoundEndObjectContainer.SetActive(false);
				MatchEndObjectContainer.SetActive(true);
			}
			else
			{
				RoundEndObjectContainer.SetActive(true);
				MatchEndObjectContainer.SetActive(false);
			}

			this.setDefaultSelection();
		}

		public void click_Continue()
		{
			GameManager.EndGame();
		}

		public void click_Exit()
		{
			GameManager.ExitGame();
		}

		public void click_Next()
		{
			GameManager.NextRound();
		}
	}
}
