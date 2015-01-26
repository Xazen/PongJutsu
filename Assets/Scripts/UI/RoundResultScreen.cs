using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PongJutsu
{
	public class RoundResultScreen : UIScript
	{
		public GameObject RoundEndElements;
		public GameObject MatchEndElements;

		public Sprite leftWinSprite;
		public Sprite leftLoseSprite;
		public Sprite rightWinSprite;
		public Sprite rightLoseSprite;

		private bool isMatchEnd = false;

		void OnEnable()
		{
			setResult();

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

		void setResult()
		{
			if (GameMatch.getLastWinner() == "left")
			{
				this.transform.FindChild("PlayerLeftResult").GetComponent<Image>().sprite = leftWinSprite;
				this.transform.FindChild("PlayerRightResult").GetComponent<Image>().sprite = rightLoseSprite;
				this.transform.FindChild("WinnerGlow").position = new Vector2(this.transform.FindChild("PlayerLeftResult").position.x, this.transform.FindChild("WinnerGlow").position.y);
			}
			else if (GameMatch.getLastWinner() == "right")
			{
				this.transform.FindChild("PlayerLeftResult").GetComponent<Image>().sprite = leftLoseSprite;
				this.transform.FindChild("PlayerRightResult").GetComponent<Image>().sprite = rightWinSprite;
				this.transform.FindChild("WinnerGlow").position = new Vector2(this.transform.FindChild("PlayerRightResult").position.x, this.transform.FindChild("WinnerGlow").position.y);
			}
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
