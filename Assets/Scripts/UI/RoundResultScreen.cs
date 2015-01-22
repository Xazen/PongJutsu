using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class RoundResultScreen : UIScript
	{
		public GameObject RoundEndObjectContainer;
		public GameObject MatchEndObjectContainer;

		private bool isMatchEnd = false;

		void OnEnable()
		{
			if (GameMatch.getMatchWinner() != null)
			{
				RoundEndObjectContainer.SetActive(false);
				MatchEndObjectContainer.SetActive(true);
				isMatchEnd = true;
			}
			else
			{
				RoundEndObjectContainer.SetActive(true);
				MatchEndObjectContainer.SetActive(false);
				isMatchEnd = false;
			}

			this.setDefaultSelection();
		}

		void Update()
		{
			if (isMatchEnd && Input.anyKeyDown)
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
