using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class GameFlow : MonoBehaviour
	{
		// Debug
		[SerializeField]
		private bool consoleLog = true;

		// Min und Max values
		[SerializeField]
		private float riverMinimumSpawnFrequency = 3.0f;

		// Regularly increase Spawn frequency
		private int riverSpeedUpCounter = 0;
		[SerializeField]
		private float riverTimeSpawnMultiplier = 0.75f;
		[SerializeField]
		private float riverTimeSpawnMultiplierFrequency = 30.0f;

		// Increase shuriken speed for combos

		// Buff loosing Player
		private bool buffedLeft = false;
		private bool buffedRight = false;
		[SerializeField]
		private int requiredFortDeltaForBuff = 2;
		[SerializeField]
		private float buffedDamageMultiplier = 1.5f;

		// Critical Mode
		private bool isCritical = false;
		[SerializeField]
		private float minimumTimeForCriticalMode = 45.0f;
		[SerializeField]
		private float riverCriticalSpawnMultiplier = 0.33f;


		public void StartFlow()
		{
			riverSpeedUpCounter = 0;
			buffedLeft = false;
			buffedRight = false;
			isCritical = false;
		}

		public void UpdateFlow()
		{
			// Regularly increase spawn frequency
			int updateRiverSpeedUpCounter = Mathf.FloorToInt(GameVar.ingameTime / riverTimeSpawnMultiplierFrequency);
			if (riverSpeedUpCounter < updateRiverSpeedUpCounter) 
			{
				riverSpeedUpCounter = updateRiverSpeedUpCounter;
				IncreaseRiverSpawnFrequency (riverTimeSpawnMultiplier);

				// Also change flow direction
				InvertRiverFlow();
			}

			// Enter critical mode when conditions are met
			if (GameVar.ingameTime > minimumTimeForCriticalMode && !isCritical && GameVar.forts.allCount <= 4 && GameVar.forts.leftCount <= 2 && GameVar.forts.rightCount <= 2) 
			{
				EnterCriticalMode ();
			}

			// Buff losing player
			int deltaFortCount = GameVar.forts.leftCount - GameVar.forts.rightCount;
			if (Mathf.Abs (deltaFortCount) >= requiredFortDeltaForBuff && !buffedLeft && !buffedRight) 
			{
				if (deltaFortCount < 0) 
				{
					BuffLosingPlayer (GameVar.players.left);
				} 
				else 
				{
					BuffLosingPlayer (GameVar.players.right);
				}
			} 
			else if (Mathf.Abs (deltaFortCount) < requiredFortDeltaForBuff && (buffedLeft || buffedRight))
			{
				DebuffPlayers ();
				buffedLeft = false;
				buffedRight = false;
			}
		}

		/// <summary>
		/// Buffs the losing player.
		/// </summary>
		/// <param name="player">Player.</param>
		private void BuffLosingPlayer(GameVar.players.player player)
		{
			if (consoleLog) 
			{
				Debug.Log ("---GameFlow---");
			}

			buffedLeft = (player == GameVar.players.left);
			buffedRight = (player == GameVar.players.right);
			
			player.damageMultiplier += (buffedDamageMultiplier-1.0f);

			// Log new values
			if (consoleLog)
			{
				Debug.Log("Buffed player: " + player + "\n" +
				          "  player damage multiplier   : " + player.damageMultiplier
				          );
			}
		}

		/// <summary>
		/// Debuffs the players.
		/// </summary>
		private void DebuffPlayers()
		{
			if (consoleLog) 
			{
				Debug.Log ("---GameFlow---");
			}

			if (buffedLeft) 
			{
				GameVar.players.left.damageMultiplier -= (buffedDamageMultiplier - 1.0f);
			}


			if (buffedRight) 
			{
				GameVar.players.right.damageMultiplier -= (buffedDamageMultiplier - 1.0f);
			}

			// Log new values
			if (consoleLog)
			{
				Debug.Log("Debuff player:\n" +
				          "  player left damage multiplier   : " + GameVar.players.left.damageMultiplier + "\n" +
				          "  player right damage multiplier  : " + GameVar.players.right.damageMultiplier
				          );
			}
		}

		/// <summary>
		/// Enters the critical mode.
		/// </summary>
		private void EnterCriticalMode()
		{
			if (consoleLog) 
			{
				Debug.Log ("---GameFlow---");
			}

			isCritical = true;

			IncreaseRiverSpawnFrequency (riverCriticalSpawnMultiplier);

			GameVar.river.itemList["Divider"].spawnProbability *= 2;
			GameVar.river.itemList["Inverter"].spawnProbability *= 2;
			GameVar.river.itemList["Repair"].spawnProbability /= 2;
			GameVar.river.itemList["ShieldExpander"].spawnProbability /= 2;
			//GameVar.river.itemList["Divider"].spawnProbability *= 2;
			//GameVar.river.itemList["Divider"].spawnProbability *= 2;

			// Log new values
			if (consoleLog)
			{
				Debug.Log("Enter critical mode.\n" +
				          "  Divider probability        : " + GameVar.river.itemList["Divider"].spawnProbability + "\n" +
				          "  ShieldExpander probability : " + GameVar.river.itemList["ShieldExpander"].spawnProbability + "\n" +
				          "  Inverter probability       : " + GameVar.river.itemList["Inverter"].spawnProbability + "\n" +
				          "  Repair probability         : " + GameVar.river.itemList["Repair"].spawnProbability + "\n" +
				          //"  Divider probability : " + GameVar.river.itemList + "\n" +
				          //"  Divider probability : " + GameVar.river.itemList + "\n" +
				          ""
				          );

			}
		}

		/// <summary>
		/// Increases the river spawn frequency. When the multiplier would decreased the spawn frequency too far, the minimum spawn frequency will be used.
		/// </summary>
		/// <param name="multiplier">Multiplier for the river spawn frequency. The smaller the value the faster the items are spawning.</param>
		private void IncreaseRiverSpawnFrequency(float multiplier)
		{
			if (consoleLog) 
			{
				Debug.Log ("---GameFlow---");
			}

			// Validate new river speed before update
			if (GameVar.river.spawnFrequency * multiplier > riverMinimumSpawnFrequency)
			{
				// Update river spawn frequency
				GameVar.river.spawnFrequency *= multiplier;
				GameVar.river.frequencyRandomizer *= multiplier;

			} 
			else if (GameVar.river.spawnFrequency != riverMinimumSpawnFrequency) 
			{
				// Update river spawn frequency to minimum values
				float tempSpawnFrequencyMultiplier = riverMinimumSpawnFrequency/GameVar.river.spawnFrequency;
				GameVar.river.spawnFrequency = riverMinimumSpawnFrequency;
				GameVar.river.frequencyRandomizer *= tempSpawnFrequencyMultiplier;
			}

			// Log new values
			if (consoleLog)
			{
				Debug.Log("Inreased river speed.\n" +
				          "  River Spawn Frequency       : " + GameVar.river.spawnFrequency + "\n" +
				          "  River Frequency Randomizer  : " + GameVar.river.frequencyRandomizer + "\n" +
				          "---------");
			}
		}

		/// <summary>
		/// Inverts the river flow.
		/// </summary>
		private void InvertRiverFlow()
		{
			GameVar.river.flowSpeed *= -1.0f;
		}
	}
}
