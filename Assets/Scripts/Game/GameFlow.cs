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

		// Increase Spawn frequency
		[SerializeField]
		private float riverTimeSpawnMultiplier = 0.75f;
		[SerializeField]
		private float riverTimeSpawnMultiplierFrequency = 30.0f;

		// Critical Mode
		[SerializeField]
		private float minimumTimeForCriticalMode = 45.0f;
		[SerializeField]
		private float riverCriticalSpawnMultiplier = 0.33f;

		private int riverSpeedUpCounter = 0;
		private bool isCritical = false;

		public void StartFlow()
		{

		}

		public void UpdateFlow()
		{
			// Regularaly increase spawn frequency
			int updateRiverSpeedUpCounter = Mathf.FloorToInt(GameVar.ingameTime / riverTimeSpawnMultiplierFrequency);
			if (riverSpeedUpCounter < updateRiverSpeedUpCounter) 
			{
				riverSpeedUpCounter = updateRiverSpeedUpCounter;
				IncreaseRiverSpawnFrequency (riverTimeSpawnMultiplier);
			}

			// Enter critical mode when conditions are met
			if (GameVar.ingameTime > minimumTimeForCriticalMode && !isCritical && GameVar.forts.allCount <= 4 && GameVar.forts.leftCount <= 2 && GameVar.forts.rightCount <= 2) 
			{
				isCritical = true;
				EnterCriticalMode ();
			}

			// Buff losing player
			int deltaFortCount = GameVar.forts.leftCount - GameVar.forts.rightCount;
			if (Mathf.Abs (deltaFortCount) >= 3) 
			{
				if (deltaFortCount < 0)
				{
					BuffLosingPlayer(GameVar.players.left);
				} else {
					BuffLosingPlayer(GameVar.players.right);
				}
			}
		}

		private void BuffLosingPlayer(Player player)
		{

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
				Debug.Log("---GameFlow---\n" +
				          "Inreased river speed.\n" +
				          "  River Spawn Frequency       : " + GameVar.river.spawnFrequency + "\n" +
				          "  River Frequency Randomizer  : " + GameVar.river.frequencyRandomizer + "\n" +
				          "---------");
			}
		}
	}
}
