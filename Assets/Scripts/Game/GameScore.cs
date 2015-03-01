using UnityEngine;
using System.Collections;

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
		if (player == GameVar.players.left.gameObject)
			return playerLeft;
		else if (player == GameVar.players.right.gameObject)
			return playerRight;

		return null;
	}
	public static PlayerScore GetByEnemyPlayer(GameObject player)
	{
		if (player == GameVar.players.left.gameObject)
			return playerRight;
		else if (player == GameVar.players.right.gameObject)
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
	public int wins { get { return _wins; } set { _wins = value; } }

	private int _thrownshurikens = 0;
	public int thrownshurikens { get { return _thrownshurikens; } set { if (GameManager.allowInput) { _thrownshurikens = value; } } }

	private int _reflections = 0;
	public int reflections { get { return _reflections; } set { if (GameManager.allowInput) { _reflections = value; } } }

	private int _catches = 0;
	public int catches { get { return _catches; } set { if (GameManager.allowInput) { _catches = value; } } }

	private int _itemhits = 0;
	public int itemhits { get { return _itemhits; } set { if (GameManager.allowInput) { _itemhits = value; } } }

	private int _forthits = 0;
	public int forthits { get { return _forthits; } set { if (GameManager.allowInput) { _forthits = value; } } }

	private int[] _dealtdamage = { 0, 0, 0 };
	public int dealtdamage { get { int d = 0; foreach (int i in _dealtdamage) { d += i; } return d; } }
	public int dealtdamageRound { get { return _dealtdamage[GameMatch.getRound()]; } set { if (GameManager.allowInput) { _dealtdamage[GameMatch.getRound()] = value; } } }
}

public class GloablScore
{
	private int _spawneditems = 0;
	public int spawneditems { get { return _spawneditems; } set { if (GameManager.allowInput) { _spawneditems = value; } } }
}
