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

		void Update()
		{
			//Debug.Log("r:" + reflections + "; c:" + catches + "; i:" + itemhits + "; f:" + forthits);
		}

		public void plusRefelect()
		{
			reflections += 1;
		}

		public void plusCatch()
		{
			catches += 1;
		}

		public void plusItemHit()
		{
			itemhits += 1;
		}

		public void plusFortHit()
		{
			forthits += 1;
		}

		public void plusDealtDamage(int damage)
		{
			dealtdamage += damage;
		}
	}
}
