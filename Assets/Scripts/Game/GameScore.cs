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
		private int reflections = 0;
		private int catches = 0;
		private int itemhits = 0;
		private int forthits = 0;
		private int dealtdamage = 0;

		public void plusRefelect()
		{
			reflections += 1;
		}
		public int resultReflect()
		{
			return reflections;
		}

		public void plusCatch()
		{
			catches += 1;
		}
		public int resultCatch()
		{
			return catches;
		}

		public void plusItemHit()
		{
			itemhits += 1;
		}
		public int resultItemHit()
		{
			return itemhits;
		}

		public void plusFortHit()
		{
			forthits += 1;
		}
		public int resultFortHit()
		{
			return forthits;
		}

		public void plusDealtDamage(int damage)
		{
			dealtdamage += damage;
		}
		public int resultDealtDamage()
		{
			return dealtdamage;
		}
	}
}
