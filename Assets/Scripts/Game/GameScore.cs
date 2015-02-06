using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class GameScore : MonoBehaviour
	{
		private static Score playerLeft;
		private static Score playerRight;

		public static Score GetByPlayer(GameObject player)
		{
			if (player == GameVar.players.left.reference)
				return playerLeft;
			else if (player == GameVar.players.right.reference)
				return playerRight;

			return null;
		}
		public static Score GetByEnemyPlayer(GameObject player)
		{
			if (player == GameVar.players.left.reference)
				return playerRight;
			else if (player == GameVar.players.right.reference)
				return playerLeft;

			return null;
		}

		public static Score GetPlayerLeftScore()
		{
			return playerLeft;
		}
		public static Score GetPlayerRightScore()
		{
			return playerRight;
		}

		public static Score GetByEnemyScore(Score playerScore)
		{
			if (playerScore == playerLeft)
				return playerRight;
			else if (playerScore == playerRight)
				return playerLeft;

			return null;
		}

		public static void Clear()
		{
			playerLeft = new Score();
			playerRight = new Score();
		}
	}

	public class Score
	{
		private int _wins = 0;
		public int wins { get { return _wins; } }

		private int _thrownshurikens = 0;
		public int thrownshurikens { get { return _thrownshurikens; }}

		private int _reflections = 0;
		public int reflections { get { return _reflections; }}

		private int _catches = 0;
		public int catches { get { return _catches; }}

		private int _itemhits = 0;
		public int itemhits { get { return _itemhits; }}

		private int _forthits = 0;
		public int forthits { get { return _forthits; }}

		private int _dealtdamage = 0;
		public int dealtdamage { get { return _dealtdamage; }}

		public void plusWin()
		{
			_wins += 1;
		}

		public void plusReflect()
		{
			_reflections += 1;
		}

		public void plusCatch()
		{
			_catches += 1;
		}

		public void plusItemHit()
		{
			_itemhits += 1;
		}

		public void plusFortHit()
		{
			_forthits += 1;
		}

		public void plusThrownShuriken()
		{
			_thrownshurikens += 1;
		}

		public void plusDealtDamage(int damage)
		{
			_dealtdamage += damage;
		}
	}
}
