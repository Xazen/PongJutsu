using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class GameMatch : MonoBehaviour
	{
		private static List<string> results = new List<string>();
		private static int minRoundsToWin = 2;

		public static void newMatch()
		{
			results.Clear();
			GameScore.Clear();
		}

		public static void addWinner(string winner)
		{
			results.Add(winner);

			if (winner == "left")
				GameScore.GetPlayerLeftScore().plusWin();
			else if (winner == "right")
				GameScore.GetPlayerRightScore().plusWin();
		}

		public static List<string> getWinnerList()
		{
			return results;
		}

		public static string getLastWinner()
		{
			return results[results.Count - 1];
		}

		public static int getWinsPlayerLeft()
		{
			return results.FindAll(x => x == "left").Count;
		}
		public static int getWinsPlayerRight()
		{
			return results.FindAll(x => x == "right").Count;
		}

		public static string getMatchWinner()
		{
			if (results.FindAll(x => x == "left").Count >= minRoundsToWin)
				return "left";
			else if (results.FindAll(x => x == "right").Count >= minRoundsToWin)
				return "right";
			else
				return null;
		}
	}
}
