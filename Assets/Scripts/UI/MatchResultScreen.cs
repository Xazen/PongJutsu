using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PongJutsu
{
	public class MatchResultScreen : UIScript
	{
		[SerializeField]
		private ScoreOutput scoreOutputLeft;
		[SerializeField]
		private ScoreOutput scoreOutputRight;

		public void click_Exit()
		{
			GameManager.ExitGame();
		}

		public void click_Restart()
		{
			GameManager.RestartGame();
		}
	}

	[System.Serializable]
	class ScoreOutput
	{
		[SerializeField]
		Text wins;
		[SerializeField]
		Text reflected;
		[SerializeField]
		Text catched;
		[SerializeField]
		Text itemhit;
		[SerializeField]
		Text forthit;
		[SerializeField]
		Text dealtdamage;
	}
}
