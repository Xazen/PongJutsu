using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Control
{
	Movement,
	Shoot,
	Dash
}

public class PlayerInput : PlayerBase
{
	private bool offlineMode = true;

	[System.Serializable]
	private class PlayerKey
	{
		public Control control;

		public KeyCode positiveButton;
		public KeyCode negativeButton;

		public string joystickAssignmentName;
	}

	[SerializeField]
	private PlayerKey[] PlayerLeft;
	[SerializeField]
	private PlayerKey[] PlayerRight;

	private PlayerKey[] _ThisPlayerKeys;
	private PlayerKey[] ThisPlayerKeys
	{
		get
		{
			if (_ThisPlayerKeys == null)
			{
				List<PlayerKey> playerKeys = new List<PlayerKey>();

				if (offlineMode)
				{
					if (Player.playerSide == PlayerSide.Left)
						playerKeys.AddRange(PlayerLeft);
					else if (Player.playerSide == PlayerSide.Right)
						playerKeys.AddRange(PlayerRight);
				}
				else
				{
					playerKeys.AddRange(PlayerLeft);
					playerKeys.AddRange(PlayerRight);
				}

				_ThisPlayerKeys = playerKeys.ToArray();
			}

			return _ThisPlayerKeys;
		}
	}

	public float GetAxis(Control control)
	{
		float AxisValue = 0f;
		float KeyboardAxis = 0f;
		float JoystickAxis = 0f;

		foreach (PlayerKey key in ThisPlayerKeys)
		{
			if (key.control == control)
			{
				// Keyboard Axis
				if (Input.GetKey(key.positiveButton) ^ Input.GetKey(key.negativeButton))
				{
					if (Input.GetKey(key.positiveButton))
						KeyboardAxis = 1f;
					else if (Input.GetKey(key.negativeButton))
						KeyboardAxis = -1f;
				}

				//Joystick Axis
				if (key.joystickAssignmentName != string.Empty && (Mathf.Abs(Input.GetAxisRaw(key.joystickAssignmentName)) > Mathf.Abs(JoystickAxis)))
					JoystickAxis = Input.GetAxisRaw(key.joystickAssignmentName);
			}
		}

		if (Mathf.Abs(KeyboardAxis) >= Mathf.Abs(JoystickAxis))
			AxisValue = KeyboardAxis;
		else
			AxisValue = JoystickAxis;

		return AxisValue;
	}

	public bool GetButton(Control control)
	{
		bool KeyboardButton = false;
		bool JoystickButton = false;

		foreach (PlayerKey key in ThisPlayerKeys)
		{
			if (key.control == control)
			{
				// Keyboard Button
				if (Input.GetKey(key.positiveButton))
				{
					KeyboardButton = true;
					break;
				}

				//Joystick Button
				if (key.joystickAssignmentName != string.Empty && Input.GetButton(key.joystickAssignmentName))
				{
					JoystickButton = true;
					break;
				}
			}
		}

		return KeyboardButton || JoystickButton;
	}
}
