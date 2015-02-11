using UnityEngine;
using System.Collections;
using System;

namespace PongJutsu
{
	public class PlayerMovement : MonoBehaviour
	{

		public float minMovementSpeed = 0f;
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

		private float dashStartPosition;
		private bool isDashing = false;
		private float dashDirection;
		private float dashLerp;
		private float lastDash;

		public float playerCollisionOffset = 0.3f;

		[HideInInspector] public bool invertDirection = false;

		public float movementNormalized { get { return (Mathf.Min(currentSpeed, maxMovementSpeed) * moveDirection) / maxMovementSpeed; } }

		void Update()
		{
			if (GameManager.allowInput)
			{
				Dashing();
				Movement();
			}
		}

		void Dashing()
		{
			lastDash += Time.deltaTime;

			if (Input.GetAxisRaw(this.tag + " dash") != 0f && Input.GetAxisRaw(this.tag) != 0f)
			{
				dash();
			}
		}

		void dash()
		{
			if (!isDashing && lastDash > dashCooldown)
			{
				// Activate dashing
				isDashing = true;

				dashLerp = 0;
				dashStartPosition = this.transform.position.y;
				dashDirection = Direction(Input.GetAxisRaw(this.tag));

				this.GetComponent<SoundPool>().PlayRandom();
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

				if (currentSpeed == 0)
					currentSpeed = minMovementSpeed;

				currentSpeed = Mathf.Clamp(currentSpeed + accelerationSpeed * Mathf.Abs(Input.GetAxisRaw(this.tag)), 0f, maxMovementSpeed);
				moveDirection = Direction(Input.GetAxisRaw(this.tag));
			}
			else
			{
				currentSpeed = Mathf.Clamp(currentSpeed - decelerationSpeed, 0f, maxMovementSpeed);
				if (currentSpeed == 0f)
					moveDirection = 0f;
			}

			// Set temp position
			position += (currentSpeed * moveDirection) * Time.deltaTime;

			// Override at dashing
			if (isDashing)
			{
				dashLerp += dashSpeed * Time.deltaTime;
				position = Mathf.Lerp(dashStartPosition, dashStartPosition + dashDistance * dashDirection, dashLerp);
				currentSpeed = dashSpeed;

				if (dashLerp >= 1f)
				{
					isDashing = false;
					lastDash = 0f;
				}
			}

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

			// Set animation
			this.GetComponentInChildren<Animator>().SetFloat("Movement", currentSpeed);
			this.GetComponentInChildren<Animator>().SetInteger("Direction", (int)moveDirection);
			this.GetComponentInChildren<Animator>().SetFloat("Position", this.transform.position.y);
			this.GetComponentInChildren<Animator>().SetInteger("Input", (int)Direction(Input.GetAxisRaw(this.tag)));
			this.GetComponentInChildren<Animator>().SetBool("Dash", isDashing);

			// Set animation speed depending on move speed
			if (Input.GetAxisRaw(this.tag) != 0)
				this.GetComponentInChildren<Animator>().speed = currentSpeed / maxMovementSpeed;
			else
				this.GetComponentInChildren<Animator>().speed = 1f;
		}

		public void stopMovement()
		{
			// Set animation
			this.GetComponentInChildren<Animator>().speed = 1f;
			this.GetComponentInChildren<Animator>().SetFloat("Movement", 0f);
			this.GetComponentInChildren<Animator>().SetInteger("Direction", 0);
			this.GetComponentInChildren<Animator>().SetFloat("Position", this.transform.position.y);
			this.GetComponentInChildren<Animator>().SetInteger("Input", 0);
			this.GetComponentInChildren<Animator>().SetBool("Dash", false);
		}

		float Direction(float f)
		{
			if (f != 0f)
				f = Mathf.Sign(f);

			return f * (Convert.ToInt32(invertDirection) * -2 + 1);
		}
	}
}