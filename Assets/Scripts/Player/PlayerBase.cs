using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	[RequireComponent(typeof(Player))]
	[RequireComponent(typeof(PlayerMovement))]
	[RequireComponent(typeof(PlayerAttack))]
	[RequireComponent(typeof(PlayerShield))]
	[RequireComponent(typeof(PlayerItemHandler))]

	public class PlayerBase : MonoBehaviour
	{
		Player _Player;
		public Player Player
		{
			get
			{
				if (_Player == null)
					_Player = GetComponent<Player>();

				return _Player;
			}
		}

		PlayerMovement _PlayerMovement;
		public PlayerMovement PlayerMovement
		{
			get
			{
				if (_PlayerMovement == null)
					_PlayerMovement = GetComponent<PlayerMovement>();

				return _PlayerMovement;
			}
		}

		PlayerAttack _PlayerAttack;
		public PlayerAttack PlayerAttack
		{
			get
			{
				if (_PlayerAttack == null)
					_PlayerAttack = GetComponent<PlayerAttack>();

				return _PlayerAttack;
			}
		}

		PlayerShield _PlayerShield;
		public PlayerShield PlayerShield
		{
			get
			{
				if (_PlayerShield == null)
					_PlayerShield = GetComponent<PlayerShield>();

				return _PlayerShield;
			}
		}

		PlayerItemHandler _PlayerItemHandler;
		public PlayerItemHandler PlayerItemHandler
		{
			get
			{
				if (_PlayerItemHandler == null)
					_PlayerItemHandler = GetComponent<PlayerItemHandler>();

				return _PlayerItemHandler;
			}
		}
	}
}
