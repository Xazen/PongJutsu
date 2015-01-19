﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class GameFlow : MonoBehaviour
	{
		// Debug
		[SerializeField]
		private bool consoleLog = true;

		// Min, Max and Default values
		[SerializeField]
		private float riverMinimumSpawnFrequency = 3.0f;
		[SerializeField]
		private float shurikenMaximumSpeed = 1.5f;
		private float angleDefaultValue;

		// Regularly increase Spawn frequency
		private int riverSpeedUpCounter = 0;
		[SerializeField]
		private float riverTimeSpawnMultiplier = 0.75f;
		[SerializeField]
		private float riverTimeSpawnMultiplierFrequency = 30.0f;

		// Increase shuriken speed for combos
		private int comboBuffCounterLeft = 0;
		private int comboBuffCounterRight = 0;
		[SerializeField]
		private int comboBuffFrequency = 5;
		[SerializeField]
		private float comboSpeedMultiplier = 1.15f;

		// Buff loosing Player
		private bool looseBuffLeft = false;
		private bool looseBuffRight = false;
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
			angleDefaultValue = GameVar.players.left.angle;
			riverSpeedUpCounter = 0;
			comboBuffCounterLeft = 0;
			comboBuffCounterRight = 0;
			looseBuffLeft = false;
			looseBuffRight = false;
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

			// Regularly buff player for combo
			int updateComboBuffCounterLeft = Mathf.FloorToInt (GameVar.players.left.comboCount / comboBuffFrequency);
			int updateComboBuffCounterRight = Mathf.FloorToInt (GameVar.players.right.comboCount / comboBuffFrequency);
			if (updateComboBuffCounterLeft > comboBuffCounterLeft) 
			{
				comboBuffCounterLeft = updateComboBuffCounterLeft;
				ComboBuffPlayer (GameVar.players.left);
			} 
			else if (updateComboBuffCounterLeft == 0 && comboBuffCounterLeft > 0)
			{
				ComboDebuffPlayer(GameVar.players.left, comboBuffCounterLeft);
				comboBuffCounterLeft = 0;
			}

			if (updateComboBuffCounterRight > comboBuffCounterRight) 
			{
				comboBuffCounterRight = updateComboBuffCounterRight;
				ComboBuffPlayer (GameVar.players.right);
			} 
			else if (updateComboBuffCounterRight == 0 && comboBuffCounterRight > 0) 
			{
				ComboDebuffPlayer(GameVar.players.right, comboBuffCounterRight);
				comboBuffCounterRight = 0;
			}

			// Enter critical mode when conditions are met
			if (GameVar.ingameTime > minimumTimeForCriticalMode && !isCritical && GameVar.forts.allCount <= 4 && GameVar.forts.leftCount <= 2 && GameVar.forts.rightCount <= 2) 
			{
				EnterCriticalMode ();
			}

			// Buff losing player
			int deltaFortCount = GameVar.forts.leftCount - GameVar.forts.rightCount;
			if (Mathf.Abs (deltaFortCount) >= requiredFortDeltaForBuff && !looseBuffLeft && !looseBuffRight) 
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
			else if (Mathf.Abs (deltaFortCount) < requiredFortDeltaForBuff && (looseBuffLeft || looseBuffRight))
			{
				DebuffPlayers ();
				looseBuffLeft = false;
				looseBuffRight = false;
			}
		}

		/// <summary>
		/// Combos the buff player.
		/// </summary>
		/// <param name="player">Player.</param>
		private void ComboBuffPlayer(GameVar.players.player player)
		{
			if (consoleLog) 
			{
				Debug.Log ("---GameFlow---");
			}

			if (player.speedMultiplier + (comboSpeedMultiplier - 1.0f) < shurikenMaximumSpeed) 
			{
				player.speedMultiplier += (comboSpeedMultiplier - 1.0f);
				player.angle += angleDefaultValue*(comboSpeedMultiplier - 1.0f);
			} 
			else 
			{
				player.angle *= (shurikenMaximumSpeed/player.speedMultiplier);
				player.speedMultiplier = shurikenMaximumSpeed;
			}

			// Log new values
			if (consoleLog)
			{
				Debug.Log("Combo buff player: " + player + "\n" +
				          "  Player speed multiplier   : " + player.speedMultiplier
				          );
			}
		}

		/// <summary>
		/// Combos the debuff player.
		/// </summary>
		/// <param name="player">Player.</param>
		/// <param name="comboCounter">Combo counter.</param>
		private void ComboDebuffPlayer(GameVar.players.player player, int comboCounter)
		{
			if (consoleLog) 
			{
				Debug.Log ("---GameFlow---");
			}

			// Correctly debuffs only buffs aquired from combo buff method
			if (player.speedMultiplier - ((comboSpeedMultiplier - 1.0f) * comboCounter) > 1.0f) 
			{
				// Speed multiplier
				player.speedMultiplier -= ((comboSpeedMultiplier - 1.0f) * comboCounter);

				// Player angle
				for (int i = 0; i < comboCounter; i++)
				{
					player.angle -= angleDefaultValue*(comboSpeedMultiplier-1.0f);
				}

				// Ensure player angle has minimum value
				if (player.angle < angleDefaultValue)
				{
					player.angle = angleDefaultValue;
				}
			} else 
			{
				// Ensure speed multiplier and player angle have minimum value;
				player.speedMultiplier = 1.0f;
				player.angle = angleDefaultValue;
			}

			// Log new values
			if (consoleLog)
			{
				Debug.Log("Combo debuff player: " + player + "\n" +
				          "  Player speed multiplier   : " + player.speedMultiplier
				          );
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

			looseBuffLeft = (player == GameVar.players.left);
			looseBuffRight = (player == GameVar.players.right);
			
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

			if (looseBuffLeft) 
			{
				GameVar.players.left.damageMultiplier -= (buffedDamageMultiplier - 1.0f);
			}


			if (looseBuffRight) 
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
