using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class RoundResultScreen : UIScript
	{
		public GameObject RoundEndElements;
		public GameObject MatchEndElements;

		private bool isMatchEnd = false;

		void OnEnable()
		{
			if (GameMatch.getMatchWinner() != null)
			{
				RoundEndElements.SetActive(false);
				MatchEndElements.SetActive(true);
				isMatchEnd = true;
			}
			else
			{
				RoundEndElements.SetActive(true);
				MatchEndElements.SetActive(false);
				isMatchEnd = false;
			}

			this.setDefaultSelection();
		}

		public override void UIpdate()
		{
			base.UIpdate();

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
