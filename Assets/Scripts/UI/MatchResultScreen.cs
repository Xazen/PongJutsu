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

			outputScore(scoreOutputLeft, GameScore.GetPlayerLeftScore());
			outputScore(scoreOutputRight, GameScore.GetPlayerRightScore());
		}

		void outputScore(ScoreOutput scoreOutput, Score score)
		{
			scoreOutput.wins.text = score.wins.ToString();
			scoreOutput.reflected.text = (int)(((float)score.reflections / (float)GameScore.GetByEnemyScore(score).thrownshurikens) * 100) + "%";
			scoreOutput.catched.text = (int)(((float)score.catches / (float)GameScore.GetByEnemyScore(score).thrownshurikens) * 100) + "%";
			scoreOutput.itemhit.text = score.itemhits.ToString();
			scoreOutput.forthit.text = (int)(((float)score.forthits / (float)(score.thrownshurikens + score.reflections)) * 100) + "%";
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
