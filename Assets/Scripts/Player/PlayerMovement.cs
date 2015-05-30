using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : PlayerBase
{
	public float minMovementSpeed = 0f;
	public float maxMovementSpeed = 8f;
	public float accelerationSpeed = 0.5f;
	public float decelerationSpeed = 1.5f;

	[SerializeField]
	private bool resetMovementAtTurn = true;
	private float currentSpeed = 0f;
	private int moveDirection;

	[SerializeField]
	private float dashDistance = 2f;
	public float dashSpeed = 10f;
	public float dashCooldown = 0.5f;

	private float dashStartPosition;
	private bool isDashing = false;
	private float dashDirection;
	private float dashLerp;
	private float lastDash;

	private float syncedPosition;
	private float latestPosition;
	private float fraction;
	private float latestTimestamp;

	[SerializeField]
	private float playerCollisionOffset = 0.3f;

	[HideInInspector]
	public bool invertDirection = false;

	[HideInInspector]
	public float movementNormalized { get { return (Mathf.Min(currentSpeed, maxMovementSpeed) * moveDirection) / maxMovementSpeed; } }

	void FixedUpdate()
	{
		if (GameManager.allowInput)
		{
			if (photonView.isMine)
			{
				InputDashing();
				InputMovement();
			}
			else
			{
				SyncedMovement();
			}
		}
	}

	private void InputDashing()
	{
		lastDash += Time.fixedDeltaTime;

		if (PlayerInput.GetAxis(Control.Dash) != 0f && PlayerInput.GetAxis(Control.Movement) != 0f)
		{
			dash();
		}
	}

	private void dash()
	{
		if (!isDashing && lastDash > dashCooldown)
		{
			// Activate dashing
			isDashing = true;
			this.GetComponent<SoundPool>().PlayRandom();

			dashLerp = 0;
			dashStartPosition = this.transform.position.y;
			dashDirection = Direction(PlayerInput.GetAxis(Control.Movement));
		}
	}

	private void InputMovement()
	{
		// Get current position
		float position = this.transform.position.y;

		// Calculate Speed and direction
		if (isDashing)
		{
			dashLerp += dashSpeed * Time.fixedDeltaTime;
			position = Mathf.Lerp(dashStartPosition, dashStartPosition + dashDistance * dashDirection, dashLerp);
			currentSpeed = dashSpeed;

			if (dashLerp >= 1f)
			{
				isDashing = false;
				lastDash = 0f;
			}
		}
		else
		{
			if (PlayerInput.GetAxis(Control.Movement) != 0f)
			{
				if (resetMovementAtTurn && moveDirection != Direction(PlayerInput.GetAxis(Control.Movement)))
					currentSpeed = 0;

				currentSpeed = Mathf.Clamp(currentSpeed + accelerationSpeed * Mathf.Abs(PlayerInput.GetAxis(Control.Movement)), minMovementSpeed, maxMovementSpeed);
				moveDirection = Direction(PlayerInput.GetAxis(Control.Movement));
			}
			else
			{
				currentSpeed = Mathf.Clamp(currentSpeed - decelerationSpeed, 0f, maxMovementSpeed);
				if (currentSpeed == 0f)
					moveDirection = 0;
			}
			position += (currentSpeed * moveDirection) * Time.fixedDeltaTime;
		}

		// Set new position
		this.transform.position = new Vector2(this.transform.position.x, ClampPosition(position));

		SetAnimation(currentSpeed, moveDirection);
	}

	private void SyncedMovement()
	{
		fraction += Time.fixedDeltaTime * (PhotonNetwork.sendRateOnSerialize * 0.85f);
		float position = Mathf.Lerp(latestPosition, syncedPosition, fraction);
		
		this.transform.position = new Vector2(this.transform.position.x, ClampPosition(position));

		SetAnimation(currentSpeed, moveDirection);

		if (isDashing && lastDash == 0f)
			this.GetComponent<SoundPool>().PlayRandom();

		lastDash += Time.fixedDeltaTime;
    }

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			float pos = this.transform.position.y;
			stream.Serialize(ref pos);

			stream.Serialize(ref isDashing);
		}
		else
		{
			float pos = 0f;
			stream.Serialize(ref pos);

			stream.Serialize(ref isDashing);


			syncedPosition = pos;
			latestPosition = transform.position.y;
			fraction = 0;

			currentSpeed = Mathf.Abs(syncedPosition - latestPosition) / ((float)info.timestamp - latestTimestamp);
			moveDirection = Direction(syncedPosition - latestPosition);

			if (isDashing)
				lastDash = 0f;

			latestTimestamp = (float)info.timestamp;
		}
	}

	private float ClampPosition(float position)
	{
		// Clamp Position in Boundaries
		GameObject boundaryTop = GameObject.FindGameObjectWithTag("BoundaryTop");
		GameObject boundaryBottom = GameObject.FindGameObjectWithTag("BoundaryBottom");
		float minPosition = boundaryBottom.transform.position.y + boundaryBottom.GetComponent<BoxCollider2D>().size.y / 2f + playerCollisionOffset;
		float maxPosition = boundaryTop.transform.position.y - boundaryTop.GetComponent<BoxCollider2D>().size.y / 2f - playerCollisionOffset;

		return Mathf.Clamp(position, minPosition, maxPosition);
	}

	private void SetAnimation(float speed, int direction)
	{
		// Set animation
		Player.Animator.SetFloat("Movement", speed);
		Player.Animator.SetInteger("Direction", direction);
		Player.Animator.SetFloat("Position", this.transform.position.y);
		Player.Animator.SetBool("Dash", isDashing);

		// Set animation speed depending on move speed
		if (direction != 0)
			Player.Animator.speed = Mathf.Max(0.3f, speed / maxMovementSpeed);
		else
			Player.Animator.speed = 1f;
	}

	public void ResetAnimation()
	{
		// Set animation
		Player.Animator.speed = 1f;
		Player.Animator.SetFloat("Movement", 0f);
		Player.Animator.SetInteger("Direction", 0);
		Player.Animator.SetFloat("Position", this.transform.position.y);
		Player.Animator.SetBool("Dash", false);
	}

	private int Direction(float f)
	{
		return (int)((f != 0f) ? Mathf.Sign(f) : 0) * (invertDirection ? -1 : 1);
	}
}
