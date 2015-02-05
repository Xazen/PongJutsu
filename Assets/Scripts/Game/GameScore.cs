using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class GameScore : MonoBehaviour
	{
		private static Score playerLeft = new Score();
		private static Score playerRight = new Score();

		public static Score GetByPlayer(GameObject player)
		{
			if (player == GameVar.players.left.reference)
				return playerLeft;
			else if (player == GameVar.players.right.reference)
				return playerRight;

			return null;
		}
		public static Score GetPlayerLeft()
		{
			return playerLeft;
		}
		public static Score GetPlayerRight()
		{
			return playerRight;
		}

		public static void Clear()
		{
			playerLeft = new Score();
			playerRight = new Score();
		}
	}

	public class Score
	{
		private int _thrownshurikens = 0;
		public int thrownshurikens { get { return _thrownshurikens; } private set { } }

		private int _reflections = 0;
		public int reflections { get { return _reflections; } private set { } }

		private int _catches = 0;
		public int catches { get { return _catches; } private set { } }

		private int _itemhits = 0;
		public int itemhits { get { return _itemhits; } private set { } }

		private int _forthits = 0;
		public int forthits { get { return _forthits; } private set { } }

		private int _dealtdamage = 0;
		public int dealtdamage { get { return _dealtdamage; } private set { } }

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
