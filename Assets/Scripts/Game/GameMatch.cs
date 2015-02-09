using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class GameMatch : MonoBehaviour
	{
		private static List<string> results = new List<string>();
		private static int round = 0;
		private static int minRoundsToWin = 2;

		public static void newMatch()
		{
			results.Clear();
			round = 0;
			GameScore.Clear();
		}

		public static int getRound()
		{
			return round;
		}

		public static void startRound()
		{
			round = getWinnerList().Count;
		}

		public static void addWinner(string winner)
		{
			results.Add(winner);

			if (winner == "left")
				GameScore.GetPlayerLeftScore().wins += 1;
			else if (winner == "right")
				GameScore.GetPlayerRightScore().wins += 1;
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
