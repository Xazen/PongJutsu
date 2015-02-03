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

		public override void uiEnable()
		{
			base.uiEnable();

			outputScore(scoreOutputLeft, GameScore.GetPlayerLeft());
			outputScore(scoreOutputRight, GameScore.GetPlayerRight());
		}

		void outputScore(ScoreOutput scoreOutput, Score score)
		{
			scoreOutput.wins.text = "-";
			scoreOutput.reflected.text = score.resultReflect().ToString();
			scoreOutput.catched.text = score.resultCatch().ToString();
			scoreOutput.itemhit.text = score.resultItemHit().ToString();
			scoreOutput.forthit.text = score.resultFortHit().ToString();
			scoreOutput.dealtdamage.text = "-";
		}

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
		public Text wins;
		public Text reflected;
		public Text catched;
		public Text itemhit;
		public Text forthit;
		public Text dealtdamage;
	}
}
