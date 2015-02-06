using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class GameScore : MonoBehaviour
	{
		private static GloablScore global;

		public static GloablScore GetGlobalScore()
		{
			return global;
		}

		private static PlayerScore playerLeft;
		private static PlayerScore playerRight;

		public static PlayerScore GetByPlayer(GameObject player)
		{
			if (player == GameVar.players.left.reference)
				return playerLeft;
			else if (player == GameVar.players.right.reference)
				return playerRight;

			return null;
		}
		public static PlayerScore GetByEnemyPlayer(GameObject player)
		{
			if (player == GameVar.players.left.reference)
				return playerRight;
			else if (player == GameVar.players.right.reference)
				return playerLeft;

			return null;
		}

		public static PlayerScore GetPlayerLeftScore()
		{
			return playerLeft;
		}
		public static PlayerScore GetPlayerRightScore()
		{
			return playerRight;
		}

		public static PlayerScore GetByEnemyScore(PlayerScore playerScore)
		{
			if (playerScore == playerLeft)
				return playerRight;
			else if (playerScore == playerRight)
				return playerLeft;

			return null;
		}

		public static void Clear()
		{
			global = new GloablScore();
			playerLeft = new PlayerScore();
			playerRight = new PlayerScore();
		}
	}

	public class PlayerScore
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

	public class GloablScore
	{
		private int _spawneditems = 0;
		public int spawneditems { get { return _spawneditems; } }

		public void plusSpawnedItem()
		{
			_spawneditems += 1;
		}
	}
}
