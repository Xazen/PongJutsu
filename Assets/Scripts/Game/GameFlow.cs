using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class GameFlow : MonoBehaviour
	{
		// Singleton implementation
		private static GameFlow _instance;
		public static GameFlow instance
		{
			get{
				if (_instance == null)
				{
					_instance = GameObject.FindObjectOfType<GameFlow>();
				}
				return _instance;
			}
		}

		// Events & Delegates
		public delegate void GameFlowDelegateWithPlayer(GameVar.players.player player);
		public delegate void GameFlowDelegate();
		public GameFlowDelegate OnMercyTimerEnd;
		public GameFlowDelegateWithPlayer OnEnterDisadvantageBuffPlayerPhase;
		public GameFlowDelegate OnExitDisadvantageBuffPlayerPhase;
		public GameFlowDelegate OnEnterMainPhase;
		public GameFlowDelegate OnEnterCriticalPhase;
		public GameFlowDelegateWithPlayer OnComboBuffPlayer;
		public GameFlowDelegateWithPlayer OnComboDebuffPlayer;
		public GameFlowDelegate OnOneFortLeft;
		private bool onMercyTimerEndTriggered = false;
		private bool onOneFortLeftTriggered = false;

		// Debug
		[SerializeField]
		private bool consoleLog = true;

		// Min, Max and Default values
		[SerializeField]
		private float riverMinimumSpawnFrequency = 3.0f;
		[SerializeField]
		private float shurikenMaximumSpeed = 1.5f;
		private float angleDefaultValue = 3.5f;
		private int itemDefaultSpawnProbability = 100;

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

		// Increase shuriken speed over time
		private float addedSpeedOverTime = 0.0f;
		[SerializeField]
		private float maxSpeedDuration = 180.0f;
		[SerializeField]
		private float mercyDuration = 30.0f;

		// Buff loosing Player
		private bool isDisadvantageBuffLeftPhase = false;
		private bool isDisadvantageBuffRightPhase = false;
		private float disadvantageBuffTimer = 0.0f;
		[SerializeField]
		private float disadvantageBuffDuration = 15.0f;
		[SerializeField]
		private int requiredFortDeltaForDisadvantageBuff = 2;
		[SerializeField]
		private float disadvantageBuffDamageMultiplier = 1.5f;

		// Main Phase
		private bool isMainPhase = false;

		// Critical Mode
		private bool isCriticalPhase = false;
		[SerializeField]
		private float minimumTimeForCriticalMode = 45.0f;
		[SerializeField]
		private float riverCriticalSpawnMultiplier = 0.33f;

		// Force critical items
		private bool isCriticalItemForced = false;
		[SerializeField] private float forceCriticalItems = 150;


		public void StartFlow()
		{
			if (consoleLog) 
			{
				Debug.Log ("---GameFlow---");
			}

			riverSpeedUpCounter = 0;
			comboBuffCounterLeft = 0;
			comboBuffCounterRight = 0;
			addedSpeedOverTime = 0;
			disadvantageBuffTimer = 0;
			isMainPhase = false;
			isDisadvantageBuffLeftPhase = false;
			isDisadvantageBuffRightPhase = false;
			isCriticalPhase = false;
			isCriticalItemForced = false;
			onMercyTimerEndTriggered = false;
			onOneFortLeftTriggered = false;

			if (consoleLog)
			{
				Debug.Log("Start new game.\n" +
				          "  Divider probability        : " + GameVar.river.itemList["Divider"].spawnProbability + "\n" +
				          "  ShieldExpander probability : " + GameVar.river.itemList["ShieldExpander"].spawnProbability + "\n" +
				          "  Inverter probability       : " + GameVar.river.itemList["Inverter"].spawnProbability + "\n" +
				          "  Repair probability         : " + GameVar.river.itemList["Repair"].spawnProbability + "\n" +
				          "  Bomb probability : " + GameVar.river.itemList["Bomb"].spawnProbability + "\n" +
				          "  Slow probability : " + GameVar.river.itemList["Slow"].spawnProbability);
			}
		}

		public void UpdateFlow()
		{
			//*******************
			// Regularly 
			//*******************

			// Regularly increase spawn frequency
			int updateRiverSpeedUpCounter = Mathf.FloorToInt(GameVar.ingameTime / riverTimeSpawnMultiplierFrequency);
			if (riverSpeedUpCounter < updateRiverSpeedUpCounter) 
			{
				riverSpeedUpCounter = updateRiverSpeedUpCounter;
				IncreaseRiverSpawnFrequency (riverTimeSpawnMultiplier);
			}

			// Regularly buff player for combo
			int latestComboBuffCounterLeft = Mathf.FloorToInt (GameVar.players.left.comboCount / comboBuffFrequency);
			int latestComboBuffCounterRight = Mathf.FloorToInt (GameVar.players.right.comboCount / comboBuffFrequency);
			if (latestComboBuffCounterLeft > comboBuffCounterLeft) 
			{
				comboBuffCounterLeft = latestComboBuffCounterLeft;
				ComboBuffPlayer (GameVar.players.left);
			} 
			else if (latestComboBuffCounterLeft == 0 && comboBuffCounterLeft > 0)
			{
				ComboDebuffPlayer(GameVar.players.left, comboBuffCounterLeft);
				comboBuffCounterLeft = 0;
			}

			if (latestComboBuffCounterRight > comboBuffCounterRight) 
			{
				comboBuffCounterRight = latestComboBuffCounterRight;
				ComboBuffPlayer (GameVar.players.right);
			} 
			else if (latestComboBuffCounterRight == 0 && comboBuffCounterRight > 0) 
			{
				ComboDebuffPlayer(GameVar.players.right, comboBuffCounterRight);
				comboBuffCounterRight = 0;
			}

			// Increase shuriken speed after time
			if (GameVar.ingameTime >= mercyDuration) 
			{
				if (!onMercyTimerEndTriggered && OnMercyTimerEnd != null)
				{
					OnMercyTimerEnd();
					onMercyTimerEndTriggered = true;
					Debug.Log ("mercy over");
				}

				if (addedSpeedOverTime <= shurikenMaximumSpeed-1.0f) 
				{
					addedSpeedOverTime += ((shurikenMaximumSpeed - 1.0f) / (maxSpeedDuration / Time.deltaTime));
				}
				
				IncreaseShurikenSpeedOverTimeForPlayer(GameVar.players.left);
				IncreaseShurikenSpeedOverTimeForPlayer(GameVar.players.right);
			}

			//*******************
			// Main game
			//*******************

			// Enable defensive items for main game
			if (GameVar.forts.allCount <= 8 && !isMainPhase)
			{
				EnterMainPhase();
			}

			//*******************
			// Critical phase
			//*******************

			// Enter critical mode when conditions are met
			if (GameVar.ingameTime > minimumTimeForCriticalMode && !isCriticalPhase && GameVar.forts.allCount <= 4 && GameVar.forts.leftCount <= 2 && GameVar.forts.rightCount <= 2) 
			{
				EnterCriticalPhase ();
			}

			// Critical item setup to speed up the game round
			if (GameVar.ingameTime >= forceCriticalItems && !isCriticalItemForced) 
			{
				EnableCriticalItems();
			}

			// Activate bomb when one of the ninjas have only 2 forts left
			if (GameVar.forts.leftCount == 2 || GameVar.forts.rightCount == 2) 
			{
				GameVar.river.itemList["Bomb"].spawnProbability = itemDefaultSpawnProbability;
			}

			//*******************
			// Other phases
			//*******************

			// Buff player in disadvantage
			if (isDisadvantageBuffLeftPhase || isDisadvantageBuffRightPhase) 
			{
				disadvantageBuffTimer += Time.deltaTime;
			}

			int deltaFortCount = GameVar.forts.leftCount - GameVar.forts.rightCount;
			if (Mathf.Abs (deltaFortCount) >= requiredFortDeltaForDisadvantageBuff && !isDisadvantageBuffLeftPhase && !isDisadvantageBuffRightPhase) 
			{
				disadvantageBuffTimer = 0.0f;

				if (deltaFortCount < 0) 
				{
					EnterDisadvantageBuffPhaseWithPlayer (GameVar.players.left);
				} 
				else 
				{
					EnterDisadvantageBuffPhaseWithPlayer (GameVar.players.right);
				}
			} 
			else if ((Mathf.Abs (deltaFortCount) < requiredFortDeltaForDisadvantageBuff || disadvantageBuffTimer >= disadvantageBuffDuration) && (isDisadvantageBuffLeftPhase || isDisadvantageBuffRightPhase))
			{
				ExitDisadvantageBuffPhase ();
				disadvantageBuffTimer = 0.0f;
				isDisadvantageBuffLeftPhase = false;
				isDisadvantageBuffRightPhase = false;
			}

			//*******************
			// Other callback
			//*******************
			if ((GameVar.forts.leftCount == 1 || GameVar.forts.rightCount == 1) && !onOneFortLeftTriggered && OnOneFortLeft != null) 
			{
				Debug.Log ("one fort left");
				OnOneFortLeft();
				onOneFortLeftTriggered = true;
			}
		}

		/// <summary>
		/// Increases the shuriken speed over time for player.
		/// </summary>
		/// <param name="player">Player.</param>
		private void IncreaseShurikenSpeedOverTimeForPlayer(GameVar.players.player player)
		{
			if (player.speedMultiplier + ((shurikenMaximumSpeed - 1.0f) / (maxSpeedDuration / Time.deltaTime)) < shurikenMaximumSpeed)
			{
				player.speedMultiplier += ((shurikenMaximumSpeed - 1.0f) / (maxSpeedDuration / Time.deltaTime));
				player.angle += angleDefaultValue * (((shurikenMaximumSpeed - 1.0f) / (maxSpeedDuration / Time.deltaTime)));
			} else {
				player.angle *= (shurikenMaximumSpeed / player.speedMultiplier);
				player.speedMultiplier = shurikenMaximumSpeed;
			}
		}

		/// <summary>
		/// Combos the buff player.
		/// </summary>
		/// <param name="player">Player.</param>
		private void ComboBuffPlayer(GameVar.players.player player)
		{
			if (player.speedMultiplier <= shurikenMaximumSpeed) 
			{
				if (OnComboBuffPlayer != null)
				{
				    OnComboBuffPlayer(player);
				}

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
		}

		/// <summary>
		/// Combos the debuff player.
		/// </summary>
		/// <param name="player">Player.</param>
		/// <param name="comboCounter">Combo counter.</param>
		private void ComboDebuffPlayer(GameVar.players.player player, int comboCounter)
		{
			if (OnComboDebuffPlayer != null) 
			{
				OnComboDebuffPlayer (player);
			}

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

				// Ensure added speed over time stays active
				if (player.speedMultiplier <= 1.0f+addedSpeedOverTime)
				{
					player.speedMultiplier = 1.0f+addedSpeedOverTime;
					player.angle = 1.0f+angleDefaultValue*addedSpeedOverTime;
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
		private void EnterDisadvantageBuffPhaseWithPlayer(GameVar.players.player player)
		{
			if (OnEnterDisadvantageBuffPlayerPhase != null) 
			{
				OnEnterDisadvantageBuffPlayerPhase (player);
			}

			if (consoleLog) 
			{
				Debug.Log ("---GameFlow---");
			}

			isDisadvantageBuffLeftPhase = (player == GameVar.players.left);
			isDisadvantageBuffRightPhase = (player == GameVar.players.right);

			player.damageMultiplier += (disadvantageBuffDamageMultiplier-1.0f);

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
		private void ExitDisadvantageBuffPhase()
		{
			if (OnExitDisadvantageBuffPlayerPhase != null) 
			{
				OnExitDisadvantageBuffPlayerPhase ();
			}

			if (consoleLog) 
			{
				Debug.Log ("---GameFlow---");
			}

			if (isDisadvantageBuffLeftPhase) 
			{
				GameVar.players.left.damageMultiplier -= (disadvantageBuffDamageMultiplier - 1.0f);
			}

			if (isDisadvantageBuffRightPhase) 
			{
				GameVar.players.right.damageMultiplier -= (disadvantageBuffDamageMultiplier - 1.0f);
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
		/// Adjusts the items for main phase.
		/// </summary>
		private void EnterMainPhase()
		{
			if (consoleLog) 
			{
				Debug.Log ("---GameFlow---");
			}

			if (OnEnterMainPhase != null) 
			{
				OnEnterMainPhase ();
			}

			isMainPhase = true;

			GameVar.river.itemList ["Repair"].spawnProbability = Mathf.RoundToInt(itemDefaultSpawnProbability*1.5f);
			// Log new values
			if (consoleLog)
			{
				Debug.Log("Enter main mode.\n" +
				          "  Divider probability        : " + GameVar.river.itemList["Divider"].spawnProbability + "\n" +
				          "  ShieldExpander probability : " + GameVar.river.itemList["ShieldExpander"].spawnProbability + "\n" +
				          "  Inverter probability       : " + GameVar.river.itemList["Inverter"].spawnProbability + "\n" +
				          "  Repair probability         : " + GameVar.river.itemList["Repair"].spawnProbability + "\n" +
				          "  Bomb probability : " + GameVar.river.itemList["Bomb"].spawnProbability + "\n" +
				          "  Slow probability : " + GameVar.river.itemList["Slow"].spawnProbability);
			}
		}

		/// <summary>
		/// Enters the critical mode.
		/// </summary>
		private void EnterCriticalPhase()
		{
			if (OnEnterCriticalPhase != null) 
			{	
				OnEnterCriticalPhase ();
			}

			if (consoleLog) 
			{
				Debug.Log ("---GameFlow---");
			}

			isCriticalPhase = true;

			IncreaseRiverSpawnFrequency (riverCriticalSpawnMultiplier);

			EnableCriticalItems ();
		}

		/// <summary>
		/// Enables the critical items.
		/// </summary>
		private void EnableCriticalItems()
		{
			isCriticalItemForced = true;

			GameVar.river.itemList["Divider"].spawnProbability *= 2;
			GameVar.river.itemList["Inverter"].spawnProbability *= 2;
			GameVar.river.itemList["Repair"].spawnProbability = 0;
			GameVar.river.itemList["ShieldExpander"].spawnProbability /= 2;
			GameVar.river.itemList["Bomb"].spawnProbability = (int)itemDefaultSpawnProbability*2;
			GameVar.river.itemList["Slow"].spawnProbability *= 2;
			
			// Log new values
			if (consoleLog)
			{
				Debug.Log("Enter critical mode.\n" +
				          "  Divider probability        : " + GameVar.river.itemList["Divider"].spawnProbability + "\n" +
				          "  ShieldExpander probability : " + GameVar.river.itemList["ShieldExpander"].spawnProbability + "\n" +
				          "  Inverter probability       : " + GameVar.river.itemList["Inverter"].spawnProbability + "\n" +
				          "  Repair probability         : " + GameVar.river.itemList["Repair"].spawnProbability + "\n" +
				          "  Bomb probability : " + GameVar.river.itemList["Bomb"].spawnProbability + "\n" +
				          "  Slow probability : " + GameVar.river.itemList["Slow"].spawnProbability);
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
