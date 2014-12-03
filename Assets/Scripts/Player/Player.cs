using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Player : MonoBehaviour
	{

		public float movementSpeed = 5f;
		public bool smoothInput = false;

		public float playerCollisionOffset = 0.3f;

		public AnimatorOverrideController ninjaLeftController;
		public AnimatorOverrideController ninjaRightController;

		public Sprite shieldLeftSprite;
		public Sprite shieldRightSprite;

		public bool mirror = false;
		[HideInInspector] public int direction = 1;


		void Start()
		{
			if (mirror)
			{
				// Mirror the Player
				direction = -1;
				Vector3 scale = this.transform.localScale;
				this.transform.localScale = new Vector3(scale.x * -1, scale.y);
			}

			// set different sprites for each player
			if (this.tag == "PlayerLeft")
			{
				this.GetComponent<Animator>().runtimeAnimatorController = ninjaLeftController;
				this.transform.FindChild("Shield").GetComponent<SpriteRenderer>().sprite = shieldLeftSprite;
			}
			else if (this.tag == "PlayerRight")
			{
				this.GetComponent<Animator>().runtimeAnimatorController = ninjaRightController;
				this.transform.FindChild("Shield").GetComponent<SpriteRenderer>().sprite = shieldRightSprite;
			}
		}

		void Update() 
		{
			Move();
		}

		void Move()
		{
			// Get current position
			Vector2 position = this.transform.position;

			// Smooth or Raw input
			if (smoothInput)
			{
				position.y = position.y + movementSpeed * Input.GetAxis(this.tag) * Time.deltaTime;
				this.GetComponentInChildren<Animator>().SetFloat("Move", Input.GetAxis(this.tag));
			}
			else
			{
				position.y = position.y + movementSpeed * Input.GetAxisRaw(this.tag) * Time.deltaTime;
				this.GetComponentInChildren<Animator>().SetFloat("Move", Input.GetAxisRaw(this.tag));
			}

			// Set animation speed depending on move speed
			if (Mathf.Abs(Input.GetAxisRaw(this.tag)) > 0)
			{
				this.GetComponentInChildren<Animator>().speed = Mathf.Abs(Input.GetAxis(this.tag));
			}
			else
			{
				this.GetComponentInChildren<Animator>().speed = 1f;
			}

			// Check and Precalculating Collision
			GameObject top = GameObject.FindGameObjectWithTag("BoundaryTop");
			GameObject bottom = GameObject.FindGameObjectWithTag("BoundaryBottom");
			if (position.y > top.transform.position.y - top.GetComponent<BoxCollider2D>().size.y / 2f - playerCollisionOffset)
			{
				position.y = top.transform.position.y - top.GetComponent<BoxCollider2D>().size.y / 2f - playerCollisionOffset;
			}
			else if (position.y < bottom.transform.position.y + bottom.GetComponent<BoxCollider2D>().size.y / 2f + playerCollisionOffset)
			{
				position.y = bottom.transform.position.y + bottom.GetComponent<BoxCollider2D>().size.y / 2f + playerCollisionOffset;
			}

			// Set the new position
			this.transform.position = new Vector2(position.x, position.y);
		}

		// --- Forward animation event (ae) ---
		public void ae_Shoot()
		{
			this.GetComponentInChildren<PlayerAttack>().Shoot();
		}
	}
}

