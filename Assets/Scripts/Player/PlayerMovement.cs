using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerMovement : MonoBehaviour
	{

		public float maxMovementSpeed = 8f;
		public float accelerationSpeed = 0.5f;
		public float decelerationSpeed = 1.5f;
		public bool resetMovementAtTurn = true;
		private float currentSpeed = 0f;
		private float moveDirection;

		public float dashDistance = 2f;
		public float dashSpeed = 10f;
		public float dashCooldown = 0.5f;
		public float dashButtonInterval = 0.2f;

		private float lastInputDeltaTime;
		private float lastInputDirection;
		private float dashStartPosition;
		private bool isDashing = false;
		private float dashDirection;
		private float dashLerp;
		private float lastDash;

		public float playerCollisionOffset = 0.3f;

		void Update()
		{
			Dashing();
			Movement();
		}

		void Dashing()
		{
			lastInputDeltaTime += Time.deltaTime;
			lastDash += Time.deltaTime;

			if (Input.GetButtonDown(this.tag))
			{
				if (lastInputDeltaTime < dashButtonInterval && !isDashing && lastDash > dashCooldown && lastInputDirection == Direction(Input.GetAxisRaw(this.tag)))
				{
					// Activate dashing
					isDashing = true;

					dashLerp = 0;
					dashStartPosition = this.transform.position.y;
					dashDirection = Direction(Input.GetAxisRaw(this.tag));
				}

				// Set last Input
				lastInputDeltaTime = 0f;
				lastInputDirection = Direction(Input.GetAxisRaw(this.tag));
			}
		}

		void Movement()
		{
			// Get current position
			float position = this.transform.position.y;

			// Calculate Speed and direction
			if (Input.GetAxisRaw(this.tag) != 0f)
			{
				if (resetMovementAtTurn && moveDirection != Direction(Input.GetAxisRaw(this.tag)))
					currentSpeed = 0;

				currentSpeed = Mathf.Clamp(currentSpeed + accelerationSpeed, 0f, maxMovementSpeed);
				moveDirection = Direction(Input.GetAxisRaw(this.tag));
			}
			else
			{
				currentSpeed = Mathf.Clamp(currentSpeed - decelerationSpeed, 0f, maxMovementSpeed);
				if (currentSpeed == 0f)
					moveDirection = 0f;
			}

			// Set temp position
			position = this.transform.position.y + currentSpeed * moveDirection * Time.deltaTime;

			// Override at dashing
			if (isDashing)
			{
				dashLerp += dashSpeed * Time.deltaTime;
				position = Mathf.Lerp(dashStartPosition, dashStartPosition + dashDistance * Direction(dashDirection), dashLerp);
				currentSpeed = dashSpeed;

				if (dashLerp >= 1f)
				{
					isDashing = false;
					lastDash = 0f;
				}
			}

			// Set animation
			this.GetComponentInChildren<Animator>().SetFloat("Move", currentSpeed * moveDirection);

			// Set animation speed depending on move speed
			if (Input.GetAxisRaw(this.tag) != 0)
				this.GetComponentInChildren<Animator>().speed = currentSpeed / maxMovementSpeed;
			else
				this.GetComponentInChildren<Animator>().speed = 1f;

			// Check and Precalculating Collision
			GameObject top = GameObject.FindGameObjectWithTag("BoundaryTop");
			GameObject bottom = GameObject.FindGameObjectWithTag("BoundaryBottom");
			if (position > top.transform.position.y - top.GetComponent<BoxCollider2D>().size.y / 2f - playerCollisionOffset)
			{
				position = top.transform.position.y - top.GetComponent<BoxCollider2D>().size.y / 2f - playerCollisionOffset;
			}
			else if (position < bottom.transform.position.y + bottom.GetComponent<BoxCollider2D>().size.y / 2f + playerCollisionOffset)
			{
				position = bottom.transform.position.y + bottom.GetComponent<BoxCollider2D>().size.y / 2f + playerCollisionOffset;
			}

			// Set new position
			this.transform.position = new Vector2(this.transform.position.x, position);
		}

		static float Direction(float f)
		{
			if (f != 0f)
				f = Mathf.Sign(f);

			return f;
		}
	}
}