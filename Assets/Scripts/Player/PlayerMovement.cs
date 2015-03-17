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
	private float moveDirection;

	[SerializeField]
	private float dashDistance = 2f;
	public float dashSpeed = 10f;
	public float dashCooldown = 0.5f;

	private float dashStartPosition;
	private bool isDashing = false;
	private float dashDirection;
	private float dashLerp;
	private float lastDash;

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
			Dashing();
			Movement();
		}
	}

	private void Dashing()
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

			dashLerp = 0;
			dashStartPosition = this.transform.position.y;
			dashDirection = Direction(PlayerInput.GetAxis(Control.Movement));

			this.GetComponent<SoundPool>().PlayRandom();
		}
	}

	private void Movement()
	{
		// Get current position
		float position = this.transform.position.y;

		// Calculate Speed and direction
		if (PlayerInput.GetAxis(Control.Movement) != 0f)
		{
			if (resetMovementAtTurn && moveDirection != Direction(PlayerInput.GetAxis(Control.Movement)))
				currentSpeed = 0;

			if (currentSpeed == 0)
				currentSpeed = minMovementSpeed;

			currentSpeed = Mathf.Clamp(currentSpeed + accelerationSpeed * Mathf.Abs(PlayerInput.GetAxis(Control.Movement)), 0f, maxMovementSpeed);
			moveDirection = Direction(PlayerInput.GetAxis(Control.Movement));
		}
		else
		{
			currentSpeed = Mathf.Clamp(currentSpeed - decelerationSpeed, 0f, maxMovementSpeed);
			if (currentSpeed == 0f)
				moveDirection = 0f;
		}

		// Set temp position
		position += (currentSpeed * moveDirection) * Time.fixedDeltaTime;

		// Override at dashing
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

		// Clamp Position in Boundaries
		GameObject boundaryTop = GameObject.FindGameObjectWithTag("BoundaryTop");
		GameObject boundaryBottom = GameObject.FindGameObjectWithTag("BoundaryBottom");
		float minPosition = boundaryBottom.transform.position.y + boundaryBottom.GetComponent<BoxCollider2D>().size.y / 2f + playerCollisionOffset;
		float maxPosition = boundaryTop.transform.position.y - boundaryTop.GetComponent<BoxCollider2D>().size.y / 2f - playerCollisionOffset;
		position = Mathf.Clamp(position, minPosition, maxPosition);

		// Set new position
		this.transform.position = new Vector2(this.transform.position.x, position);

		// Set animation
		Player.Animator.SetFloat("Movement", currentSpeed);
		Player.Animator.SetInteger("Direction", (int)moveDirection);
		Player.Animator.SetFloat("Position", this.transform.position.y);
		Player.Animator.SetInteger("Input", (int)Direction(PlayerInput.GetAxis(Control.Movement)));
		Player.Animator.SetBool("Dash", isDashing);

		// Set animation speed depending on move speed
		if (PlayerInput.GetAxis(Control.Movement) != 0)
			Player.Animator.speed = currentSpeed / maxMovementSpeed;
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
		Player.Animator.SetInteger("Input", 0);
		Player.Animator.SetBool("Dash", false);
	}

	private float Direction(float f)
	{
		if (f != 0f)
			f = Mathf.Sign(f);

		return f * (Convert.ToInt32(invertDirection) * -2 + 1);
	}
}
